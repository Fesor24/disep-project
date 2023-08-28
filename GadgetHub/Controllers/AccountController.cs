﻿using GadgetHub.Entities.Identity;
using GadgetHub.Services.Abstractions;
using GadgetHub.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GadgetHub.Controllers;
public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenService _tokenService;

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
        ITokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }
    public IActionResult Login(string returnUrl = null)
    {
        LoginViewModel lvm = new();

        lvm.ReturnUrl = returnUrl;

        return View(lvm);
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        model.ReturnUrl ??= "~/";

        var user = await _userManager.FindByEmailAsync(model.Email);

        if(user is null)
        {
            ViewData["errorMessage"] = "Unauthorized";
            return View(model);
        }

        var claims = new List<Claim>
            {
                new Claim("dsp", user.DisplayName),
                new Claim(ClaimTypes.Email, user.Email)
            };

        var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);

        if (!signInResult.Succeeded)
        {
            ViewData["errorMessage"] = "Unauthorized";
            return View(model);
        }

        await _signInManager.SignInWithClaimsAsync(user, true, claims);

        return LocalRedirect(model.ReturnUrl);
    }

    public IActionResult Register()
    {
        RegisterViewModel rvm = new();

        return View(rvm);
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("Register", model);
        }

        var user = new ApplicationUser
        {
            Email = model.Email,
            UserName = model.Email,
            DisplayName = model.DisplayName
        };

        var identityResult = await _userManager.CreateAsync(user, model.Password);

        if (identityResult.Succeeded)
        {
            _tokenService.CreateToken(user);

            var claims = new List<Claim>
            {
                new Claim("dsp", user.DisplayName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            await _signInManager.SignInWithClaimsAsync(user, true, claims);

            return LocalRedirect("~/");
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();

        return LocalRedirect("~/");
    }
}
