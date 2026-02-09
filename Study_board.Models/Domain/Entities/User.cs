using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Study_board.Models.Domain.Enums.ProjectType;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Study_board.Models.Domain.Entities
{
    public class User
    {
        /// <summary>
        /// Gets or sets the user's nickname, which must be between 1 and 20 characters in length.
        /// </summary>
        public string Nickname { get; set; } = string.Empty;
        /// <summary> 
        /// Gets or sets the user's age, which must be a non-negative integer.
        /// </summary>
        public int Age { get; set; }
        /// <summary>
        /// Gets or sets the user's id.
        /// </summary>
        public Guid UserId { get; set; }
        public virtual IdentityUser? User { get; set; }
        /// <summary>
        /// Gets or sets the collection of checklists associated with the user. Each checklist represents a set of projects that the user is working on.
        /// </summary>
        public virtual ICollection<Checklist>? Checklists { get; set; } = new List<Checklist>();
    }
}