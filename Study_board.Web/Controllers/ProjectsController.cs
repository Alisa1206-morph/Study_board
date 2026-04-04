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
using Study_board.Models.Domain.Enums.ProjectType;
using System.Diagnostics.Tracing;

namespace Study_board.Web.Controllers
{
    /// <summary>
    /// Controller for managing projects.
    /// </summary>
    public class ProjectsController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IChecklistService _checklistService;
        private readonly StudyPointsSettings _points;

        public ProjectsController(IProjectService projectService, IChecklistService checklistService, IOptions<StudyPointsSettings> options)
        {
            _projectService = projectService;
            _checklistService = checklistService;
            _points = options.Value;
        }

        public async Task<IActionResult> GetPoints()
        {
            var selectedType = Console.ReadLine() switch
            {
                "Homework" => ProjectType.Homework,
                "Presentation" => ProjectType.Presentation,
                "ScienceProject" => ProjectType.ScienceProject,
                "BigEssay" => ProjectType.BigEssay,
                "SmallEssay" => ProjectType.SmallEssay,
                _ => throw new ArgumentException("Invalid project type")
            };

            var studyPoints = await _projectService.AssignStudyPointsUponCompletionAsync(Guid.NewGuid(), true, selectedType);
            return View(studyPoints);
        }


        public async Task<IActionResult> Index()
        {
            return View(await _projectService.GetAllAsync());
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
            ViewBag.Checklists = (await _checklistService.GetAllAsync()).Select(item => new SelectListItem() { Value = item.Id.ToString(), Text = item.Title }).ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectCreateOrEditViewModel project)
        {
            if (ModelState.IsValid)
            {
                await _projectService.CreateAsync(project);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Checklists = (await _checklistService.GetAllAsync()).Select(item => new SelectListItem() { Value = item.Id.ToString(), Text = item.Title }).ToList();

            return View(project);
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