using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Study_board.Models.Domain.Entities;
using Study_board.Models.Domain.Enums.ProjectType;
using Study_board.Models.ViewModels.Checklists;

namespace Study_board.Models.ViewModels.Projects
{
    public class ProjectViewModel
    {
        /// <summary>
        /// Gets or sets the unique identifier
        /// </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the checklist to which the project belongs.
        /// </summary>
        public string ChecklistTitle { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the project's name within 50 characters.
        /// </summary>
        [Required]
        [MaxLength(50)]
        [MinLength(1)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the project's description within 500 characters.
        /// </summary>
        [Required]
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the due date of the project.
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the project is completed.
        /// </summary>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// Gets or sets the user ID of the owner of the project. This is used to associate the project with a specific user and to ensure that users can only access and modify their own projects.
        /// </summary>
        public string UserId { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the type of the project (Homework, Presentation, ScienceProject, BigEssay, SmallEssay).
        /// </summary>
        [Required(ErrorMessage = "Project type is required.")]
        public ProjectType ProjectType { get; set; }
        /// <summary>
        /// Gets or sets the study points awarded for completing the project.
        /// </summary>
        public int StudyPoints { get; set; }
    }
}