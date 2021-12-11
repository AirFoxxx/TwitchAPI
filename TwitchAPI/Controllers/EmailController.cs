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
    public class EmailController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ITwitchRepository _repository;
        private readonly IEmailSender _emailSender;
        private readonly App _app;

        public EmailController(UserManager<ApplicationUser> userManager, IHttpClientFactory clientFactory, SignInManager<ApplicationUser> signInManager,
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
        public async Task<IActionResult> EmailVerification(ApplicationUser user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Email", new { token, email = user.Email }, Request.Scheme);
            var message = new Message(new string[] { user.Email }, "Confirmation email link", confirmationLink, null);
            await _emailSender.SendEmailAsync(message);
            return RedirectToAction(nameof(SuccessRegistration));
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return View("Error");
            var result = await _userManager.ConfirmEmailAsync(user, token);
            return View(result.Succeeded ? nameof(ConfirmEmail) : "Error");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult SuccessRegistration()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult SendEmailTest()
        {
            var rng = new Random();
            var message = new Message(new string[] { "kociandreas@seznam.cz" }, "Test email", "This is the content from our email.");
            _emailSender.SendEmail(message);
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            var rng = new Random();
            var files = Request.Form.Files.Any() ? Request.Form.Files : new FormFileCollection();
            var message = new Message(new string[] { "codemazetest@mailinator.com" }, "Test mail with Attachments", "This is the content from our mail with attachments.", files);
            await _emailSender.SendEmailAsync(message);
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
