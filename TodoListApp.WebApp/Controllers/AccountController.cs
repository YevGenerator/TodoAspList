using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApp.Models;
using TodoListApp.WebApp.Services;

namespace TodoListApp.WebApp.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> userManager;
    private readonly SignInManager<IdentityUser> signInManager;
    private readonly IEmailService emailService;

    public AccountController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IEmailService emailService)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.emailService = emailService;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return this.View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        if (this.ModelState.IsValid)
        {
            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await this.userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await this.signInManager.SignInAsync(user, isPersistent: false);
                return this.RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                this.ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return this.View(model);
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        this.ViewData["ReturnUrl"] = returnUrl;
        return this.View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginModel model, string? returnUrl = null)
    {
        ArgumentNullException.ThrowIfNull(model);
        this.ViewData["ReturnUrl"] = returnUrl;
        if (this.ModelState.IsValid)
        {
            var result = await this.signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return this.RedirectToLocal(returnUrl);
            }
            else
            {
                this.ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return this.View(model);
            }
        }

        return this.View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await this.signInManager.SignOutAsync();
        return this.RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult ForgotPassword()
    {
        return this.View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return this.View();
        }

        var user = await this.userManager.FindByEmailAsync(email);
        if (user != null)
        {
            var token = await this.userManager.GeneratePasswordResetTokenAsync(user);

            var callbackUrl = this.Url.Action("ResetPassword", "Account", new { email = email, token = token }, protocol: this.HttpContext.Request.Scheme);

            if (callbackUrl != null)
            {
                var emailBody = $"Hello!<br/><br/>Please reset your password by <a href='{callbackUrl}'>clicking here</a>.";
                await this.emailService.SendEmailAsync(email, "Reset Password - To-Do List App", emailBody);
            }
        }

        return this.View("ForgotPasswordConfirmation");
    }

    [HttpGet]
    public IActionResult ResetPassword(string token, string email)
    {
        if (token == null || email == null)
        {
            return this.RedirectToAction("Index", "Home");
        }

        var model = new RegisterModel { Email = email };
        this.ViewBag.Token = token;
        return this.View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(RegisterModel model, string token)
    {
        ArgumentNullException.ThrowIfNull(model);

        if (!this.ModelState.IsValid)
        {
            return this.View(model);
        }

        var user = await this.userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            return this.RedirectToAction("Login");
        }

        var result = await this.userManager.ResetPasswordAsync(user, token, model.Password);
        if (result.Succeeded)
        {
            return this.RedirectToAction("Login");
        }

        foreach (var error in result.Errors)
        {
            this.ModelState.AddModelError(string.Empty, error.Description);
        }

        return this.View(model);
    }

    private IActionResult RedirectToLocal(string? returnUrl)
    {
        if (this.Url.IsLocalUrl(returnUrl))
        {
            return this.Redirect(returnUrl);
        }

        return this.RedirectToAction("Index", "Home");
    }
}
