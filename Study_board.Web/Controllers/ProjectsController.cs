using Study_board.Business.Services.Interfaces;
using Study_board.Business.Services.Implementations;
using Study_board.Models.ViewModels.Projects;
using Study_board.Models.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Study_board.Models.Domain.Enums.ProjectType;
using System.Diagnostics.Tracing;
using Study_board.Models.ViewModels.Users;
using Study_board.Business.Repositories.Interfaces;
using Study_board.Business.Repositories.Implementations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Study_board.Web.Controllers
{
    /// <summary>
    /// Controller for managing projects.
    /// </summary>
    public class ProjectsController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IChecklistService _checklistService;
        private readonly ProjectType _projectType;
        private readonly StudyPointsSettings _points;
        private readonly IRepository<Checklist> _checklistRepository;

        public ProjectsController(IProjectService projectService, IChecklistService checklistService, IRepository<Checklist> checklistRepository, IOptions<StudyPointsSettings> points)
        {
            _projectService = projectService;
            _checklistService = checklistService;
            _checklistRepository = checklistRepository;
            _points = points.Value;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            //if the user is admin, load all projects, otherwise load only the projects of the current user
            if (User.IsInRole("Admin"))
            {
                return View(await _projectService.GetAllAsync());
            } 
            else
            {
                return View(await _projectService.GetByUserIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            }
        }
        

        public async Task<IActionResult> Complete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _projectService.GetByIdAsync(id.Value);
            if (project == null)
            {
                return NotFound();
            }

            await _projectService.MarkProjectAsCompletedAsync(id.Value);
            await _projectService.AssignStudyPointsUponCompletionAsync(id.Value);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _projectService.GetByIdAsync(id.Value);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }
        public async Task<IActionResult> Create()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.Checklists = await GetUserChecklistsAsSelectListAsync(userId);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectCreateOrEditViewModel project)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var isOwner = await _checklistRepository.Query()
                .AnyAsync(c => c.Id == project.ChecklistId && c.UserId == userId);
                if (!isOwner)
                {
                    ModelState.AddModelError("ChecklistId", "You can only assign projects to your own checklists.");
                }
                else
                {
                    project.UserId = userId;

                    await _projectService.CreateAsync(project);
                    return RedirectToAction(nameof(Index));
                }
                var userChecklists = await _checklistRepository.Query()
                .Where(c => c.UserId == project.UserId)
                .ToListAsync();
                await _projectService.CreateAsync(project);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Checklists = (await _checklistService.GetAllAsync()).Select(item => new SelectListItem() { Value = item.Id.ToString(), Text = item.Title }).ToList();

            return View(project);
        }

        private async Task<List<SelectListItem>> GetUserChecklistsAsSelectListAsync(string userId)
        {
            var checklists = await _checklistRepository.Query()
            .Where(c => c.UserId == userId)
            .ToListAsync();

            return checklists.Select(item => new SelectListItem()
            {
                Value = item.Id.ToString(),
                Text = item.Title
            }).ToList();
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _projectService.GetForEditByIdAsync(id.Value);
            if (project == null)
            {
                return NotFound();
            }
            ViewBag.Checklists = (await _checklistService.GetAllAsync()).Select(item => new SelectListItem() { Value = item.Id.ToString(), Text = item.Title }).ToList();

            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProjectCreateOrEditViewModel project)
        {
            if (ModelState.IsValid)
            {
                var existing = await _projectService.GetForEditByIdAsync(id);
                try
                {
                    await _projectService.UpdateAsync(id, project);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(id))
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
            
            ViewBag.Checklists = (await _checklistService.GetAllAsync()).Select(item => new SelectListItem() { Value = item.Id.ToString(), Text = item.Title }).ToList();

            return View(project);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _projectService.GetByIdAsync(id.Value);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var project = await _projectService.GetByIdAsync(id);
            if (project != null)
            {
                await _projectService.DeleteAsync(id);
            }

            return RedirectToAction(nameof(Index));
        }
        private bool ProjectExists(Guid id)
        {
            return _projectService.GetByIdAsync(id) != null;
        }
    }
}