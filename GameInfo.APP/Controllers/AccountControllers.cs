using GameInfo.APP.Interfaces;
using GameInfo.APP.Models;
using GameInfo.APP.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameInfo.APP.Controllers
{
    [Authorize]
    public class AccountControllers : BaseController
    {
        private readonly IApiClient _apiClient;
        private readonly IOptions<AppSettings> _settings;
        public AccountControllers(IApiClient apiClient, IOptions<AppSettings> settings)
        {
            _apiClient = apiClient;
            _settings = settings;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("Login")]
        public IActionResult Login()
        {
            var user = GetCurrentUser();
            if (user.UserId == null)
            {
                return View();
            }
            return RedirectToAction("Dashboard", "Reward");
        }

        [Route("Login")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _apiClient.ExecuteAsync(new Models.ApiRequest
                {
                    BaseUri = _settings.Value.ApiUrl,
                    Method = RestSharp.Method.POST,
                    Headers = new Dictionary<string, string>
                    {
                        { "X-ApiKey",_settings.Value.GiftProvider.APIKEY },
                        { "Accept-Language","en-GB" }
                    }
                },
                });
            if (response.Success)
            {
                var claims = new List<Claim>
                    {
                        new Claim("ProductId", response.Value.ProductId.ToString()),
                        new Claim("UserId", response.Value.Id.ToString())
                    };

                await _authService.SignInAsync(HttpContext, claims);

                return Redirect("/Reward/Dashboard");
            }
            else
            {
                ModelState.AddModelError("Fail", response.Message);
            }
        }
            return View(model);
    }
}
}
