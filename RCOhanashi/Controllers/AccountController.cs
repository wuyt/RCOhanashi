using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RCOhanashi.Models;
using RCOhanashi.Models.AccountViewModels;

namespace RCOhanashi.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        [Route("Login")]
        public async Task<string> Login([FromBody]LoginViewModel model)
        {
           // ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    //_logger.LogInformation("User logged in.");
                    return "User logged in.";
                }

                if (result.IsLockedOut)
                {
                    return "User account locked out.";
                }
                else
                {
                    return "Invalid login attempt.";
                }
            }

            // If we got this far, something failed, redisplay form
            return "something failed";
        }

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        [Route("Register")]
        public async Task<string> Register([FromBody]RegisterViewModel model)
        {
            //ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "normal");
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return "User created a new account with password.";
                }
            }

            return "something failed";
        }

        [HttpGet]
        [Route("LoginOut")]
        public async Task<string> Logout()
        {
            await _signInManager.SignOutAsync();
            return "User logged out.";
        }
    }
}
