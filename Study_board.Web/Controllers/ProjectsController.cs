using Study_board.Business.Services.Interfaces;
using Study_board.Models.ViewModels.Projects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Study_board.Web.Controllers
{
    /// <summary>
    /// Controller for managing projects.
    /// </summary>
    public class ProjectsController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IChecklistService _checklistService;

        public ProjectsController(IProjectService projectService, IChecklistService checklistService)
        {
            _projectService = projectService;
            _checklistService = checklistService;
        }

        // Actions for managing projects would go here
        public async Task<IActionResult> Index()
        {
            return View(await _projectService.GetAllAsync());
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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectCreateOrEditViewModel project)
        {
            if (!ModelState.IsValid)
            {
                await _projectService.CreateAsync(project);
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        public async Task<IActionResult> Edit(Guid? id)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProjectCreateOrEditViewModel project)
        {
            if (ModelState.IsValid)
            {
                var existing = await _projectService.GetByIdAsync(id);
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