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
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;


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
        private readonly IWebHostEnvironment _env;
        public AccountController(UserManager<AppUser> userManager,
       SignInManager<AppUser> signInManager, EmailService emailService, IConfiguration config, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _config = config;
            _env = env;

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

            try
            {
                var result = await _userManager.CreateAsync(user, dto.Password);

                if (result.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var verificationLink = Url.Action("VerifyEmail", "Account", new
                    {
                        userId = user.Id,
                        token = token
                    }, Request.Scheme);

                    var emailSubject = "Email Verification";
                    var emailBody = $"Please verify your email by clicking the following link: {verificationLink}";
                    await _emailService.SendEmailAsync(user.Email, emailSubject, emailBody);

                    return Ok("User registered successfully. An email verification link has been sent.");
                }

                // Convert IdentityErrors to field-based structure
                var fieldErrors = result.Errors
                    .GroupBy(e =>
                        e.Code.Contains("Email") ? "email" :
                        e.Code.Contains("Password") ? "password" :
                        e.Code.Contains("UserName") ? "username" :
                        "general")
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.Description).ToArray()
                    );

                return BadRequest(new { errors = fieldErrors });
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("idx_users_email_unique") == true)
            {
                // Handle unique email constraint from database
                return BadRequest(new
                {
                    errors = new Dictionary<string, string[]>
            {
                { "email", new[] { "An account with this email already exists." } }
            }
                });
            }
        }


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
        };

            // Include role claims
            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            // Create the signing key
            var keyBytes = Encoding.UTF8.GetBytes(_config["Jwt:Key"]!);
            var creds = new SigningCredentials(
                new SymmetricSecurityKey(keyBytes),
                SecurityAlgorithms.HmacSha256);

            var expires = DateTime.UtcNow
                        .AddDays(double.Parse(_config["Jwt:ExpiryDays"]!));

            // Build the JWT
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            var cookie = new CookieOptions
            {
                HttpOnly = true,
                Path = "/",
                Expires = expires
            };

            if (_env.IsDevelopment())
            {
                // HTTP localhost → same-site 
                cookie.SameSite = SameSiteMode.Lax;
                cookie.Secure = false;
            }
            else
            {
                // Production: cross-site friendly + HTTPS only
                cookie.SameSite = SameSiteMode.None;
                cookie.Secure = true;
            }

            Response.Cookies.Append("authToken", jwt, cookie);

            return Ok(new { message = "Login successful" });
        }
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("authToken");
            return Ok(new { message = "Logged out" });
        }
        // GET: api/account/me
        [HttpGet("me")]
        [Authorize]
        public IActionResult GetCurrentUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                         User.FindFirstValue(JwtRegisteredClaimNames.Sub); // in case you're using Sub

            var email = User.FindFirstValue(ClaimTypes.Email) ??
                        User.FindFirstValue(JwtRegisteredClaimNames.Email);

            var roles = User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            return Ok(new
            {
                userId,
                email,
                roles
            });
        }
    }
}