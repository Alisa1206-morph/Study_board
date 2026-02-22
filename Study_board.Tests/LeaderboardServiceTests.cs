using AutoMapper;
using FluentAssertions;
using Study_board.Business.Repositories.Interfaces;
using Study_board.Business.Services.Implementations;
using Study_board.Business.Services.Interfaces;
using Study_board.Models.Domain.Entities;
using Study_board.Models.ViewModels.Leaderboard;
using Study_board.Models.ViewModels.Users;
using Microsoft.AspNetCore.Http;
using MockQueryable;
using MockQueryable.NSubstitute;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;
using System.Diagnostics.Tracing;
using System.Data.Common;
using Microsoft.AspNetCore.Identity;

namespace Study_board.Business.Tests.Services
{
    public class LeaderboardServiceTests
    {
        private readonly IMapper _mapper;
        private readonly LeaderboardService _service;
        private readonly UserManager<User> _userManager;

        public LeaderboardServiceTests()
        {
            _userManager = Substitute.For<UserManager<User>>(
                Substitute.For<IUserStore<User>>(),
                null, null, null, null, null, null, null, null);

            _mapper = Substitute.For<IMapper>();

            _service = new LeaderboardService(_userManager, _mapper);
        }

        [Fact]
        public async Task GetUsersToLeaderboardAsync_ShouldReturnLeaderboardViewModel()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = "1", Checklists = new List<Checklist> { new Checklist { Projects = new List<Project> { new Project { StudyPoints = 10 } } } } },
                new User { Id = "2", Checklists = new List<Checklist> { new Checklist { Projects = new List<Project> { new Project { StudyPoints = 20 } } } } }
            };

            var mockQueryable = users.BuildMock();
            _userManager.Users.Returns(mockQueryable);

            var leaderboardViewModel = new LeaderboardViewModel
            {
                Users = users.Select(u => new UserViewModel { Id = u.Id }).ToList()
            };

            _mapper.Map<UserViewModel>(Arg.Any<User>()).Returns(Arg.Any<UserViewModel>());

            // Act
            var result = await _service.GetUsersToLeaderboardAsync();

            // Assert
            result.Should().NotBeNull();
            result.Users.Count.Should().Be(2);
        }

        [Fact]
        public async Task GetUsersToLeaderboardAsync_ShouldOrderUsersByStudyPointsDescending()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = "1", Checklists = new List<Checklist> { new Checklist { Projects = new List<Project> { new Project { StudyPoints = 10 } } } } },
                new User { Id = "2", Checklists = new List<Checklist> { new Checklist { Projects = new List<Project> { new Project { StudyPoints = 20 } } } } } 
            };
            var mockQueryable = users.BuildMock();
            _userManager.Users.Returns(mockQueryable);
 
            var leaderboardViewModel = new LeaderboardViewModel
            {
                Users = users.Select(u => new UserViewModel { Id = u.Id }).ToList()
            };

            _mapper.Map<UserViewModel>(Arg.Any<User>()).Returns(Arg.Any<UserViewModel>());

            // Act
            var result = await _service.GetUsersToLeaderboardAsync();

            // Assert
            result.Users.ElementAt(0).Id.Should().Be("2");
            result.Users.ElementAt(1).Id.Should().Be("1");
        }

        [Fact]
        public async Task GetUsersToLeaderboardAsync_ShouldHandleEmptyUserList()
        {
            // Arrange
            var users = new List<User>();
            var mockQueryable = users.BuildMock();
            _userManager.Users.Returns(mockQueryable);
            var leaderboardViewModel = new LeaderboardViewModel
            {
                Users = new List<UserViewModel>()
            };

            _mapper.Map<UserViewModel>(Arg.Any<User>()).Returns(Arg.Any<UserViewModel>());

            // Act
            var result = await _service.GetUsersToLeaderboardAsync();

            // Assert
            result.Should().NotBeNull();
            result.Users.Should().BeEmpty();
        }

        [Fact]
        public async Task GetUsersToLeaderboardAsync_ShouldHandleNullChecklists()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = "1", Checklists = null },
                new User { Id = "2", Checklists = new List<Checklist> { new Checklist { Projects = new List<Project> { new Project { StudyPoints = 20 } } } } } 
            };

            var mockQueryable = users.BuildMock();
            _userManager.Users.Returns(mockQueryable);
            var leaderboardViewModel = new LeaderboardViewModel
            {
                Users = new List<UserViewModel>()
            };

            _mapper.Map<UserViewModel>(Arg.Any<User>()).Returns(Arg.Any<UserViewModel>());

            // Act
            var result = await _service.GetUsersToLeaderboardAsync();

            // Assert
            result.Should().NotBeNull();
            result.Users.Count.Should().Be(2);
        }

        [Fact]
        public async Task GetUsersToLeaderboardAsync_ShouldHandleNullProjects()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = "1", Checklists = new List<Checklist> { new Checklist { Projects = null } } },
                new User { Id = "2", Checklists = new List<Checklist> { new Checklist { Projects = new List<Project> { new Project { StudyPoints = 20 } } } } }
            };
            var mockQueryable = users.BuildMock();
            _userManager.Users.Returns(mockQueryable);
            var leaderboardViewModel = new LeaderboardViewModel
            {
                Users = new List<UserViewModel>()
            };

            _mapper.Map<UserViewModel>(Arg.Any<User>()).Returns(Arg.Any<UserViewModel>());

            // Act
            var result = await _service.GetUsersToLeaderboardAsync();

            // Assert
            result.Should().NotBeNull();
            result.Users.Count.Should().Be(2);
        }

        [Fact]
        public async Task GetUsersToLeaderboardAsync_ShouldHandleUsersWithNoStudyPoints()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = "1", Checklists = new List<Checklist> { new Checklist { Projects = new List<Project> { new Project { StudyPoints = 0 } } } } },
                new User { Id = "2", Checklists = new List<Checklist> { new Checklist { Projects = new List<Project> { new Project { StudyPoints = 0 } } } } }
            };
            var mockQueryable = users.BuildMock();
            _userManager.Users.Returns(mockQueryable);
            var leaderboardViewModel = new LeaderboardViewModel
            {
                Users = new List<UserViewModel>()
            };

            _mapper.Map<UserViewModel>(Arg.Any<User>()).Returns(Arg.Any<UserViewModel>());

            // Act
            var result = await _service.GetUsersToLeaderboardAsync();

            // Assert
            result.Should().NotBeNull();
            result.Users.Count.Should().Be(2);
            result.Users.ElementAt(0).Id.Should().Be("1");
            result.Users.ElementAt(1).Id.Should().Be("2");
        }        
    }
}