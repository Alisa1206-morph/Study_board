using AutoMapper;
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
    /// Service for managing checklist-related operations.
    /// </summary>
    public class ChecklistService : IChecklistService
    {
        private readonly IRepository<Checklist> _checklistRepository;
        private readonly IMapper _mapper;

        public ChecklistService(IRepository<Checklist> checklistRepository, IMapper mapper)
        {
            _checklistRepository = checklistRepository;
            _mapper = mapper;
        }

        public Task<IEnumerable<ChecklistViewModel>> AddImageToChecklistsAsync(Collection<ChecklistImageViewModel> images)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ChecklistViewModel>> AddProjectsToChecklistsAsync(Collection<ProjectViewModel> projects)
        {
            throw new NotImplementedException();
        }

        public async Task<ChecklistViewModel> CreateAsync(ChecklistCreateOrEditViewModel model)
        {
            var checklistEntity = _mapper.Map<Checklist>(model);
            await _checklistRepository.AddAsync(checklistEntity);
            await _checklistRepository.SaveChangesAsync();
            return _mapper.Map<ChecklistViewModel>(checklistEntity);
        }

        public async Task<ChecklistViewModel> DeleteAsync(Guid id)
        {
            var checklist = await _checklistRepository.GetByIdAsync(id);
            if (checklist == null)
            {
                throw new KeyNotFoundException($"Checklist with ID {id} not found.");
            }

            _checklistRepository.Remove(checklist);
            await _checklistRepository.SaveChangesAsync();

            return _mapper.Map<ChecklistViewModel>(checklist);
        }

        public async Task<IEnumerable<ChecklistViewModel>> GetAllAsync()
        {
            var checklists = await _checklistRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ChecklistViewModel>>(checklists);
        }

        public async Task<ChecklistViewModel?> GetByIdAsync(Guid id)
        {
            var checklist = await _checklistRepository.GetByIdAsync(id);
            return _mapper.Map<ChecklistViewModel>(checklist);
        }

        public async Task<ChecklistViewModel> UpdateAsync(Guid Id, ChecklistCreateOrEditViewModel model)
        {
            var checklist = await _checklistRepository.GetByIdAsync(Id);
            if (checklist == null)
            {
                throw new KeyNotFoundException($"Checklist with ID {Id} not found.");
            }

            _mapper.Map(model, checklist);
            await _checklistRepository.SaveChangesAsync();

            return _mapper.Map<ChecklistViewModel>(checklist);
        }
    }
}