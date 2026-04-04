using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Study_board.Models.ViewModels.Projects;

namespace Study_board.Models.Domain.Entities
{
    /// <summary>
    /// Represents a checklist entity containing a collection of projects.
    /// </summary>
    public class Checklist
    {
        /// <summary>
        /// Gets or sets the unique identifier for the checklist.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the title of the checklist.
        /// </summary>
        ///
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Checklist title must be between 1 and 100 characters.")]
        [Required(ErrorMessage = "Checklist title is required.")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user ID who owns this checklist.
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the projects associated with this checklist.
        /// </summary>
        public List<Project> Projects { get; set; } = new List<Project>();
        /// <summary>
        /// Gets or sets the total study points for all projects in the checklist by summing the StudyPoints of each project.
        /// </summary>
        /// 
        [NotMapped]
        public int TotalStudyPoints
        {
            get
            {
                return Projects.Sum(p => p.StudyPoints);
            }
        }

        /// <summary>
        /// Gets or sets the image for the checklist.
        /// </summary>
        public virtual ChecklistImage? Image { get; set; }
    }
}