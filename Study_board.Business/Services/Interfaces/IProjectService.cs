using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Study_board.Models.ViewModels.Projects;
namespace Study_board.Business.Services.Interfaces
{
    /// <summary>
    /// Defines the contract for project service operations.
    /// </summary>
    public interface IProjectService
    {
        /// <summary>
        /// Gets a project by its identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the project.</param>
        /// <returns>A <see cref="ProjectViewModel"/> if found; otherwise, <see langword="null"/>.</returns>
        Task<ProjectViewModel> GetProjectByIdAsync(Guid id);

        /// <summary>
        /// Gets all projects.
        /// </summary>
        /// <returns>A collection of <see cref="ProjectViewModel"/> projects.</returns>
        Task<IEnumerable<ProjectViewModel>> GetAllProjectsAsync();
        /// <summary>
        /// Gets all projects associated with a specific checklist.
        /// </summary>
        /// <param name="checklistId">The unique identifier of the checklist.</param>
        /// <returns>A collection of <see cref="ProjectViewModel"/> projects.</returns>
        Task<IEnumerable<ProjectViewModel>> GetProjectsByChecklistIdAsync(Guid checklistId);

        /// <summary>
        /// Creates a new project.
        /// </summary>
        /// <param name="model">The project data to create.</param>
        /// <returns>The created <see cref="ProjectViewModel"/> project</returns>
        Task<ProjectViewModel> CreateProjectAsync(ProjectViewModel model);

        /// <summary>
        /// Updates an existing project.
        /// </summary>
        /// <param name="id">The unique identifier of the project to update.</param>
        /// <param name="model">The updated project data.</param>
        /// <returns>The updated <see cref="ProjectViewModel"/> project.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the project with the provided <paramref name="id"/> is not found.</exception>
        Task<ProjectViewModel> UpdateProjectAsync(Guid id, ProjectViewModel model);

        Task<bool> DeleteProjectAsync(Guid id);
    }
}