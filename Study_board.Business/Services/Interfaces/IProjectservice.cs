using Study_board.Models.Domain.Entities;
using Study_board.Models.ViewModels.Projects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
namespace Study_board.Business.Services.Interfaces
{
    /// <summary>
    /// Defines methods for managing projects.
    /// </summary>
    public interface IProjectService
    {
        /// <summary>
        /// Gets a project by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the project.</param>
        /// <returns>A <see cref="ProjectViewModel"/> if found; otherwise, <see langword="null"/>.</returns>
        Task<ProjectViewModel?> GetByIdAsync(Guid id);

        /// <summary>
        /// Gets all projects.
        /// </summary>
        /// <returns>A list of <see cref="ProjectViewModel"/>.</returns>
        Task<IEnumerable<ProjectViewModel>> GetAllAsync();
        /// <summary>
        /// Retrieves all projects associated with a specific checklist.
        /// </summary>
        /// <param name="ChecklistId">The ID of the checklist.</param>
        /// <returns>A collection of project view models.</returns>
        Task<IEnumerable<ProjectViewModel>> GetProjectsByChecklistIdAsync(Guid ChecklistId);

        /// <summary>
        /// Creates a new project.
        /// </summary>
        /// <param name="model">The Project data to create.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<ProjectViewModel> CreateAsync(ProjectCreateOrEditViewModel model);
        /// <summary>
        /// Updates an existing project.
        /// </summary>
        /// <param name="model">The Project data to update.</param>
        /// <returns>The updated <see cref="ProjectViewModel"/>.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the Project with the provided <paramref name="id"/> does not exist.</exception>
        Task<ProjectViewModel> UpdateAsync(ProjectCreateOrEditViewModel model);
        /// <summary>
        /// Deletes a project by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the Project to delete.</param>
        /// <returns>The deleted <see cref="ProjectViewModel"/>.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the Project with the provided <paramref name="id"/> does not exist.</exception>
        Task<ProjectViewModel> DeleteAsync(Guid id);
    }
}