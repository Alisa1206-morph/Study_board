using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Study_board.Models.Domain.Entities;
using Study_board.Models.ViewModels.Checklists;

namespace Study_board.Models.ViewModels.Users
{
    public class UserViewModel
    {
        /// <summary>
        /// Gets or sets collection of checklists associated with the user. Each checklist represents a set of projects that the user is working on.
        /// </summary>
        public virtual ICollection<ChecklistViewModel>? Checklists { get; set; } = new List<ChecklistViewModel>();
        /// <summary>
        /// Gets or sets the total study points accumulated by the user. Study points are awarded based on the completion of projects, and this property provides a way to track the user's overall progress and performance on the platform.
        /// </summary>
        public int TotalStudyPoints
        {
            get
            {
                return Checklists?.Where(c => c.UserId == Id).Sum(c => c.Projects.Sum(p => p.StudyPoints)) ?? 0;
            }
        }

        public string UserName { get; set; } = string.Empty;

        public string Id { get; set; } = string.Empty;
    }
}