using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Study_board.Models.Domain.Enums.ProjectType;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Study_board.Models.Domain.Entities
{
    public class User : IdentityUser
    {
        /// <summary>
        /// Gets or sets the collection of checklists associated with the user. Each checklist represents a set of projects that the user is working on.
        /// </summary>
        public virtual ICollection<Checklist>? Checklists { get; set; } = new List<Checklist>();
        /// <summary>
        /// Gets or sets the total study points accumulated by the user. Study points are awarded based on the completion of projects, and this property provides a way to track the user's overall progress and performance on the platform.
        /// </summary>
        public int TotalStudyPoints {get; set;} = 0;
    }
}