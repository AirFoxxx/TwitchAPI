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

namespace TwitchAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _clientFactory;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Privacy()
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
            "https://api.twitch.tv/helix/users?login=kenji_CZ&login=hexy&login=sodapoppin");
            request.Headers.Add("Authorization", "Bearer 2yj97dk1uikb6f2ykkpj608rbx4u18");
            request.Headers.Add("Client-Id", "1uwdj9owa71a5prb3crveucdval8hp");

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return View("Privacy", await response.Content.ReadFromJsonAsync<UserInfos>());
            }
            else
            {
                return View("Privacy");
            }
        }

        public IActionResult Login()
        {
            return View();
        }

        public async Task<IActionResult> Redirection(string code)
        {
            var request = new HttpRequestMessage(HttpMethod.Post,
            "https://id.twitch.tv/oauth2/token");
            request.Headers.Add("client_id", "1uwdj9owa71a5prb3crveucdval8hp");
            request.Headers.Add("client_secret", "rvpkm2h35mvtw2s6dmv2eu7wn7gn81");
            request.Headers.Add("code", code);
            request.Headers.Add("grant_type", "authorization_code");
            request.Headers.Add("redirect_uri", "https://localhost:44367/Home/Redirection");

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return View("Index");
            }
            else
            {
                return View("Index");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
