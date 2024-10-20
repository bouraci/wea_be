﻿using Microsoft.AspNetCore.Mvc;
using WEA_BE.DTO;
using WEA_BE.Services;

namespace WEA_BE.Controllers;

public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
    {

        if (string.IsNullOrWhiteSpace(registerRequestDto.Name) || string.IsNullOrWhiteSpace(registerRequestDto.UserName))
        {
            return BadRequest("Name or Username cannot be empty or just whitespace.");
        }

        if (registerRequestDto.Password.Any(char.IsWhiteSpace))
        {
            return UnprocessableEntity("Password cannot contain whitespace characters.");
        }

        if (registerRequestDto.Password.Length < 8)
        {
            return UnprocessableEntity("Password must be at least 8 characters long.");
        }

        bool result = await _authService.RegisterAsync(registerRequestDto.Name, registerRequestDto.UserName, registerRequestDto.Password);
        if (result)
        {
            return StatusCode(201, "User registered successfully.");
        }

        return Conflict("User already exists.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        UserDto? result = await _authService.LoginAsync(loginRequestDto.UserName, loginRequestDto.Password);
        if (result is not null)
        {
            return Ok(result);
        }

        return Unauthorized("Invalid username or password.");
    }
}
