using Core.DTOs;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Dtos;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IProductService : IServiceGeneric<Product, DbContext>
    {
        public Task<Response<NoDataDto>> CategoryCheck(int categoryId);
    }
}