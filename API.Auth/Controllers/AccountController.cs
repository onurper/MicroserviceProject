using Core.DTOs;
using Core.Services;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Extensions;
using System.Threading.Tasks;

namespace API.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : CustomBaseController
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
        {
            return ActionResultInstance(await _userService.CreateUserAsync(createUserDto));
        }
    }
}