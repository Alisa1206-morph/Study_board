using AutoMapper;
using Study_board.Models.Domain.Entities;
using Study_board.Models.ViewModels.Users;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserViewModel>()
            .ForMember(dest => dest.TotalStudyPoints, opt => opt.MapFrom(src => src.Checklists.Sum(c => c.Projects.Sum(p => p.StudyPoints))));
    }
}
