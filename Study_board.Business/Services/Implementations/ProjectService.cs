using AutoMapper;
using Study_board.Business.Repositories.Interfaces;
using Study_board.Business.Services.Interfaces;
using Study_board.Models.Domain.Entities;
using Study_board.Models.Domain.Enums.ProjectType;
using Study_board.Models.ViewModels.Checklists;
using Study_board.Models.ViewModels.Projects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Study_board.Models.ViewModels.Users;

namespace Study_board.Business.Services.Implementations
{
    /// <summary>
    /// Service for managing project-related operations.
    /// </summary>
    public class ProjectService : IProjectService
    {
        private readonly IRepository<Project> _projectRepository;
        private readonly IRepository<Checklist> _checklistRepository;
        private readonly IMapper _mapper;
        private readonly StudyPointsSettings _studyPointsSettings;

        public ProjectService(IRepository<Project> projectRepository, IRepository<Checklist> checklistRepository, IMapper mapper, IOptions<StudyPointsSettings> studyPointsSettings)
        {
            _projectRepository = projectRepository;
            _checklistRepository = checklistRepository;
            _mapper = mapper;
            _studyPointsSettings = studyPointsSettings.Value;
        }

        public async Task<IEnumerable<ProjectViewModel>> AddProjectsToChecklistAsync(Guid checklistId, Collection<ProjectCreateOrEditViewModel> projects)
        {
            var checklist = await _checklistRepository.GetByIdAsync(checklistId);
            if (checklist == null)
            {
                throw new KeyNotFoundException($"Checklist with ID {checklistId} not found.");
            }

            var projectEntities = projects.Select(p => _mapper.Map<Project>(p)).ToList();
            foreach (var project in projectEntities)
            {
                project.ChecklistId = checklistId;
                await _projectRepository.AddAsync(project);
            }
            await _projectRepository.CommitAsync();

            return _mapper.Map<IEnumerable<ProjectViewModel>>(projectEntities);
        }

        public async Task<IEnumerable<ProjectViewModel>> AssignStudyPointsUponCompletionAsync(Guid checklistId, bool isCompleted, ProjectType projectType)
        {
            var projects = await _projectRepository.Query()
                .Where(p => p.ChecklistId == checklistId)
                .ToListAsync();

            foreach (var project in projects)
            {
                if (project.IsCompleted == true)
                {
                    project.StudyPoints = projectType switch
                    {
                        ProjectType.Homework => _studyPointsSettings.Homework,
                        ProjectType.Presentation => _studyPointsSettings.Presentation,
                        ProjectType.ScienceProject => _studyPointsSettings.ScienceProject,
                        ProjectType.BigEssay => _studyPointsSettings.BigEssay,
                        ProjectType.SmallEssay => _studyPointsSettings.SmallEssay,
                        _ => 0
                    };
                }
                else
                {
                    project.StudyPoints = 0;
                }
            };
            await _projectRepository.CommitAsync();
            return _mapper.Map<IEnumerable<ProjectViewModel>>(projects);
        }


        
        public async Task<ProjectViewModel> CreateAsync(ProjectCreateOrEditViewModel model)
        {
            var checklist = await _checklistRepository.GetByIdAsync(model.ChecklistId);
            if (checklist == null)            
            { 
                throw new KeyNotFoundException($"Checklist with ID {model.ChecklistId} not found.");
            }
      
            var project = _mapper.Map<Project>(model);
            project.Checklist = checklist;
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
            var projects = await _projectRepository.Query()
                .Include(p => p.Checklist)
                .ToListAsync();
            return _mapper.Map<IEnumerable<ProjectViewModel>>(projects);
        }

        public async Task<ProjectViewModel?> GetByIdAsync(Guid id)
        {
            var project = await _projectRepository.GetByIdAsync(id, p => p.Checklist);
            return _mapper.Map<ProjectViewModel>(project);
        }

        public async Task<ProjectCreateOrEditViewModel?> GetForEditByIdAsync(Guid id)
        {
            var project = await _projectRepository.GetByIdAsync(id, p => p.Checklist);
            return _mapper.Map<ProjectCreateOrEditViewModel>(project);
        }

        public async Task<IEnumerable<ProjectViewModel>> GetProjectsByChecklistIdAsync(Guid ChecklistId)
        {
            var projects = await _projectRepository.Query()
                .Where(p => p.ChecklistId == ChecklistId)
                .ToListAsync();
            return _mapper.Map<IEnumerable<ProjectViewModel>>(projects);
        }

        public async Task<ProjectViewModel> MarkProjectAsCompletedAsync(Guid id)
        {
            var project = _projectRepository.Query()
                .FirstOrDefault(p => p.Id == id);
            if (project == null)
            {
                throw new KeyNotFoundException($"Project with ID {id} not found.");
            }
            project.IsCompleted = true;
            _projectRepository.Update(project);
            await _projectRepository.CommitAsync();
            return _mapper.Map<ProjectViewModel>(project);
        }

        public async Task<ProjectViewModel> UpdateAsync(Guid ChecklistId, ProjectCreateOrEditViewModel model)
        {
            var project = await _projectRepository.GetByIdAsync(ChecklistId);
            if (project == null)
            {
                throw new KeyNotFoundException($"Project with ID {model.ChecklistId} not found.");
            }

            _mapper.Map(model, project);
            await _projectRepository.CommitAsync();

            return _mapper.Map<ProjectViewModel>(project);
        }
    }
}