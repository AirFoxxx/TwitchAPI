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
using Microsoft.AspNetCore.Http;

namespace TwitchAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ITwitchRepository _repository;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory clientFactory, ITwitchRepository repository)
        {
            _logger = logger;
            _clientFactory = clientFactory;
            _repository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
