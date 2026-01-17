using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Study_board.Models.Domain.Entities
{
    /// <summary>
    /// Represents an image associated with a checklist.
    /// </summary>
    public class ChecklistImage
    {
        public Guid Id { get; set; }

        /// <summary>
        /// The file system path or URL to the image
        /// </summary>
        [Required]
        public string ImagePath { get; set; } = string.Empty;

        /// <summary>
        /// Indicates if this is the primary cover image
        /// </summary>
        public Guid ChecklistId { get; set; }

        /// <summary>
        /// Navigation property
        /// </summary>
        [ForeignKey(nameof(ChecklistId))]
        public virtual Checklist Checklist { get; set; } = null!;
    }
}
