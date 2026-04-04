using AutoMapper;
using AutoMapper.QueryableExtensions;
using Study_board.Business.Repositories.Interfaces;
using Study_board.Models.ViewModels.Users;
using Study_board.Models.ViewModels.Leaderboard;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Study_board.Models.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Study_board.Business.Services.Interfaces
{
    /// <summary>
    /// Service interface for managing leaderboard.
    /// </summary>
    public class LeaderboardService : ILeaderboardService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public LeaderboardService(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<LeaderboardViewModel> GetUsersToLeaderboardAsync()
        {
            var users = await _userManager.Users
                .Include(u => u.Checklists)
                    .ThenInclude(c => c.Projects)
                .OrderByDescending(u => u.Checklists.Sum(c => c.Projects.Sum(p => p.StudyPoints)))
                .ToListAsync();

            var leaderboardViewModel = new LeaderboardViewModel
            {
                Users = users.Select(u => _mapper.Map<UserViewModel>(u)).ToList()
            };

            return leaderboardViewModel;
        }
    }
}