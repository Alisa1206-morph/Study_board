using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Study_board.Models.Domain.Entities;
using Study_board.Models.ViewModels.Users;

namespace Study_board.Models.ViewModels.Leaderboard
{
    public class LeaderboardViewModel
    {
        /// <summary>
        /// Gets or sets the collection of all registered users. Each user represents a participant in the leaderboard, and their study points contribute to their ranking on the leaderboard.
        /// </summary>
        public virtual ICollection<UserViewModel>? Users { get; set; } = new List<UserViewModel>();
    }
}