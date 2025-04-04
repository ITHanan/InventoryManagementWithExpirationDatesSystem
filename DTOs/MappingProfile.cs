using AutoMapper;
using InventoryManagementWithExpirationDatesSystem.DTOs;
using InventoryManagementWithExpirationDatesSystem.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {

        CreateMap<Stock, StockDTO>().ReverseMap();
        //.ForMember(dest => dest.Item, opt => opt.MapFrom(src => src.Item))
        //.ForMember(dest => dest.Supplier, opt => opt.MapFrom(src => src.Supplier));

        CreateMap<Item, ItemDTO>();
        CreateMap<Supplier, SupplierDTO>();

    }
}