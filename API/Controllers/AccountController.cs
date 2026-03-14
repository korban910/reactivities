using System.Text;
using Contracts.Account.Requests;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(
    SignInManager<User> signInManager,
    IEmailSender<User> emailSender) : BaseApiController
{
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult> RegisterUser(RegisterAccountRequest request)
    {
        var user = new User
        {
            UserName = request.Email,
            Email = request.Email,
            DisplayName = request.DisplayName,
        };

        var result = await signInManager.UserManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            await SendConfirmationLinkAsync(user, request.Email);
            return Ok();
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(error.Code,  error.Description);
        }

        return ValidationProblem();
    }

    [AllowAnonymous]
    [HttpGet("resend-confirm-email")]
    public async Task<ActionResult> ResendConfirmEmail(string? email, string? userId)
    {
        if (string.IsNullOrEmpty(email) && string.IsNullOrEmpty(userId))
        {
            return BadRequest("Email or UserId must be provided");
        }
        
        var user = await signInManager.UserManager.Users.FirstOrDefaultAsync(
            x => x.Email == email || x.Id == userId);

        if (user == null || string.IsNullOrEmpty(user.Email))
        {
            return BadRequest("User not found");
        }
        
        await SendConfirmationLinkAsync(user, user.Email);
        
        return Ok();
    }
    

    private async Task SendConfirmationLinkAsync(
        User user, 
        string email)
    {
        var code = await signInManager.UserManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code)); // URL safe to protect special characters
        
        var confirmEmailUrl = $"{Environment.GetEnvironmentVariable("CLIENT_APP_URL")}/confirm-email?code={code}&userId={user.Id}";
        
        await emailSender.SendConfirmationLinkAsync(user, email, confirmEmailUrl);
    }

    [AllowAnonymous]
    [HttpGet("user-info")]
    public async Task<ActionResult> GetUserInfo()
    {
        if (User.Identity?.IsAuthenticated == false) return NoContent();
        
        var user = await signInManager.UserManager.GetUserAsync(User);

        if (user == null)
        {
            return Unauthorized();
        }

        return Ok(new
        {
            user.DisplayName,
            user.Email,
            user.Id,
            user.ImageUrl,
        });
    }

    [HttpPost("logout")]
    public async Task<ActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        
        return NoContent();
    }
}