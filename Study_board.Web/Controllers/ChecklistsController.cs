using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Study_board.Business.Services.Interfaces;
using Study_board.Models.ViewModels.Projects;
using Study_board.Models.Domain.Entities;
using Study_board.Data.Persistance;
using Study_board.Models.ViewModels.Checklists;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Study_board.Models.Domain.Enums.ProjectType;
using Study_board.Models.ViewModels.Users;

namespace Study_board.Web.Controllers
{
    public class ChecklistsController : Controller
    {
        private readonly IChecklistService _checklistService;
        private readonly IProjectService _projectService;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        public ChecklistsController(IChecklistService checklistService, IProjectService projectService, UserManager<User> userManager, ApplicationDbContext context)
        {
            _checklistService = checklistService;
            _projectService = projectService;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _checklistService.GetAllAsync());
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CompleteProject(int projectId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            var project = await _context.Projects.FindAsync(projectId);
            if (project == null || project.UserId != userId) return NotFound();

            int pointsToAdd = GetPointsByType(project.ProjectType, new StudyPointsSettings
            {
                Homework = 10,
                Presentation = 20,
                ScienceProject = 30,
                BigEssay = 25,
                SmallEssay = 15
            }); 
            user.TotalStudyPoints += pointsToAdd;
    
            project.IsCompleted = true;
            await _userManager.UpdateAsync(user);
            await _context.SaveChangesAsync();

            return Ok(new { TotalPoints = user.TotalStudyPoints, Added = pointsToAdd });
        }

        private int GetPointsByType(ProjectType type, StudyPointsSettings points)
        {
            return type switch
            {
                ProjectType.Homework => points.Homework,
                ProjectType.Presentation => points.Presentation,
                ProjectType.ScienceProject => points.ScienceProject,
                ProjectType.BigEssay => points.BigEssay,
                ProjectType.SmallEssay => points.SmallEssay,
                _ => 0
            };
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

        [Authorize]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ChecklistCreateOrEditViewModel checklist)
        {
            if (ModelState.IsValid)
            {
                checklist.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _checklistService.CreateAsync(checklist);
                return RedirectToAction(nameof(Index));
            }
            return View(checklist);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checklist = await _checklistService.GetForEditByIdAsync(id.Value);
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
                var existing = await _checklistService.GetForEditByIdAsync(id);
                if (existing != null)
                {
                    checklist.Image = existing.Image;
                }
                try
                {
                    checklist.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
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
