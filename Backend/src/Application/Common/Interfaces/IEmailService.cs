namespace Application.Common.Interfaces;

public interface IEmailService
{
    void SendMail(string userEmail, string subject, string message);
    EmailDto CreateMailForAccountVerification(UserDto user);
    EmailDto CreateMailForResetPassword(UserDto user);
}
