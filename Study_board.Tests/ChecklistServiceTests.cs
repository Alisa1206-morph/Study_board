using AutoMapper;
using FluentAssertions;
using Study_board.Business.Repositories.Interfaces;
using Study_board.Business.Services.Implementations;
using Study_board.Business.Services.Interfaces;
using Study_board.Models.Domain.Entities;
using Study_board.Models.ViewModels.Projects;
using Study_board.Models.ViewModels.Checklists;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MockQueryable;
using MockQueryable.NSubstitute;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;


namespace Study_board.Business.Tests.Services
{
    public class ChecklistServiceTests
    {
        private readonly IRepository<Checklist> _checklistRepository;
        private readonly IMapper _mapper;
        private readonly ChecklistService _service;

        public ChecklistServiceTests()
        {
            _checklistRepository = Substitute.For<IRepository<Checklist>>();
            _mapper = Substitute.For<IMapper>();

            _service = new ChecklistService(
                _checklistRepository,
                Substitute.For<IRepository<ChecklistImage>>(),
                _mapper);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnMappedChecklist_WhenFound()
        {
            // Arrange
            var checklistId = Guid.NewGuid();

            var checklists = new List<Checklist>
            {
                new Checklist
                {
                    Id = checklistId,
                    Title = "Test checklist",
                    Projects = new List<Project>(),
                    Image = new ChecklistImage { Id = Guid.NewGuid()}
                },
                new Checklist { Id = Guid.NewGuid(), Title = "Other checklist" }
            };

            var mockQueryable = checklists.BuildMock();

            _checklistRepository.Query().Returns(mockQueryable);

            var expectedViewModel = new ChecklistViewModel
            {
                Id = checklistId,
                Title = "Test checklist",
                Projects = new List<ProjectViewModel>(),
                Image = new ChecklistImageViewModel { Id = Guid.NewGuid() }
            };

            _mapper.Map<ChecklistViewModel>(Arg.Any<Checklist>()).Returns(expectedViewModel);

            // Act
            var result = await _service.GetByIdAsync(checklistId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(checklistId);
            result.Title.Should().Be("Test checklist");
            result.Projects.Should().BeEmpty();
            result.Image.Should().NotBeNull();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            // Arrange
            var checklists = new List<Checklist>();
            var mockQueryable = checklists.BuildMock();
            _checklistRepository.Query().Returns(mockQueryable);

            // Act
            var result = await _service.GetByIdAsync(Guid.NewGuid());
            
            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task AddImageToChecklistAsync_ShouldAddImage_WhenChecklistExists()
        {
            // Arrange
            var checklistId = Guid.NewGuid();
            var checklist = new Checklist
            {
                Id = checklistId,
                Title = "Test checklist",
                Projects = new List<Project>(),
                Image = null
            };
            _checklistRepository.GetByIdAsync(checklistId).Returns(checklist);
            var image = new ChecklistImageViewModel
            {
                ImagePath = "http://example.com/image.jpg"
            };

            // Act
            var result = await _service.AddImageToChecklistsAsync(checklistId, image);
            // Assert
            result.Should().NotBeNull();
            var updatedChecklist = await _checklistRepository.GetByIdAsync(checklistId);
            updatedChecklist.Image.Should().NotBeNull();
            updatedChecklist.Image.ImagePath.Should().Be("http://example.com/image.jpg");

            _checklistRepository.Received(1).GetByIdAsync(checklistId);
            _checklistRepository.Received(1).CommitAsync();
        }

        [Fact]
        public async Task AddImageToChecklistAsync_ShouldThrow_WhenChecklistNotFound()
        {
            // Arrange
            var checklistId = Guid.NewGuid();
            _checklistRepository.GetByIdAsync(checklistId).Returns((Checklist)null);
            var image = new ChecklistImageViewModel
            {
                ImagePath = "http://example.com/image.jpg"
            };

            // Act
            Func<Task> act = async () => await _service.AddImageToChecklistsAsync(checklistId, image);
            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"Checklist with ID {checklistId} not found.");
            _checklistRepository.Received(1).GetByIdAsync(checklistId);
            _checklistRepository.DidNotReceive().CommitAsync();
        }
        [Fact]
        public async Task AddProjectsToChecklistsAsync_ShouldAddProjects_WhenChecklistExists()
        {
            // Arrange
            var checklistId = Guid.NewGuid();
            var checklist = new Checklist
            {
                Id = checklistId,
                Title = "Test checklist",
                Projects = new List<Project>(),
                Image = null
            };
            _checklistRepository.GetByIdAsync(checklistId).Returns(checklist);
            var projects = new List<ProjectViewModel>
            {
                new ProjectViewModel { Name = "Project 1", StudyPoints = 10 },
                new ProjectViewModel { Name = "Project 2", StudyPoints = 20 }
            };

            // Act
            var result = await _service.AddProjectsToChecklistsAsync(checklistId, projects);
            // Assert
            result.Should().NotBeNull();
            var updatedChecklist = await _checklistRepository.GetByIdAsync(checklistId);
            updatedChecklist.Projects.Should().HaveCount(2);
            updatedChecklist.Projects[0].Name.Should().Be("Project 1");
            updatedChecklist.Projects[0].StudyPoints.Should().Be(10);
            updatedChecklist.Projects[1].Name.Should().Be("Project 2");
            updatedChecklist.Projects[1].StudyPoints.Should().Be(20);

            _checklistRepository.Received(1).GetByIdAsync(checklistId);
            _checklistRepository.Received(1).CommitAsync();
        }

        [Fact]
        public async Task AddProjectsToChecklistsAsync_ShouldThrow_WhenChecklistNotFound()
        {
            // Arrange
            var checklistId = Guid.NewGuid();
            _checklistRepository.GetByIdAsync(checklistId).Returns((Checklist)null);
            var projects = new List<ProjectViewModel>
            {
                new ProjectViewModel { Name = "Project 1", StudyPoints = 10 },
                new ProjectViewModel { Name = "Project 2", StudyPoints = 20 }
            };

            // Act
            Func<Task> act = async () => await _service.AddProjectsToChecklistsAsync(checklistId, projects);
            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"Checklist with ID {checklistId} not found.");
            _checklistRepository.Received(1).GetByIdAsync(checklistId);
            _checklistRepository.DidNotReceive().CommitAsync();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnMappedChecklists_WhenRepositoryReturnsData()
        {
            // Arrange
            var checklists = new List<Checklist> { new Checklist { Id = Guid.NewGuid(), Title = "Test checklist" } };
            var viewModels = new List<ChecklistViewModel> { new ChecklistViewModel { Id = checklists[0].Id, Title = "Test checklist" } };

            _checklistRepository.GetAllAsync().Returns(checklists);
            _mapper.Map<IEnumerable<ChecklistViewModel>>(checklists).Returns(viewModels);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().Id.Should().Be(checklists[0].Id);
            result.First().Title.Should().Be("Test checklist");
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenRepositoryReturnsNoData()
        {
            // Arrange
            var checklists = new List<Checklist>();
            var viewModels = new List<ChecklistViewModel>();
            _checklistRepository.GetAllAsync().Returns(checklists);
            _mapper.Map<IEnumerable<ChecklistViewModel>>(checklists).Returns(viewModels);

            // Act
            var result = await _service.GetAllAsync();
            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
    }
}