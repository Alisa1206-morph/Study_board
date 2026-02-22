using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Study_board.Models.Domain.Entities
{
    /// <summary>
    /// Represents the settings for study points awarded for different types of projects.
    /// </summary>
    public class StudyPointsSettings
    {
        /// <summary>
        /// Gets or sets the number of points awarded for completing a homework project.
        /// </summary>
        public int Homework { get; set; }
        
        /// <summary>
        /// Gets or sets the number of points awarded for completing a presentation project.
        /// </summary>
        public int Presentation { get; set; }

        /// <summary>
        /// Gets or sets the number of points awarded for completing a science project.
        /// </summary>
        public int ScienceProject { get; set; }

        /// <summary>
        /// Gets or sets the number of points awarded for completing a big essay project.
        /// </summary>
        public int BigEssay { get; set; }

        /// <summary> 
        /// Gets or sets the number of points awarded for completing a small essay project.
        /// </summary>
        public int SmallEssay { get; set; }
    }
}