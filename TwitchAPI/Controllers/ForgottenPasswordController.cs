using EmailService;
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
using System.Threading.Tasks;
using TwitchAPI.Data;
using TwitchAPI.Data.Static;
using TwitchAPI.Models;
using TwitchAPI.Models.AppUsers;
using TwitchAPI.ViewModels;

namespace TwitchAPI.Controllers
{
    [Authorize]
    public class ForgottenPasswordController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ITwitchRepository _repository;
        private readonly IEmailSender _emailSender;
        private readonly App _app;

        public ForgottenPasswordController(UserManager<ApplicationUser> userManager, IHttpClientFactory clientFactory, SignInManager<ApplicationUser> signInManager,
           IEmailSender emailSender, ITwitchRepository context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _clientFactory = clientFactory;
            _repository = context;
            _emailSender = emailSender;
            _app = _repository.GetApp();
        }

        public async Task<ApplicationUser> GetCurrentUserAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            return user;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgottenPasswordVM email)
        {
            // Verify email validity
            if (!ModelState.IsValid)
                return View(email);

            // Email might be invalid
            var user = await _userManager.FindByEmailAsync(email.EmailAddress);
            if (user == null)
            {
                TempData["Error"] = "There is no account for this email address!";
                return View(email);
            }

            // Send email with password generation website link
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callback = Url.Action(nameof(ResetPassword), "ForgottenPassword", new { token, email = user.Email }, Request.Scheme);
            var message = new Message(new string[] { user.Email }, "Reset password token", callback, null);
            await _emailSender.SendEmailAsync(message);
            return RedirectToAction(nameof(ForgotPasswordConfirmation));
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            var model = new ResetPasswordVM { Token = token, Email = email };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPasswordVM)
        {
            if (!ModelState.IsValid)
                return View(resetPasswordVM);

            var user = await _userManager.FindByEmailAsync(resetPasswordVM.Email);
            if (user == null)
            {
                return View("Failure", "Something went wrong! Please try to generate new Password email link!");
            }

            var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPasswordVM.Token, resetPasswordVM.Password);
            if (!resetPassResult.Succeeded)
            {
                foreach (var error in resetPassResult.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return View();
            }
            return RedirectToAction(nameof(ResetPasswordConfirmation));
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Error()
        {
            return View();
        }
    }
}
