using Core.DTOs;
using Core.Models;
using Core.Repositories;
using Core.Services;
using Data;
using SharedLibrary.Dtos;
using System;
using System.Threading.Tasks;

namespace Service.Services
{
    public class UserService : ServiceGeneric<User, AppUserContext>, IUserService
    {
        private readonly IGenericRepository<User, AppUserContext> _userRep;

        public UserService(Core.UnitOfWork.IUnitOfWork<AppUserContext> unitOfWork, IGenericRepository<User, AppUserContext> genericRepository) : base(unitOfWork, genericRepository)
        {
            _userRep = genericRepository;
        }

        public async Task<Response<UserDto>> CreateUserAsync(CreateUserDto createUserDto)
        {
            var user = new User { Email = createUserDto.Email, UserName = createUserDto.UserName };

            try
            {
                await _userRep.AddAsync(user);
            }
            catch (Exception Ex)
            {
                return Response<UserDto>.Fail(new ErrorDto(Ex.Message, true), 400);
            }

            return Response<UserDto>.Success(new UserDto { Id = user.Id, UserName = user.UserName, Email = user.Email }, 200);
        }
    }
}