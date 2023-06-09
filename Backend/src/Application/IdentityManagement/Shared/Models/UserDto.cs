﻿namespace Application.IdentityManagement.Shared.Models;

public class UserDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string VerificationToken { get; set; } = string.Empty;
    public string? PasswordResetToken { get; set; }
    public bool IsBlocked { get; set; }
}
