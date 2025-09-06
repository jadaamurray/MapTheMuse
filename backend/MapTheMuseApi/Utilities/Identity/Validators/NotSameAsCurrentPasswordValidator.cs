using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

public sealed class NotSameAsCurrentPasswordValidator<TUser> : IPasswordValidator<TUser>
    where TUser : class
{
    public async Task<IdentityResult> ValidateAsync(
        UserManager<TUser> manager, TUser user, string password)
    {
        // error if the provided new password equals the current one
        if (await manager.CheckPasswordAsync(user, password))
        {
            return IdentityResult.Failed(new IdentityError {
                Code = "PasswordUnchanged",
                Description = "Your new password must be different from your current password."
            });
        }

        return IdentityResult.Success;
    }
}
