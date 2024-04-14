using Core.DTOs;
using Core.Models;

namespace Core.Services
{
    public interface ITokenService
    {
        TokenDto CreateToken(User userApp);
    }
}