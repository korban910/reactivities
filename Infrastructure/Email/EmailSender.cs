using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Resend;

namespace Infrastructure.Email;

public class EmailSender(IServiceScopeFactory scopeFactory) : IEmailSender<User>
{
    public async Task SendConfirmationLinkAsync(
        User user, 
        string email, 
        string confirmationLink)
    {
        var subject = "Confirm your email address";
        var body = $@"
            <p>Hi {user.DisplayName}</p>
            <p>Please confirm your email by clicking the link below</p>
            <p><a href='{confirmationLink}'>Click here to verify email<a/></p>
            <p>Thanks</p>
        ";
        
        await SendMailAsync(email, subject, body);
    }

    public Task SendPasswordResetLinkAsync(User user, string email, string resetLink)
    {
        throw new NotImplementedException();
    }

    public async Task SendPasswordResetCodeAsync(User user, string email, string resetCode)
    {
        var subject = "Reset your password";
        var body = $@"
            <p>Hi {user.DisplayName}</p>
            <p>Please click the below link to reset your password</p>
            <p>
                <a href='{Environment.GetEnvironmentVariable("CLIENT_APP_URL")!}/reset-password?email={email}&code={resetCode}'>
                    Click to reset your password
                <a/>
            </p>
            <p>If you did not request this, you can ignore this email</p>
        ";
        
        await SendMailAsync(email, subject, body);
    }
    
    private async Task SendMailAsync(
        string email, 
        string subject, 
        string body)
    {
        using var scope = scopeFactory.CreateScope();
        var resend = scope.ServiceProvider.GetRequiredService<IResend>();
        
        var message = new EmailMessage()
        {
            From = Environment.GetEnvironmentVariable("EMAIL_MESSAGE_FROM")!,
            Subject = subject,
            HtmlBody = body,
        };
        
        message.To.Add(email);

        Console.WriteLine(message.HtmlBody);
        
        await resend.EmailSendAsync(message);
        await Task.CompletedTask;
    }
}