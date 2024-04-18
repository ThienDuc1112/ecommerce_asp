
using AutoMapper;
using FinalProject.Models;
using FinalProject.ViewModels.Brand;
using FinalProject.ViewModels.Category;
using FinalProject.ViewModels.Product;

namespace FinalProject.Mappings
{
    public class MappingProfile : Profile
    {
       public MappingProfile()
        {
            CreateMap<Brand, AddBrand>().ReverseMap();
            CreateMap<Brand, UpdateBrand>().ReverseMap();

            CreateMap<Category, AddCategory>().ReverseMap();
            CreateMap<Category, UpdateCategory>().ReverseMap();

            CreateMap<Product, CreateProduct>().ReverseMap();
            CreateMap<Product, UpdateProduct>().ReverseMap();
        }
    }
}
