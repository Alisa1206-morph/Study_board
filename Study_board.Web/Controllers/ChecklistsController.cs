using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Study_board.Business.Services.Interfaces;
using Study_board.Models.ViewModels.Projects;
using Study_board.Models.Domain.Entities;
using Study_board.Data.Persistence;
using Study_board.Models.ViewModels.Checklists;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Study_board.Web.Controllers
{
    public class ChecklistsController : Controller
    {
        private readonly IChecklistService _checklistService;
        private readonly IProjectService _projectService;

        public ChecklistsController(IChecklistService checklistService, IProjectService projectService)
        {
            _checklistService = checklistService;
            _projectService = projectService;
        }

        // Actions for managing checklists would go here
        public async Task<IActionResult> Index()
        {
            return View(await _checklistService.GetAllAsync());
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checklist = await _checklistService.GetByIdAsync(id.Value);
            if (checklist == null)
            {
                return NotFound();
            }

            return View(checklist);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ChecklistCreateOrEditViewModel checklist)
        {
            if (!ModelState.IsValid)
            {
                await _checklistService.CreateAsync(checklist);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checklist = await _checklistService.GetByIdAsync(id.Value);
            if (checklist == null)
            {
                return NotFound();
            }

            return View(checklist);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ChecklistCreateOrEditViewModel checklist)
        {
            if (ModelState.IsValid)
            {
                var existing = await _checklistService.GetByIdAsync(id);
                if (existing != null)
                {
                    checklist.ExistingImages = existing.Images?.ToList() ?? new List<ChecklistImageViewModel>();
                }
                try
                {
                    await _checklistService.UpdateAsync(id, checklist);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChecklistExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(checklist);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checklist = await _checklistService.GetByIdAsync(id.Value);
            if (checklist == null)
            {
                return NotFound();
            }

            return View(checklist);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var checklist = await _checklistService.GetByIdAsync(id);
            if (checklist != null)
            {
                await _checklistService.DeleteAsync(id);
            }

            return RedirectToAction(nameof(Index));
        }
        private bool ChecklistExists(Guid id)
        {
            return _checklistService.GetByIdAsync(id) != null;
        }
    }
}
