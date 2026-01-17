using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Study_board.Business.Services.Interfaces;
using Study_board.Models.Domain.Entities;
using Study_board.Models.ViewModels.Projects;
using AutoMapper;
using Study_board.Business.Repositories.Interfaces;

namespace Study_board.Services.Implementations
{
    /// <summary>
    /// Service for managing projects.
    /// </summary>
    public class ProjectService : IProjectService
    {
        private readonly IRepository<Project> _projectRepository;
        private readonly IRepository<Checklist> _checklistRepository;
        private readonly IMapper _mapper;

        public ProjectService(IRepository<Project> projectRepository, IRepository<Checklist> checklistRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _checklistRepository = checklistRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProjectViewModel>> GetAllAsync()
        {
            var projects = await _projectRepository.GetAllAsync(p => p.Checklist);
            return _mapper.Map<IEnumerable<ProjectViewModel>>(projects);
        }

        public async Task<ProjectViewModel?> GetByIdAsync(Guid id)
        {
            var project = await _projectRepository.GetByIdAsync(id, p => p.Checklist);
            return _mapper.Map<ProjectViewModel?>(project);
        }

        public async Task<IEnumerable<ProjectViewModel>> GetByChecklistIdAsync(Guid checklistId)
        {
            var projects = await _projectRepository.Query()
                .Where(p => p.ChecklistId == checklistId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ProjectViewModel>>(projects);
        }

        public async Task<ProjectViewModel> CreateAsync(ProjectCreateOrEditViewModel model)
        {
            var checklist = await _checklistRepository.GetByIdAsync(model.ChecklistId);
            if (checklist == null)
            {
                throw new KeyNotFoundException($"Checklist ID {model.ChecklistId} not found.");
            }

            if (model.ActiveTo.HasValue && model.ActiveTo.Value < model.ActiveFrom)
            {
                throw new ArgumentException("ActiveTo cannot be earlier than ActiveFrom.");
            }

            var project = _mapper.Map<Project>(model);
            project.Id = Guid.NewGuid();

            await _projectRepository.AddAsync(project);
            await _projectRepository.CommitAsync();

            return _mapper.Map<ProjectViewModel>(project);
        }

        public async Task<ProjectViewModel> UpdateAsync(Guid id, ProjectCreateOrEditViewModel model)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null)
            {
                throw new KeyNotFoundException($"ID {id} not found.");
            }

            var checklist = await _checklistRepository.GetByIdAsync(model.ChecklistId);
            if (checklist == null)
            {
                throw new KeyNotFoundException($"Checklist ID {model.ChecklistId} not found.");
            }

            if (model.ActiveTo.HasValue && model.ActiveTo.Value < model.ActiveFrom)
            {
                throw new ArgumentException("ActiveTo cannot be earlier than ActiveFrom.");
            }

            _mapper.Map(model, project);

            _projectRepository.Update(project);
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
    }
}
