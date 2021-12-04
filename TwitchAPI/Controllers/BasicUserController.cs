using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TwitchAPI.Models;
using System.Net.Http.Json;
using TwitchAPI.ViewModels;
using TwitchAPI.Data;

namespace TwitchAPI.Controllers
{
    public class BasicUserController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ITwitchRepository _repository;
        private readonly App _app;

        public BasicUserController(ILogger<HomeController> logger, IHttpClientFactory clientFactory, ITwitchRepository repository)
        {
            _logger = logger;
            _clientFactory = clientFactory;
            _repository = repository;
            _app = _repository.GetApp();
        }

        public async Task<IActionResult> CustomUsersInfo()
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
            "https://api.twitch.tv/helix/users?login=kenji_CZ&login=hexy&login=sodapoppin");
            request.Headers.Add("Authorization", "Bearer " + _app.Token);
            request.Headers.Add("Client-Id", _app.ClientId);

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return View(await response.Content.ReadFromJsonAsync<UserInfos>());
            }
            else
            {
                return View();
            }
        }

        public async Task<IActionResult> FollowedStreams(string code)
        {
            // Get OAUTH2 token
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

                    // GET followed streams
                    var userStreamsrequest = new HttpRequestMessage(HttpMethod.Get,
                    "https://api.twitch.tv/helix/streams/followed" + "?user_id=" + newUser.UserId);
                    userStreamsrequest.Headers.Add("Authorization", "Bearer " + newUser.UserToken); // TODO: save to DB
                    userStreamsrequest.Headers.Add("Client-Id", _app.ClientId);

                    var userStreamsResponse = await client.SendAsync(userStreamsrequest);

                    if (userStreamsResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        // Can be because of supplied scope
                        var revalidateRequest = new HttpRequestMessage(HttpMethod.Post, "https://id.twitch.tv/oauth2/token"
                            + "?grant_type=refresh_token"
                            + "&refresh_token=" + newUser.RefreshToken // TODO: store in DB
                            + "&client_id=" + _app.ClientId
                            + "&client_secret=" + _app.ClientSecret);

                        var revalidationResponse = await client.SendAsync(revalidateRequest);

                        if (revalidationResponse.IsSuccessStatusCode)
                        {
                            var validationToken = await revalidationResponse.Content.ReadFromJsonAsync<RevalidatedUserToken>();

                            newUser.UserToken = validationToken.AccessToken;

                            // GET followed streams attempt 2
                            userStreamsrequest = new HttpRequestMessage(HttpMethod.Get,
                            "https://api.twitch.tv/helix/streams/followed" + "?user_id=" + newUser.UserId);
                            userStreamsrequest.Headers.Add("Authorization", "Bearer " + newUser.UserToken); // TODO: save to DB
                            userStreamsrequest.Headers.Add("Client-Id", _app.ClientId);

                            userStreamsResponse = await client.SendAsync(userStreamsrequest);
                            // GOTO: 120
                        }
                    }

                    var scopes = new List<Scope>();
                    scopes.Add(Scope.user_edit);
                    scopes.Add(Scope.user_read_blocked_users);

                    newUser.Scopes = scopes;
                    _repository.CreateUser(newUser);
                    _repository.SaveChanges();

                    var followedStreams = await userStreamsResponse.Content.ReadFromJsonAsync<FollowedStreams>();
                    // Request succeeded
                    if (followedStreams != null)
                    {
                        ViewBag.UserName = user.DisplayName;
                        return View(followedStreams);
                    }
                    else
                    {
                        return Error();
                    }
                }

                return Error();
            }
            else
            {
                return Error();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
