using AutoMapper;
using Study_board.Models.Domain.Entities;
using Study_board.Models.ViewModels.Checklists.ChecklistViewModel;
using Study_board.Models.ViewModels.Checklists.ChecklistCreateOrEditViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Study_board.Business.Mappings
{
    /// <summary>
    /// Defines mappings for checklist view models.
    /// </summary>
    public class ChecklistMappingProfile : Profile
    {
        public ChecklistMappingProfile()
        {
            CreateMap<Checklist, ChecklistViewModel>();

            CreateMap<ChecklistCreateOrEditViewModel, Checklist>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Images, opt => opt.Ignore());

            CreateMap<ChecklistImage, ChecklistImageViewModel>();

            CreateMap<ChecklistViewModel, ChecklistCreateOrEditViewModel>()
                .ForMember(dest => dest.ExistingImages, opt => opt.MapFrom(src => src.Images))
                .ForMember(dest => dest.Images, opt => opt.Ignore());
        }
    }
}