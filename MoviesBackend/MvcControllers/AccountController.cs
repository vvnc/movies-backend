using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

using MoviesBackend.Services;

public class AccountController : Controller
{
  private readonly UserManager<IdentityUser> _userManager;
  private readonly SignInManager<IdentityUser> _signInManager;
  private readonly IMessageService _messageService;

  public AccountController(UserManager<IdentityUser> userManager,
                           SignInManager<IdentityUser> signInManager,
                           IMessageService messageService)
  {
    this._userManager = userManager;
    this._signInManager = signInManager;
    this._messageService = messageService;
  }

  public IActionResult Register()
  {
    return View();
  }

  [HttpPost]
  public async Task<IActionResult> Register(string email, string password, string repassword)
  {
    if (password != repassword)
    {
      ModelState.AddModelError(string.Empty, "Passwords don't match");
      return View();
    }

    var newUser = new IdentityUser
    {
      UserName = email,
      Email = email
    };

    var userCreationResult = await _userManager.CreateAsync(newUser, password);
    if (!userCreationResult.Succeeded)
    {
      foreach (var error in userCreationResult.Errors)
        ModelState.AddModelError(string.Empty, error.Description);
      return View();
    }

    var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
    var tokenVerificationUrl = Url.Action(
      "VerifyEmail",
      "Account",
      new
      {
        id = newUser.Id,
        token = emailConfirmationToken
      },
      Request.Scheme);

    await _messageService.Send(
      email,
      "Verify your email", $"Click <a href=\"{tokenVerificationUrl}\">here</a> to verify your email");

    return Content("Check your email for a verification link");
  }
}
