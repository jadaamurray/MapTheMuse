using System.ComponentModel.DataAnnotations;

public record ChangeUserNameRequest(
    [Required, MinLength(3), MaxLength(32),RegularExpression("^(?![.])(?!.*[._]{2})[a-zA-Z0-9._]{3,32}(?<![.])$",
        ErrorMessage = "Use lowercase letters, numbers, dot and underscore only.")] 
            string NewUserName);

public record ChangePasswordRequest(
    [Required] string CurrentPassword,
    [Required, MinLength(8)] string NewPassword);

public record ChangeEmailRequest(
    [Required, EmailAddress] string NewEmail);

public record ConfirmEmailChangeRequest(
    [Required] string UserId,
    [Required, EmailAddress] string Email,
    [Required] string Token);
