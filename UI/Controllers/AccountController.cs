using Core.DTOs;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SharedLibrary.Dtos;
using System.Text;

namespace UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IValidator<LoginDto> _validator;
        private readonly IValidator<CreateUserDto> _registervalidator;

        public AccountController(IHttpClientFactory httpClientFactory, IValidator<LoginDto> validator, IValidator<CreateUserDto> registervalidator)
        {
            _httpClientFactory = httpClientFactory;
            _validator = validator;
            _registervalidator = registervalidator;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (Request.Cookies["User"] == null)
                return View();

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            ValidationResult result = await _validator.ValidateAsync(loginDto);
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return View(loginDto);
            }

            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(loginDto);
            StringContent content = new(jsonData, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:44372/api/Auth/CreateToken", content);
            if (response.IsSuccessStatusCode)
            {
                var resultJsonData = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<ResponseSuccess>(resultJsonData);

                var options = new CookieOptions
                {
                    Expires = DateTime.Now.AddHours(24)
                };

                HttpContext.Response.Cookies.Append("User", values.data.accessToken, options);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                var resultJsonData = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<ResponseFail>(resultJsonData);

                foreach (string item in values.error.Errors)
                    ModelState.AddModelError("", item);

                return View(loginDto);
            }
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            if (Request.Cookies["User"] == null)
                return View();

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
        {
            ValidationResult result = await _registervalidator.ValidateAsync(createUserDto);

            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return View(createUserDto);
            }

            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(createUserDto);
            StringContent content = new(jsonData, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:44372/api/Account", content);
            if (response.IsSuccessStatusCode)
            {
                var resultJsonData = await response.Content.ReadAsStringAsync();

                var values = JsonConvert.DeserializeObject<ResponseJsonDeserializeObject<CreateUserDto>>(resultJsonData);

                client = _httpClientFactory.CreateClient();
                jsonData = JsonConvert.SerializeObject(new LoginDto { Email = createUserDto.Email, Password = createUserDto.Password });
                content = new(jsonData, Encoding.UTF8, "application/json");

                response = await client.PostAsync("https://localhost:44372/api/Auth/CreateToken", content);

                resultJsonData = await response.Content.ReadAsStringAsync();
                var Loginvalues = JsonConvert.DeserializeObject<ResponseJsonDeserializeObject<TokenDto>>(resultJsonData);

                var options = new CookieOptions
                {
                    Expires = DateTime.Now.AddHours(24)
                };

                HttpContext.Response.Cookies.Append("User", Loginvalues.Data.AccessToken, options);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                var resultJsonData = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<ResponseFail>(resultJsonData);

                foreach (string item in values.error.Errors)
                    ModelState.AddModelError("", item);

                return View(createUserDto);
            }
        }
    }
}