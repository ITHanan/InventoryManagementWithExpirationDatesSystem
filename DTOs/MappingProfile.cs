using AutoMapper;
using InventoryManagementWithExpirationDatesSystem.Models;
using InventoryManagementWithExpirationDatesSystem.DTOs;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Item, ItemDTO>().ReverseMap();
    }
}