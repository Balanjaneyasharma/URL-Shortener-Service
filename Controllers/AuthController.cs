using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApi.Domain.Interfaces;
using MyApi.Domain.Models;
using MyApi.Services;
using System.Security.Claims;

namespace MyApi.Controllers;

[AllowAnonymous]
[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IJwtService _jwtService;
    private readonly IConfiguration _config;

    public AuthController(
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IJwtService jwtService,
        IConfiguration config)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _jwtService = jwtService;
        _config = config;
    }

    // Step 1 — redirect user to Google login
    [HttpGet("google/login")]
    public IActionResult GoogleLogin()
    {
        var redirectUrl = Url.Action(nameof(GoogleCallback), "Auth");
        var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    // Step 2 — Google calls back here after user logs in
    [HttpGet("google/callback")]
    public async Task<IActionResult> GoogleCallback()
    {
        var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        if (!result.Succeeded)
            return Unauthorized("Google authentication failed.");

        var googleId = result.Principal.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var email = result.Principal.FindFirstValue(ClaimTypes.Email)!;
        var name = result.Principal.FindFirstValue(ClaimTypes.Name);

        // Find or create user
        var user = await _userRepository.GetByGoogleIdAsync(googleId);
        if (user is null)
        {
            user = await _userRepository.CreateAsync(new User
            {
                GoogleId = googleId,
                Email = email,
                Name = name
            });
        }

        // Generate tokens
        var accessToken = _jwtService.GenerateAccessToken(user);
        var refreshToken = _jwtService.GenerateRefreshToken();
        var hashedRefreshToken = _jwtService.HashToken(refreshToken);

        var refreshExpiryDays = int.Parse(_config["Authentication:Jwt:RefreshTokenExpiryDays"]!);
        await _refreshTokenRepository.CreateAsync(user.Id, hashedRefreshToken, DateTime.UtcNow.AddDays(refreshExpiryDays));

        // Return tokens to frontend
        return Ok(new
        {
            accessToken,
            refreshToken
        });
    }

    // Refresh access token
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
    {
        var hashedToken = _jwtService.HashToken(request.RefreshToken);
        var userId = await _refreshTokenRepository.GetUserIdByTokenAsync(hashedToken);

        if (userId is null)
            return Unauthorized("Invalid or expired refresh token.");

        if (!await _refreshTokenRepository.TryConsumeAsync(hashedToken))
            return Unauthorized("Invalid or expired refresh token.");

        var user = await _userRepository.GetByIdAsync(userId.Value);
        if (user is null)
            return Unauthorized("User not found.");

        // Rotate refresh token
        var newRefreshToken = _jwtService.GenerateRefreshToken();
        var newHashedToken = _jwtService.HashToken(newRefreshToken);

        var refreshExpiryDays = int.Parse(_config["Authentication:Jwt:RefreshTokenExpiryDays"]!);
        await _refreshTokenRepository.CreateAsync(user.Id, newHashedToken, DateTime.UtcNow.AddDays(refreshExpiryDays));

        var newAccessToken = _jwtService.GenerateAccessToken(user);

        return Ok(new
        {
            accessToken = newAccessToken,
            refreshToken = newRefreshToken
        });
    }

    // Logout — revoke refresh token
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
    {
        var hashedToken = _jwtService.HashToken(request.RefreshToken);
        await _refreshTokenRepository.RevokeAsync(hashedToken);
        return NoContent();
    }
}

public record RefreshRequest(string RefreshToken);
public record LogoutRequest(string RefreshToken);