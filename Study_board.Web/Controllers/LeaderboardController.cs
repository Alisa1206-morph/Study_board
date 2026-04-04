using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Study_board.Business.Services.Interfaces;
using Study_board.Models.ViewModels.Projects;
using Study_board.Models.Domain.Entities;
using Study_board.Data;
using Study_board.Models.ViewModels.Checklists;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Study_board.Web.Controllers
{
    public class LeaderboardController : Controller
    {
        private readonly ILeaderboardService _leaderboardService;

        public LeaderboardController(ILeaderboardService leaderboardService)
        {
            _leaderboardService = leaderboardService;
        }

        public async Task<IActionResult> Index()
       {
            var viewModel = await _leaderboardService.GetUsersToLeaderboardAsync();

            if (viewModel == null)
                {
                    return NotFound("Leaderboard data not found.");
            }
            return View(viewModel);
        }
    }
}