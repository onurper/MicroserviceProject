using Core.DTOs;
using Core.Models;
using Core.Repositories;
using Core.Services;
using Core.UnitOfWork;
using Data;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Dtos;
using System;
using System.Threading.Tasks;

namespace Service.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork<AppUserContext> _unitOfWork;
        private readonly IGenericRepository<UserRefreshToken, AppUserContext> _userRefreshTokenService;
        private readonly IGenericRepository<User, AppUserContext> _user;

        public AuthenticationService(ITokenService tokenService, IUnitOfWork<AppUserContext> unitOfWork, IGenericRepository<UserRefreshToken, AppUserContext> userRefreshTokenService, IGenericRepository<User, AppUserContext> user)
        {
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
            _userRefreshTokenService = userRefreshTokenService;
            _user = user;
        }

        public async Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto)
        {
            if (loginDto == null) throw new ArgumentNullException(nameof(loginDto));

            var user = await _user.Where(x => x.Email == loginDto.Email).FirstOrDefaultAsync();

            if (user == null) return Response<TokenDto>.Fail("E-posta veya Şifre yanlış", 400, true);

            if (!await _user.Where(x => x.Password == loginDto.Password).AnyAsync())
            {
                return Response<TokenDto>.Fail("E-posta veya Şifre yanlış", 400, true);
            }
            var token = _tokenService.CreateToken(user);

            var userRefreshToken = await _userRefreshTokenService.Where(x => x.UserId == user.Id.ToString()).SingleOrDefaultAsync();

            if (userRefreshToken == null)
            {
                await _userRefreshTokenService.AddAsync(new UserRefreshToken { UserId = user.Id.ToString(), Code = token.RefreshToken, Expiration = token.RefreshTokenExpiration });
            }
            else
            {
                userRefreshToken.Code = token.RefreshToken;
                userRefreshToken.Expiration = token.RefreshTokenExpiration;
            }

            await _unitOfWork.CommmitAsync();

            return Response<TokenDto>.Success(token, 200);
        }

        public async Task<Response<TokenDto>> CreateTokenByRefreshToken(string refreshToken)
        {
            var existRefreshToken = await _userRefreshTokenService.Where(x => x.Code == refreshToken).SingleOrDefaultAsync();

            if (existRefreshToken == null)
            {
                return Response<TokenDto>.Fail("Refresh token not found", 404, true);
            }

            var user = await _user.Where(x => x.Id.ToString() == existRefreshToken.UserId).SingleOrDefaultAsync();

            if (user == null)
            {
                return Response<TokenDto>.Fail("User Id not found", 404, true);
            }

            var tokenDto = _tokenService.CreateToken(user);

            existRefreshToken.Code = tokenDto.RefreshToken;
            existRefreshToken.Expiration = tokenDto.RefreshTokenExpiration;

            await _unitOfWork.CommmitAsync();

            return Response<TokenDto>.Success(tokenDto, 200);
        }

        public async Task<Response<NoDataDto>> RevokeRefreshToken(string refreshToken)
        {
            var existRefreshToken = await _userRefreshTokenService.Where(x => x.Code == refreshToken).SingleOrDefaultAsync();
            if (existRefreshToken == null)
            {
                return Response<NoDataDto>.Fail("Refresh token not found", 404, true);
            }

            _userRefreshTokenService.Remove(existRefreshToken);

            await _unitOfWork.CommmitAsync();

            return Response<NoDataDto>.Success(200);
        }
    }
}