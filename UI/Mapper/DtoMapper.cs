using AutoMapper;
using Core.DTOs;
using Core.Models;

namespace UI.Mapper
{
    internal class DtoMapper : Profile
    {
        public DtoMapper()
        {
            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<ProductDto, Models.CreateProductViewModel>().ReverseMap();
            CreateMap<UserDto, User>().ReverseMap();
        }
    }
}