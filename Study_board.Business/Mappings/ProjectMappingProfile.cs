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
                .ForMember(m => m.Name, opt => opt.MapFrom(src => src.Name));
            CreateMap<ProjectCreateOrEditViewModel, Project>();
        }
    }
}