using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MapTheMuseApi.Models;
using MapTheMuseApi.Dtos;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/me")]
[Authorize]
public class MeController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    public MeController(UserManager<AppUser> userManager) => _userManager = userManager;

    [HttpGet]
    public async Task<ActionResult<UserProfileDto>> Get()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound();

        var roles = await _userManager.GetRolesAsync(user);
        return Ok(new UserProfileDto {
            Id = user.Id,
            UserName = user.UserName ?? "",
            Email = user.Email ?? "",
            FirstName = user.FirstName,
            LastName  = user.LastName,
            ProfilePictureUrl = user.ProfilePictureUrl,
            Country = user.Country,
            PreferredLanguage = user.PreferredLanguage,
            Roles = roles
        });
    }

    // allow the user to update their own profile
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UserUpdateDto dto)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound();

        user.FirstName = dto.FirstName ?? user.FirstName;
        user.LastName  = dto.LastName  ?? user.LastName;
        user.ProfilePictureUrl = dto.ProfilePictureUrl ?? user.ProfilePictureUrl;
        user.Country   = dto.Country   ?? user.Country;
        user.PreferredLanguage = dto.PreferredLanguage ?? user.PreferredLanguage;

        if (!string.IsNullOrWhiteSpace(dto.UserName) && dto.UserName != user.UserName)
            user.UserName = dto.UserName;
        if (!string.IsNullOrWhiteSpace(dto.Email) && dto.Email != user.Email)
            user.Email = dto.Email;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded) return BadRequest(result.Errors);
        return NoContent();
    }
}
