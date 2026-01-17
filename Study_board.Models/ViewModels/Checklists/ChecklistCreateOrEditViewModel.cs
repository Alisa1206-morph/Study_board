using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Study_board.Models.Domain.Entities;

namespace Study_board.Models.ViewModels.Checklists
{
    /// <summary>
    /// Represents the data required when creating checklist
    /// </summary>
    public class ChecklistCreateOrEditViewModel
    {
        /// <summary>
        /// Gets or sets the checklist's title
        /// </summary>
        [MinLength(1)]
        [MaxLength(50)]
        [Required]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the image for the checklist.
        /// </summary>
        public virtual ChecklistImage? Image { get; set; }

        /// <summary>
        /// Gets or sets the list of projects associated with this checklist.
        /// </summary>  
        public List<string> Projects { get; set; } = new List<string>();

    }
}