using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using mylittle_project.Application.DTOs;
using mylittle_project.Domain.Entities;
using System.Text.Encodings.Web;

namespace mylittle_project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (model == null) return BadRequest("Invalid request body");
            if (model.Password != model.ConfirmPassword)
                return BadRequest(new { error = "Password and Confirm Password do not match." });

            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Auth", new { userId = user.Id, token }, Request.Scheme);

            await _emailSender.SendEmailAsync(model.Email, "Confirm your email",
                $"<h2>Welcome!</h2><p>Click to confirm: <a href='{HtmlEncoder.Default.Encode(confirmationLink!)}'>Confirm Email</a></p>");

            return Ok(new { message = "User registered. Please confirm your email.", userId = user.Id });
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded ? Ok("Email confirmed") : BadRequest("Failed to confirm email");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
                return Unauthorized("Invalid username or password");

            var result = await _signInManager.PasswordSignInAsync(
                model.Username, model.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return Ok(new { message = "Login successful", username = user.UserName, userId = user.Id });
            }

            return Unauthorized("Invalid login attempt");
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok("Logged out");
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto model)
        {
            if (model == null) return BadRequest("Invalid request body");

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                return Ok(); // Don't reveal user existence

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = Url.Action("ResetPassword", "Auth", new { token, email = model.Email }, Request.Scheme);

            await _emailSender.SendEmailAsync(model.Email, "Reset Password",
                $"<p>Click below to reset password:</p><a href='{HtmlEncoder.Default.Encode(resetLink!)}'>Reset Password</a>");

            return Ok("Password reset link sent.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            if (model == null) return BadRequest("Invalid request body");

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return NotFound();

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            return result.Succeeded ? Ok("Password reset successful") : BadRequest("Password reset failed");
        }

        [HttpPost("enable-2fa")]
        public async Task<IActionResult> Enable2FA([FromBody] Enable2FADto model)
        {
            if (model == null) return BadRequest("Invalid request body");

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return NotFound("User not found");

            await _userManager.SetTwoFactorEnabledAsync(user, true);
            return Ok("Two-Factor Authentication enabled.");
        }

        [HttpPost("verify-2fa")]
        public async Task<IActionResult> Verify2FA([FromBody] TwoFactorDto model)
        {
            if (model == null) return BadRequest("Invalid request body");

            var result = await _signInManager.TwoFactorSignInAsync("Email", model.Code, false, false);
            return result.Succeeded ? Ok("2FA successful") : Unauthorized("2FA failed");
        }
    }
}
