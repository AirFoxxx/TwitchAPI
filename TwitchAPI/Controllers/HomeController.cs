using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TwitchAPI.Models;

using System.Net.Http;

using System.Net.Http.Json;

using System.Threading.Tasks;

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
            "https://api.twitch.tv/helix/users?login=kenji_CZ");
            request.Headers.Add("Authorization", "Bearer 3dzpqshuvd6opstwfzmh69i5astiwd");
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
