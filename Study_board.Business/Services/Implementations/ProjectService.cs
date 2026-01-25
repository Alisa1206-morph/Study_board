using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Study_board.Business.Repositories.Interfaces;
using Study_board.Business.Services.Interfaces;
using Study_board.Models.Domain.Entities;
using Study_board.Models.ViewModels.Checklists;
using Study_board.Models.ViewModels.Projects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Study_board.Business.Services.Implementations
{
    /// <summary>
    /// Service for managing project-related operations.
    /// </summary>
    public class ProjectService : IProjectService
    {
        private readonly IRepository<Project> _projectRepository;
        private readonly IMapper _mapper;

        public ProjectService(IRepository<Project> projectRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        public async Task<ProjectViewModel> CreateAsync(ProjectCreateOrEditViewModel model)
        {
            var project = await _projectRepository.GetByIdAsync(model.Id);
            if (project == null)
            {
                throw new KeyNotFoundException($"Project with ID {model.Id} not found.");
            }

            await _projectRepository.AddAsync(project);
            await _projectRepository.CommitAsync();

            return _mapper.Map<ProjectViewModel>(project);
        }

        public async Task<ProjectViewModel> DeleteAsync(Guid id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null)
            {
                throw new KeyNotFoundException($"ID {id} not found.");
            }

            _projectRepository.Remove(project);
            await _projectRepository.CommitAsync();

            return _mapper.Map<ProjectViewModel>(project);
        }

        public async Task<IEnumerable<ProjectViewModel>> GetAllAsync()
        {
            var projects = await _projectRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProjectViewModel>>(projects);
        }

        public async Task<ProjectViewModel?> GetByIdAsync(Guid id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            return _mapper.Map<ProjectViewModel>(project);
        }

        public async Task<IEnumerable<ProjectViewModel>> GetProjectsByChecklistIdAsync(Guid ChecklistId)
        {
            var projects = await _projectRepository.Query()
                .Where(p => p.ChecklistId == ChecklistId)
                .ToListAsync();
            return _mapper.Map<IEnumerable<ProjectViewModel>>(projects);
        }


        public async Task<ProjectViewModel> UpdateAsync(ProjectCreateOrEditViewModel model)
        {
            var project = await _projectRepository.GetByIdAsync(model.Id);
            if (project == null)
            {
                throw new KeyNotFoundException($"Project with ID {model.Id} not found.");
            }

            _mapper.Map(model, project);
            await _projectRepository.CommitAsync();

            return _mapper.Map<ProjectViewModel>(project);
        }
    }
}