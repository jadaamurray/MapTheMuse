using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MapTheMuseApi.Models;
using MapTheMuseApi.Dtos;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

[ApiController]
[Route("api/me")]
[Authorize]
public class MeController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly EmailService _emailService;

    public MeController(UserManager<AppUser> userManager, EmailService emailService)
    {
        _userManager = userManager;
        _emailService = emailService;
    }
    [HttpGet]
    public async Task<ActionResult<UserProfileDto>> Get()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound();

        var roles = await _userManager.GetRolesAsync(user);
        return Ok(ToProfileDto(user, roles));
    }

    // allow the user to update their own profile
    [HttpPatch]
    public async Task<IActionResult> Patch([FromBody] JsonPatchDocument<UserProfilePatchDto> patch)
    {
        if (patch is null) return BadRequest("Missing patch document.");

        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound();

        // Project current values -> patch DTO
        var dto = new UserProfilePatchDto
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            ProfilePictureUrl = user.ProfilePictureUrl,
            Country = user.Country,
            PreferredLanguage = user.PreferredLanguage
        };

        // Apply operations (validates paths etc.)
        patch.ApplyTo(dto, ModelState);
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        // Map back to entity
        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.ProfilePictureUrl = dto.ProfilePictureUrl;
        user.Country = dto.Country;
        user.PreferredLanguage = dto.PreferredLanguage;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded) return ProblemFromIdentity(result);

        var roles = await _userManager.GetRolesAsync(user);
        return Ok(ToProfileDto(user, roles));
    }

    // Email Change
    [HttpPost("email")]
    public async Task<IActionResult> StartEmailChange([FromBody] ChangeEmailRequest dto)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound();

        if (string.Equals(user.Email, dto.NewEmail, StringComparison.OrdinalIgnoreCase))
            return BadRequest(new { error = "EmailUnchanged", message = "That’s already your email." });
    
        var normalised = _userManager.NormalizeEmail(dto.NewEmail);

        // Re-check uniqueness in case it was taken after the request was sent
        var taken = await _userManager.Users
            .AnyAsync(u => u.Id != user.Id && u.NormalizedEmail == normalised);

        if (taken)
        {
            return Conflict(new
            {
                error = "EmailAlreadyInUse",
                message = "That email address is already registered."
            });
        }

        var rawToken = await _userManager.GenerateChangeEmailTokenAsync(user, dto.NewEmail);
        var token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(rawToken));

        // Send confirmation link to the NEW email
         var callbackUrl =
            $"https://mapthemuse.world/account/confirm-email-change?userId={Uri.EscapeDataString(user.Id)}" +
            $"&email={Uri.EscapeDataString(dto.NewEmail)}&token={Uri.EscapeDataString(token)}";


        await _emailService.SendEmailAsync(dto.NewEmail,
            subject: "Confirm your email change",
            body: $"Click to confirm: <a href=\"{callbackUrl}\">confirm</a>");

        return Accepted(new { message = "Confirmation sent." });
    }

    [AllowAnonymous]
    [HttpPost("email/confirm")]
    public async Task<IActionResult> ConfirmEmailChange([FromBody] ConfirmEmailChangeRequest dto)
    {
        var user = await _userManager.FindByIdAsync(dto.UserId);
        if (user == null) return NotFound();

        var normalised = _userManager.NormalizeEmail(dto.Email);

        // check uniqueness again in case it was taken after the request was sent
        var taken = await _userManager.Users
            .AnyAsync(u => u.Id != user.Id && u.NormalizedEmail == normalised);

        if (taken)
        {
            return Conflict(new
            {
                error = "EmailAlreadyInUse",
                message = "That email address is already registered."
            });
        }

        var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(dto.Token));
        var result = await _userManager.ChangeEmailAsync(user, dto.Email, decodedToken);
        if (!result.Succeeded) return ProblemFromIdentity(result);

        return NoContent();
    }

    // Username change
    [HttpPost("username")]
    public async Task<IActionResult> ChangeUserName([FromBody] ChangeUserNameRequest dto)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound();

        var desired = (dto.NewUserName ?? "").Trim().ToLowerInvariant();

        if (string.Equals(user.UserName, desired, StringComparison.OrdinalIgnoreCase))
            return BadRequest(new { error = "UserNameUnchanged", message = "That’s already your username." });

        if (desired.Length < 3 || desired.Length > 32)
            return BadRequest(new { error = "InvalidUserName", message = "Username must be 3–32 characters." });

        var normalised = _userManager.NormalizeName(desired);
        var exists = await _userManager.Users
            .AnyAsync(u => u.Id != user.Id && u.NormalizedUserName == normalised);
        if (exists)
            return Conflict(new { error = "UserNameTaken", message = "That username is already in use." });

        var result = await _userManager.SetUserNameAsync(user, desired);
        if (!result.Succeeded) return ProblemFromIdentity(result);

        return NoContent();
    }

    // Password change
    [HttpPost("password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest dto)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound();

        var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
        if (!result.Succeeded) return ProblemFromIdentity(result);

        return NoContent();
    }

    // helpers
    private static UserProfileDto ToProfileDto(AppUser user, IList<string> roles) => new()
    {
        Id = user.Id,
        UserName = user.UserName ?? "",
        Email = user.Email ?? "",
        FirstName = user.FirstName,
        LastName = user.LastName,
        ProfilePictureUrl = user.ProfilePictureUrl,
        Country = user.Country,
        PreferredLanguage = user.PreferredLanguage,
        Roles = roles
    };
    private ActionResult ProblemFromIdentity(IdentityResult result)
    {
        foreach (var e in result.Errors)
            ModelState.AddModelError(e.Code ?? "Identity", e.Description);
        return ValidationProblem(ModelState);
    }
}
