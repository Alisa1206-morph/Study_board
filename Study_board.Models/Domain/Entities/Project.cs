using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Study_board.Models.Domain.Enums.ProjectType;

namespace Study_board.Models.Domain.Entities
{
    /// <summary>
    /// Represents a project entity in the study board system.
    /// </summary>
    public class Project
    {
        /// <summary>
        /// Gets or sets the unique identifier for the project.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the project. Maximum length: 100 characters.
        /// </summary>
        /// 
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the project. Maximum length: 500 characters.
        /// </summary>
        /// 
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
        /// Gets or sets the type of the project (Homework, Presentation, ScienceProject, BigEssay, SmallEssay).
        /// </summary>
        public ProjectType Type { get; set; } = ProjectType.Homework;
    }
}