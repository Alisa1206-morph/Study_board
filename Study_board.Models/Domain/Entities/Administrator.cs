using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Study_board.Models.Domain.Enums.ProjectType;

namespace Study_board.Models.Domain.Entities
{
    public class Administrator
    {
        /// <summary>
        /// Gets or sets the administrator's first name, which must be between 1 and 20 characters in length.
        /// </summary>
        public string FirstName { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the administrator's last name, which must be between 1 and 20 characters in length.
        /// </summary> 
        public string LastName { get; set; } = string.Empty;
        /// <summary> 
        /// Gets or sets the administrator's age, which must be a non-negative integer.
        /// </summary>
        public int Age { get; set; }
        /// <summary>
        /// Gets or sets the administrator's id.
        /// </summary>
        public Guid AdminId { get; set; }
    }
}