using Azure;
using Core.DTOs;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SharedLibrary.Dtos;
using System;
using System.Text;

namespace UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IValidator<LoginDto> _validator;
        public AccountController(IHttpClientFactory httpClientFactory, IValidator<LoginDto> validator)
        {
            _httpClientFactory = httpClientFactory;
            _validator = validator;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (!User.Identity.IsAuthenticated)
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
    }
}