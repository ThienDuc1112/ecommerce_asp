
using AutoMapper;
using FinalProject.Models;
using FinalProject.ViewModels.Brand;
using FinalProject.ViewModels.Category;
using FinalProject.ViewModels.Post;
using FinalProject.ViewModels.Product;
using FinalProject.ViewModels.Whislist;

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
            CreateMap<Product, GetProduct>().ReverseMap();
            CreateMap<Product, GetAdminProduct>().ReverseMap();

            CreateMap<Post, GetPost>().ReverseMap();
            CreateMap<Post, CreatePost>().ReverseMap();
            CreateMap<Post, UpdatePost>().ReverseMap();

            CreateMap<Whislist, CreateWistlist>().ReverseMap();
        }
    }
}
