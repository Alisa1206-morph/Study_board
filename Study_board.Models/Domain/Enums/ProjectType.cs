using Study_board.Models.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Study_board.Models.Domain.Enums.ProjectType
{
    /// <summary>
    /// Defines the types of projects a user can create.
    /// </summary>
    public enum ProjectType
    {
        /// <summary>
        /// Represents a homework project.
        /// </summary>
        Homework = 0,

        /// <summary>
        /// Represents a presentation project.
        /// </summary>
        Presentation = 1,

        /// <summary>
        /// Represents a science project.
        /// </summary>
        ScienceProject = 2,

        /// <summary>
        /// Represents a big essay project.
        /// </summary>
        BigEssay = 3,

        /// <summary>
        /// Represents a small essay project.
        /// </summary>
        SmallEssay = 4,
    }
}
