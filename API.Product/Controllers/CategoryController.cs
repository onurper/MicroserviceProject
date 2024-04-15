using Core.Models;
using Core.Services;
using Data;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Extensions;

namespace API.Product.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : CustomBaseController
    {
        private readonly IServiceGeneric<Category, AppProductContext> serviceGeneric;

        public CategoryController(IServiceGeneric<Category, AppProductContext> serviceGeneric)
        {
            this.serviceGeneric = serviceGeneric;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            return ActionResultInstance(await serviceGeneric.GetAllCategoryAsync());
        }
    }
}