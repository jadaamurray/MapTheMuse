using MapTheMuseApi.Dtos;
using MapTheMuseApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapTheMuseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly EmailService _emailService;
        public AccountController(UserManager<AppUser> userManager,
       SignInManager<AppUser> signInManager, EmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
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
        public async Task<IActionResult> Login(Auth model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password,
           isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return Ok("Login successful.");
            }
            return Unauthorized("Invalid login attempt.");
        }
    }
}