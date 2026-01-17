using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Study_board.Models.ViewModels.Checklists
{
    /// <summary>
    /// Represents a view model for a single checklist image.
    /// Used for displaying image data in the UI.
    /// </summary>
    public class ChecklistImageViewModel
    {
        /// <summary>
        /// Gets or sets the unique identifier of the image.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the relative file path or URL to the image (e.g., "/images/checklists/pic.jpg").
        /// </summary>
        public string ImagePath { get; set; } = string.Empty;

    }
}
