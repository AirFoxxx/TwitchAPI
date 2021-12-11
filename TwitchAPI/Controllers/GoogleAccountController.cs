using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using TwitchAPI.Data;
using TwitchAPI.Data.Static;
using TwitchAPI.Models;
using TwitchAPI.Models.AppUsers;
using TwitchAPI.ViewModels;

namespace TwitchAPI.Controllers
{
    [AllowAnonymous]
    public class GoogleAccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ITwitchRepository _repository;
        private readonly App _app;

        public GoogleAccountController(UserManager<ApplicationUser> userManager, IHttpClientFactory clientFactory, SignInManager<ApplicationUser> signInManager,
           ITwitchRepository context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _clientFactory = clientFactory;
            _repository = context;
            _app = _repository.GetApp();
        }

        [AllowAnonymous]
        public IActionResult GoogleLogin()
        {
            string redirectUrl = Url.Action("GoogleResponse", "GoogleAccount");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return new ChallengeResult("Google", properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> GoogleResponse()
        {
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return RedirectToAction("Login", "Account");

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            string[] userInfo = { info.Principal.FindFirst(ClaimTypes.Name).Value, info.Principal.FindFirst(ClaimTypes.Email).Value };
            if (result.Succeeded)
                return RedirectToAction("Index", "Home");
            else
            {
                var user = new ApplicationUser
                {
                    TwitchUserId = 15,
                    Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    UserName = info.Principal.FindFirst(ClaimTypes.Email)
                        .Value.Substring(0, info.Principal.FindFirst(ClaimTypes.Email).Value.IndexOf('@')),
                    ConnectedTwitch = false,
                };

                IdentityResult identResult = await _userManager.CreateAsync(user);
                if (identResult.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, ApplicationRoles.User);
                    identResult = await _userManager.AddLoginAsync(user, info);
                    if (identResult.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, false);
                        // Success
                        return RedirectToAction("Index", "Home");
                    }
                }
                return View("Failure",
                    "Access has been denied!");
            }
        }
    }
}
