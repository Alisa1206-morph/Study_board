using AutoMapper;
using Study_board.Models.Domain.Entities;
using Study_board.Models.ViewModels.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Study_board.Business.Mappings
{
    /// <summary>
    /// Defines mappings for project view models.
    /// </summary>
    public class ProjectMappingProfile : Profile
    {
        public ProjectMappingProfile()
        {
            CreateMap<Project, ProjectViewModel>()
                .ForMember(m => m.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(m => m.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(m => m.DueDate, opt => opt.MapFrom(src => src.DueDate))
                .ForMember(m => m.IsCompleted, opt => opt.MapFrom(src => src.IsCompleted))
                .ForMember(m => m.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(m => m.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(m => m.ChecklistTitle, opt => opt.MapFrom(src => src.Checklist != null ? src.Checklist.Title : "No Checklist"))
                .ForMember(m => m.StudyPoints, opt => opt.MapFrom(src => src.StudyPoints))
                .ForMember(m => m.ProjectType, opt => opt.MapFrom(src => src.ProjectType));
            CreateMap<ProjectCreateOrEditViewModel, Project>();
            CreateMap<Project, ProjectCreateOrEditViewModel>()
                .ForMember(dest => dest.ChecklistId, opt => opt.MapFrom(src => src.ChecklistId));
        }
    }
}