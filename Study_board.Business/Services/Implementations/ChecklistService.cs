using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Study_board.Models.Domain.Entities;
using Study_board.Models.ViewModels.Checklists;
using AutoMapper;
using Study_board.Business.Repositories.Interfaces;
using Study_board.Business.Services.Interfaces;

namespace Study_board.Services.Implementations
{
    public class ChecklistService : IChecklistService
    {
        private readonly IRepository<Checklist> _checklistRepository;
        private readonly IMapper _mapper;

        public ChecklistService(IRepository<Checklist> checklistRepository, IMapper mapper)
        {
            _checklistRepository = checklistRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ChecklistViewModel>> GetAllAsync()
        {
            var checklists = await _checklistRepository.GetAllAsync(c => c.Image);
            return _mapper.Map<IEnumerable<ChecklistViewModel>>(checklists);
        }

        public async Task<ChecklistViewModel?> GetByIdAsync(Guid id)
        {
            var checklist = await _checklistRepository.GetByIdAsync(id, c => c.Image);
            return _mapper.Map<ChecklistViewModel?>(checklist);
        }

        public async Task<ChecklistViewModel> CreateAsync(ChecklistCreateOrEditViewModel model)
        {
            var checklist = _mapper.Map<Checklist>(model);
            await _checklistRepository.AddAsync(checklist);
            await _checklistRepository.SaveChangesAsync();

            return _mapper.Map<ChecklistViewModel>(checklist);
        }

        public async Task<ChecklistViewModel> UpdateAsync(Guid id, ChecklistCreateOrEditViewModel model)
        {
            var existingChecklist = await _checklistRepository.GetByIdAsync(id, c => c.Image);
            if (existingChecklist == null)
            {
                throw new KeyNotFoundException($"Checklist ID {id} not found.");
            }

            _mapper.Map(model, existingChecklist);
            _checklistRepository.Update(existingChecklist);
            await _checklistRepository.SaveChangesAsync();

            return _mapper.Map<ChecklistViewModel>(existingChecklist);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existingChecklist = await _checklistRepository.GetByIdAsync(id);
            if (existingChecklist == null)
            {
                return false;
            }

            _checklistRepository.Delete(existingChecklist);
            await _checklistRepository.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ChecklistViewModel>> GetByUserIdAsync(Guid userId)
        {
            var checklists = await _checklistRepository.FilterAsync(
                c => c.UserId == userId,
                new Expression<Func<Checklist, object>>[] { c => c.Image });

            return _mapper.Map<IEnumerable<ChecklistViewModel>>(checklists);
        }

    }
}