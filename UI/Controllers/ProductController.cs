using Core.DTOs;
using Core.Models;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SharedLibrary.Dtos;
using System.Text;
using UI.Mapper;
using UI.Models;

namespace UI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IValidator<ProductDto> _validator;

        public ProductController(IHttpClientFactory httpClientFactory, IValidator<ProductDto> validator)
        {
            _httpClientFactory = httpClientFactory;
            _validator = validator;
        }

        [HttpGet]
        public async Task<IActionResult> Products()
        {
            if (Request.Cookies["User"] == null)
                return RedirectToAction("Index", "Home");

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Request.Cookies["User"]);

            var response = await client.GetAsync("https://localhost:44343/api/Product");
            if (!response.IsSuccessStatusCode)
                return RedirectToAction("Index", "Home");

            var resultJsonData = await response.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<ResponseJsonDeserializeObject<List<ProductDto>>>(resultJsonData);

            return View(values.Data);
        }

        [HttpGet]
        public async Task<IActionResult> CreateProduct()
        {
            if (Request.Cookies["User"] == null)
                return RedirectToAction("Index", "Home");

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Request.Cookies["User"]);

            var categoriesResponse = await client.GetAsync("https://localhost:44343/api/Category");
            if (!categoriesResponse.IsSuccessStatusCode)
                return RedirectToAction("Index", "Home");

            var resultCategoriesJsonData = await categoriesResponse.Content.ReadAsStringAsync();
            var categoryValues = JsonConvert.DeserializeObject<ResponseJsonDeserializeObject<List<Category>>>(resultCategoriesJsonData);

            CreateProductViewModel createProductViewModel = new() { Categories = categoryValues.Data };

            return View(createProductViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductViewModel createProductViewModel)
        {
            if (Request.Cookies["User"] == null)
                return RedirectToAction("Index", "Home");

            ValidationResult result = await _validator.ValidateAsync(ObjectMapper.Mapper.Map<ProductDto>(createProductViewModel));
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return View(ObjectMapper.Mapper.Map<ProductDto>(createProductViewModel));
            }

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Request.Cookies["User"]);

            var jsonData = JsonConvert.SerializeObject(ObjectMapper.Mapper.Map<ProductDto>(createProductViewModel));
            StringContent content = new(jsonData, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:44343/api/Product", content);
            if (response.IsSuccessStatusCode)
            {
                var resultJsonData = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<ResponseSuccess>(resultJsonData);

                return RedirectToAction("Products", "Product");
            }
            else
            {
                var resultJsonData = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<ResponseFail>(resultJsonData);

                foreach (string item in values.error.Errors)
                    ModelState.AddModelError("", item);

                return View(createProductViewModel);
            }
        }
    }
}