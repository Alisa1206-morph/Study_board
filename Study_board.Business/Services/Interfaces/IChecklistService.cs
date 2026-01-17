using Study_board.Models.ViewModels.Checklists.ChecklistViewModel;
using Study_board.Models.ViewModels.Checklists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Study_board.Business.Services.Interfaces
{
    /// <summary>
    /// Defines the contract for checklist-related operations.
    /// </summary>
    public interface IChecklistService
    {
        /// <summary>
        /// Retrieves all checklists.
        /// </summary>
        /// <returns>A collection of <see cref="ChecklistViewModel"/> objects.</returns>
        Task<IEnumerable<ChecklistViewModel>> GetAllChecklistsAsync();
        /// <summary>
        /// Retrieves a checklist by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the checklist.</param>
        /// <returns>A <see cref="ChecklistViewModel"/> object if found;if not found, <see langword="null"/>.</returns>
        Task<ChecklistViewModel?> GetChecklistByIdAsync(Guid id);
        /// <summary>
        /// Retrieves checklists by title.
        /// </summary>
        /// <param name="title">The title of the checklist.</param>
        /// <returns>A collection of <see cref="ChecklistViewModel"/> objects matching the title.</returns>
        Task<IEnumerable<ChecklistViewModel>> GetChecklistsByTitleAsync(string title);
        /// <summary>
        /// Creates a new checklist.
        /// </summary>
        /// <param name="model">The data transfer object containing the checklist details to create.</param>
        /// <returns>The created <see cref="ChecklistViewModel"/>.</returns>
        Task<ChecklistViewModel> CreateChecklistAsync(ChecklistCreateOrEditViewModel viewModel);
        /// <summary>
        /// Updates an existing checklist.
        /// </summary>
        /// <param name="id">The unique identifier of the checklist.</param>
        /// <param name="model">The updated checklist data.</param>
        /// <returns>The updated <see cref="ChecklistViewModel"/>.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the checklist with the provided <paramref name="id"/> is not found.</exception>
        Task<ChecklistViewModel> UpdateChecklistAsync(Guid id, ChecklistCreateOrEditViewModel viewModel);
        Task<bool> DeleteChecklistAsync(Guid id);
        /// <summary>
        /// Retrieves checklists by user ID.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A collection of checklists associated with the user.</returns>
        Task<IEnumerable<ChecklistViewModel>> GetChecklistsByUserIdAsync(Guid userId);
    }
}