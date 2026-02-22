using AutoMapper;
using Study_board.Models.Domain.Entities;
using Study_board.Models.ViewModels.Users;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserViewModel>();
    }
}
