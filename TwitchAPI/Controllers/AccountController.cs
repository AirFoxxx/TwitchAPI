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
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ITwitchRepository _repository;
        private readonly App _app;

        public AccountController(UserManager<ApplicationUser> userManager, IHttpClientFactory clientFactory, SignInManager<ApplicationUser> signInManager,
           ITwitchRepository context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _clientFactory = clientFactory;
            _repository = context;
            _app = _repository.GetApp();
        }

        public async Task<ApplicationUser> GetCurrentUserAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            return user;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Register() => View(new RegisterVM());

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM register)
        {
            if (!ModelState.IsValid) return View(register);

            var user = await _userManager.FindByEmailAsync(register.EmailAddress);
            if (user != null)
            {
                TempData["Error"] = "This email address is already in use";
                return View(register);
            }

            var validationResult = await _userManager.PasswordValidators.FirstOrDefault().ValidateAsync(_userManager, user, register.Password);
            if (!validationResult.Succeeded)
            {
                TempData["Error"] = validationResult.Errors.ToList().Select(error => error.Description).Aggregate("", (first, next) => first + " " + next);
                return View(register);
            }

            var newUser = new ApplicationUser()
            {
                Email = register.EmailAddress,
                UserName = register.EmailAddress,
                ConnectedTwitch = false,
            };
            var newUserResponse = await _userManager.CreateAsync(newUser, register.Password);

            if (newUserResponse.Succeeded)
                await _userManager.AddToRoleAsync(newUser, ApplicationRoles.User);

            return View("RegisterCompleted");
        }

        public IActionResult TwitchLoginScopes() => View(new ScopeContainer());

        public async Task<IActionResult> TwitchRevalidateScopes()
        {
            var model = new ScopeContainer();
            // Test for DB loaded values when we implement Accounts
            var user = await GetCurrentUserAsync(User.Identity.Name);
            var twitchUser = _repository.GetUserByUserId(user.TwitchUserId);

            if (twitchUser == null)
            {
                return View("Failure",
                    "No user is logged in for this to work!");
            }

            foreach (var item in model.CheckBoxes)
            {
                item.PreviouslySelected = false;
                if (twitchUser.Scopes.ToList().Contains(item.Value))
                {
                    item.PreviouslySelected = true;
                }
            }

            return View("TwitchLoginScopes", model);
        }

        [AllowAnonymous]
        public IActionResult Login() => View(new LoginVM());

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM login)
        {
            if (!ModelState.IsValid) return View(login);

            var user = await _userManager.FindByEmailAsync(login.EmailAddress);
            if (user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, login.Password);
                if (passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, login.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    TempData["Error"] = "Wrong credentials!";
                    return View(login);
                }
            }

            TempData["Error"] = "Wrong credentials!";
            return View(login);
        }

        public IActionResult ScopeRegistration(IFormCollection collection)
        {
            try
            {
                var container = new ScopeContainer();

                container.ScopesFormatted = Request.Form["CategoryIds"];

                if (string.IsNullOrEmpty(container.ScopesFormatted))
                {
                    // Return invalid view back
                    ModelState.AddModelError("ScopeList", "Please select at least one!!!");
                    return View("TwitchLoginScopes", container);
                }

                // Build the link here
                var redirectUrl = "https://id.twitch.tv/oauth2/authorize"
                    + "?response_type=code" +
                    "&client_id=" + _app.ClientId +
                    "&redirect_uri=" + _app.RedirectURI +
                    "&scope=" + container.ScopesFormatted.Replace(",", "%20") +
                    "&state= " + _app.Token;

                return new RedirectResult(redirectUrl);
            }
            catch
            {
                return View("Failure", "Something did not go quite right with the form submition. Wanna try it again?");
            }
        }

        public async Task<IActionResult> Redirection(string code)
        {
            // Get OAUTH2 token for user
            var request = new HttpRequestMessage(HttpMethod.Post, "https://id.twitch.tv/oauth2/token"
                + "?client_id=" + _app.ClientId
                + "&client_secret=" + _app.ClientSecret
                + "&code=" + code
                + "&grant_type=authorization_code"
                + "&redirect_uri=" + _app.RedirectURI);

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            var newUser = new User();
            newUser.OAuthCode = code;

            if (response.IsSuccessStatusCode)
            {
                var userTokenObject = await response.Content.ReadFromJsonAsync<UserToken>();

                newUser.UserToken = userTokenObject.AccessToken;
                newUser.RefreshToken = userTokenObject.RefreshToken;
                newUser.ExpiresIn = TimeSpan.FromSeconds(userTokenObject.ExpiresIn);

                // Get user ID
                var userIDrequest = new HttpRequestMessage(HttpMethod.Get,
                "https://api.twitch.tv/helix/users");
                userIDrequest.Headers.Add("Authorization", "Bearer " + newUser.UserToken);
                userIDrequest.Headers.Add("Client-Id", _app.ClientId);

                var userResponse = await client.SendAsync(userIDrequest);

                var users = await userResponse.Content.ReadFromJsonAsync<UserInfos>();
                var user = users.UserInfoList.FirstOrDefault();
                if (user != null)
                {
                    newUser.UserId = user.Id;

                    // Save TwitchUserId to Identity object
                    var identityUser = await GetCurrentUserAsync(User.Identity.Name);

                    // Assign the fetched Id and other params
                    identityUser.TwitchUserId = user.Id;
                    identityUser.ConnectedTwitch = true;

                    // Assign new validated role
                    await _userManager.AddToRoleAsync(identityUser, ApplicationRoles.TwitchValidatedUser);
                    // await _userManager.RemoveFromRoleAsync(identityUser, ApplicationRoles.User);

                    // What if user is already in the database?
                    var dbUser = _repository.GetUserByUserId(identityUser.TwitchUserId);
                    if (dbUser != null)
                    {
                        dbUser.ExpiresIn = newUser.ExpiresIn;
                        dbUser.OAuthCode = newUser.OAuthCode;
                        dbUser.UserToken = newUser.UserToken;
                        dbUser.UserId = identityUser.TwitchUserId;
                        dbUser.Scopes = userTokenObject.Scope.ToArray();
                        _repository.SaveChanges();
                    }
                    else
                    {
                        // dbUser is NULL
                        newUser.Scopes = userTokenObject.Scope.ToArray();
                        _repository.CreateUser(newUser);
                        _repository.SaveChanges();
                    }
                }
                else
                {
                    // Getting a user ID for this user failed!
                    return View("Failure",
                        "User ID retrieval for this user failed! Try to change available scopes.");
                }
            }
            else
            {
                // OAUTH2 validation failed!
                return View("Failure", "OAUTH2 token retrieval for this user failed! Try to validate this app again.");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied(string ReturnUrl)
        {
            return View();
        }
    }
}
