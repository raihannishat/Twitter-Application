using Infrastructure.Common;

namespace Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly string _address;
    private readonly string _password;

    public EmailService(IConfiguration configuration)
    {
        _address = configuration.GetSection("Email:Address").Value;
        _password = configuration.GetSection("Email:Password").Value;
    }

    public void SendMail(string userEmail, string subject, string message)
    {
        var email = new MimeMessage();
        
        email.From.Add(new MailboxAddress(EmailConstant.MailBoxName, _address));
        
        email.To.Add(MailboxAddress.Parse(userEmail));
        
        email.Subject = subject;
        
        email.Body = new TextPart(TextFormat.Html) { Text = message };

        var smtp = new SmtpClient();

        smtp.Connect(EmailConstant.EmailHost, 465, true);
       
        smtp.Authenticate(_address, _password);
        
        smtp.Send(email);
        
        smtp.Disconnect(true);
    }
    public EmailDto CreateMailForAccountVerification(UserDto user)
    {
        var emailDto = new EmailDto
        {
            Subject = EmailConstant.EmailSubject,
            Message = $"<h2> Hey {user.Name},</h2>"
            + "<h3>Welcome to Twitter. (DIU_Raizor)</h3>"
            + "<p>Your Account Verfication Code </p>"
                + "<h2>" + user.VerificationToken + "</h2>"
                + "<p>For more information. Contact with us </p> </br>"
            + "<p>Asif Abdullah [01755808860], Raihan Nishat [01710512211] </p> </br>"
            + "<p>Department of Software Engineering.</p> </br>"
            + "<p>Daffodil Internation University.</p> </br>"
        };

        return emailDto;
    }

    public EmailDto CreateMailForResetPassword(UserDto user)
    {
        var emailDto = new EmailDto
        {
            Subject = "Password reset",
            Message = $"<h2>Hey {user.Name},</h2>" +
            "<p> We received a request to reset the password on your Twitter Account</p>"
            + $"<h2>{user.PasswordResetToken} </h2>" +
            "<p>Enter this code to complete the reset </p> </br"
                + "<p>For more information. Contact with us </p> </br>"
            + "<p>Asif Abdullah [01755808860], Raihan Nishat [01710512211] </p> </br>"
            + "<p>Department of Software Engineering.</p> </br>"
            + "<p>Daffodil Internation University.</p> </br>"
        };

        return emailDto;
    }

}
