using MapTheMuseApi.Dtos;
using MapTheMuseApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NuGet.Packaging.Signing;


namespace MapTheMuseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly EmailService _emailService;
        private readonly IConfiguration _config;
        public AccountController(UserManager<AppUser> userManager,
       SignInManager<AppUser> signInManager, EmailService emailService, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _config = config;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var user = new AppUser
            {
                Email = dto.Email,
                UserName = dto.UserName,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Country = dto.Country,
                PreferredLanguage = dto.PreferredLanguage
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            
            if (result.Succeeded)
            {
                // Generate an email verification token
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                // Create the verification link
                var verificationLink = Url.Action("VerifyEmail", "Account", new
                {
                    userId = user.Id,
                    token =
               token
                }, Request.Scheme);
                // Send the verification email
                var emailSubject = "Email Verification";
                var emailBody = $"Please verify your email by clicking the following link: {verificationLink}";
                await _emailService.SendEmailAsync(user.Email, emailSubject, emailBody);

                return Ok("User registered successfully. An email verification link has been sent.");
            }
            return BadRequest(result.Errors);
        }
        // Add an action to handle email verification
        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return Ok("Email verification successful.");
            }
            return BadRequest("Email verification failed.");
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            // Find the user
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
            return Unauthorized("Email or password invalid.");

        // Check password
        var result = await _signInManager.CheckPasswordSignInAsync(
            user, dto.Password, lockoutOnFailure: false);
        if (!result.Succeeded)
            return Unauthorized("Email or password invalid.");

        // Build the token claims
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            // you can add more claims here if you need
        };

        // Include role claims
        var roles = await _userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        // Create the signing key
        var keyBytes = Encoding.UTF8.GetBytes(_config["Jwt:Key"]!);
        var creds = new SigningCredentials(
            new SymmetricSecurityKey(keyBytes),
            SecurityAlgorithms.HmacSha256);

        // Set expiry
        var expires = DateTime.UtcNow
                    .AddDays(double.Parse(_config["Jwt:ExpiryDays"]!));

        // 7) Build the JWT
        var token = new JwtSecurityToken(
            issuer:             _config["Jwt:Issuer"],
            audience:           _config["Jwt:Audience"],
            claims:             claims,
            expires:            expires,
            signingCredentials: creds
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(new { token = jwt });
    }
    }
}