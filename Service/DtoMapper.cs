using AutoMapper;
using Core.DTOs;
using Core.Models;

namespace Service
{
    internal class DtoMapper : Profile
    {
        public DtoMapper()
        {
            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<UserAppDto, User>().ReverseMap();
        }
    }
}