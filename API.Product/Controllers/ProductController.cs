using Core.DTOs;
using Core.Services;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Dtos;
using SharedLibrary.Events;
using SharedLibrary.Extensions;
using System.Security.Claims;

namespace API.Product.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : CustomBaseController
    {
        private readonly IProductService _productService;
        private readonly IPublishEndpoint _publishEndpoint;

        public ProductController(IProductService productService, IPublishEndpoint publishEndpoint)
        {
            _productService = productService;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            return ActionResultInstance(await _productService.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> SaveProduct(ProductDto productDto)
        {
            var userId = Convert.ToInt32(User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var product = new Core.Models.Product()
            {
                UserId = userId,
                CategoryId = productDto.CategoryId,
                Deleted = false,
                Price = productDto.Price,
                Stock = productDto.Stock,
                ProductName = productDto.ProductName
            };

            var newProductResult = await _productService.AddAsync(product);

            if (!newProductResult.IsSuccessful)
                return ActionResultInstance(SharedLibrary.Dtos.Response<NoDataDto>.Fail(new ErrorDto("Ürün ekleme aşamasında bir hata oluştu", true), 500));

            var ProductCreatedEvent = new ProductCreatedEvent()
            {
                UserId = userId,
                ProductId = newProductResult.Data.ProductId,
                InvoiceNumber = $"FA-{userId}-{DateTime.Now.ToShortTimeString()}",
                ProductInvoiceMessage = new ProductInvoiceMessage() { UserId = userId, ProductId = newProductResult.Data.ProductId }
            };

            await _publishEndpoint.Publish(ProductCreatedEvent);

            return ActionResultInstance(SharedLibrary.Dtos.Response<Core.Models.Product>.Success(newProductResult.Data, 200));
        }
    }
}