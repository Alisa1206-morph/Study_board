using AutoMapper;
using FluentAssertions;
using Study_board.Business.Repositories.Interfaces;
using Study_board.Business.Services.Implementations;
using Study_board.Business.Services.Interfaces;
using Study_board.Models.Domain.Entities;
using Study_board.Models.ViewModels.Projects;
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

namespace Study_board.Business.Tests.Services
{
    public class ProjectServiceTests
    {
        private readonly IRepository<Project> _projectRepository;
        private readonly IMapper _mapper;
        private readonly ProjectService _service;

        public ProjectServiceTests()
        {
            _projectRepository = Substitute.For<IRepository<Project>>();
            _mapper = Substitute.For<IMapper>();

            _service = new ProjectService(
                _projectRepository,
                _mapper);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnMappedproject_WhenFound()
        {
            // Arrange
            var projectId = Guid.NewGuid();

            var projects = new List<Project>
            {
                new Project
                {
                    Id = projectId,
                    Name = "Test project"
                },
                new Project { Id = Guid.NewGuid(), Name = "Other project" }
            };

            var mockQueryable = projects.BuildMock();

            _projectRepository.Query().Returns(mockQueryable);

            _mapper.Map<ProjectViewModel?>(Arg.Is<Project>(r => r.Id == projectId))
                .Returns(new ProjectViewModel { Id = projectId, Name = "Test project" });

            // Act
            var result = await _service.GetByIdAsync(projectId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(projectId);

            _projectRepository.Received(1).Query();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            // Arrange
            var projects = new List<Project>();
            var mockQueryable = projects.BuildMock();

            _projectRepository.Query().Returns(mockQueryable);

            // Act
            var result = await _service.GetByIdAsync(Guid.NewGuid());

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnMappedprojects_WhenRepositoryReturnsData()
        {
            // Arrange
            var projects = new List<Project> { new Project { Id = Guid.NewGuid() } };
            var viewModels = new List<ProjectViewModel> { new ProjectViewModel() };

            _projectRepository.GetAllAsync(Arg.Any<Expression<Func<Project, object>>[]>())
                .Returns(projects);

            _mapper.Map<IEnumerable<ProjectViewModel>>(projects).Returns(viewModels);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
        }


        [Fact]
        public async Task UpdateAsync_ShouldThrowKeyNotFoundException_WhenIdDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            _projectRepository.GetByIdAsync(id, Arg.Any<Expression<Func<Project, object>>[]>())
                .Returns((Project)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _service.UpdateAsync(id, new ProjectCreateOrEditViewModel()));
        }
    }
}