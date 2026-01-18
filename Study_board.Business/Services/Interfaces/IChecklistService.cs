using Study_board.Models.ViewModels.Checklists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Study_board.Business.Services.Interfaces
{
    /// <summary>
    /// Service interface for managing checklists.
    /// </summary>
    public interface IChecklistService
{
    /// <summary>
    /// Gets a checklist by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the checklist.</param>
    /// <returns>A <see cref="ChecklistViewModel"/> if found; otherwise, <see langword="null"/>.</returns>
    Task<ChecklistViewModel?> GetByIdAsync(Guid id);

    /// <summary>
    /// Gets all checklists.
    /// </summary>
    /// <returns>A list of <see cref="ChecklistViewModel"/>.</returns>
    Task<IEnumerable<ChecklistViewModel>> GetAllAsync();

    /// <summary>
    /// Creates a new checklist.
    /// </summary>
    /// <param name="model">The checklist data to create.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<ChecklistViewModel> CreateAsync(ChecklistCreateOrEditViewModel model);
    /// <summary>
    /// Updates an existing checklist.
    /// </summary>
    /// <param name="model">The checklist data to update.</param>
    /// <returns>The updated <see cref="ChecklistViewModel"/>.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the checklist with the provided <paramref name="id"/> does not exist.</exception>
    Task<ChecklistViewModel> UpdateAsync(ChecklistCreateOrEditViewModel model);
    /// <summary>
    /// Deletes a checklist by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the checklist to delete.</param>
    /// <returns>The deleted <see cref="ChecklistViewModel"/>.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the checklist with the provided <paramref name="id"/> does not exist.</exception>
    Task<ChecklistViewModel> DeleteAsync(Guid id);
}
}