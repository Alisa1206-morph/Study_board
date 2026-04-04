using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Study_board.Models.Domain.Entities;
using Study_board.Models.ViewModels.Projects;

namespace Study_board.Models.ViewModels.Checklists
{
    /// <summary>
    /// Represents the data required when creating checklist
    /// </summary>
    public class ChecklistCreateViewModel
    {
        /// <summary>
        /// Gets or sets the checklist's title
        /// </summary>
        [MinLength(1)]
        [MaxLength(50)]
        [Required]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets projects associated with this checklist.
        /// </summary>
        public List<ProjectViewModel> Projects { get; set; } = new List<ProjectViewModel>();
    }
}