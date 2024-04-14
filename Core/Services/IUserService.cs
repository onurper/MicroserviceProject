using Core.DTOs;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Dtos;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IUserService : IServiceGeneric<User, DbContext>
    {
        Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto);
    }
}