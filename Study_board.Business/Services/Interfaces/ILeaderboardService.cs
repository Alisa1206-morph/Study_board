using Study_board.Models.ViewModels.Users;
using Study_board.Models.ViewModels.Leaderboard;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;

namespace Study_board.Business.Services.Interfaces
{
    /// <summary>
    /// Service interface for managing leaderboard.
    /// </summary>
    public interface ILeaderboardService
    {
        /// <summary>
        /// Retrieves the users and their study points for the leaderboard based on a specific identifier.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="LeaderboardViewModel"/> with the users and their study points for the specified leaderboard entry.</returns>
        Task<LeaderboardViewModel> GetUsersToLeaderboardAsync();
    }
}