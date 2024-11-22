using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Application.DTOs;
using Template.Application.Interfaces;
using Template.Domain.Entities;

namespace Template.Tests.Application.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserService> _mockUserService;

        public UserServiceTests()
        {
            _mockUserService = new Mock<IUserService>();
        }

        #region GetAllAsync Tests
        [Fact]
        public async Task GetAllAsync_ShouldReturnPaginatedResponse_WhenCalled()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 10;
            var expectedResponse = new PaginatedResponseDTO<UserDTO>(
                new List<UserDTO> { new UserDTO("John Doe", "john@example.com") },
                pageNumber,
                pageSize,
                100,
                10);

            _mockUserService.Setup(service => service.GetAllAsync(pageNumber, pageSize))
                            .ReturnsAsync(expectedResponse);

            // Act
            var result = await _mockUserService.Object.GetAllAsync(pageNumber, pageSize);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(1);
            result.Items[0].Name.Should().Be("John Doe");
            result.TotalCount.Should().Be(100);
            result.TotalPages.Should().Be(10);
        }
        #endregion

        #region GetUserByFirebaseIdAsync Tests
        [Fact]
        public async Task GetUserByFirebaseIdAsync_ShouldReturnUser_WhenFirebaseIdExists()
        {
            // Arrange
            var firebaseId = "firebase123";
            var expectedUser = new User { Id = Guid.NewGuid(), FirebaseId = firebaseId, Name = "John Doe", Email = "john@example.com" };

            _mockUserService.Setup(service => service.GetUserByFirebaseIdAsync(firebaseId))
                            .ReturnsAsync(expectedUser);

            // Act
            var result = await _mockUserService.Object.GetUserByFirebaseIdAsync(firebaseId);

            // Assert
            result.Should().NotBeNull();
            result?.FirebaseId.Should().Be(firebaseId);
            result?.Name.Should().Be("John Doe");
            result?.Email.Should().Be("john@example.com");
        }

        [Fact]
        public async Task GetUserByFirebaseIdAsync_ShouldReturnNull_WhenFirebaseIdNotFound()
        {
            // Arrange
            var firebaseId = "firebase123";
            _mockUserService.Setup(service => service.GetUserByFirebaseIdAsync(firebaseId))
                            .ReturnsAsync((User?)null);

            // Act
            var result = await _mockUserService.Object.GetUserByFirebaseIdAsync(firebaseId);

            // Assert
            result.Should().BeNull();
        }
        #endregion

        #region GetUserByIdAsync Tests
        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var expectedUser = new User { Id = userId, Name = "John Doe", Email = "john@example.com" };

            _mockUserService.Setup(service => service.GetUserByIdAsync(userId))
                            .ReturnsAsync(expectedUser);

            // Act
            var result = await _mockUserService.Object.GetUserByIdAsync(userId);

            // Assert
            result.Should().NotBeNull();
            result?.Id.Should().Be(userId);
            result?.Name.Should().Be("John Doe");
            result?.Email.Should().Be("john@example.com");
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnNull_WhenUserNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _mockUserService.Setup(service => service.GetUserByIdAsync(userId))
                            .ReturnsAsync((User?)null);

            // Act
            var result = await _mockUserService.Object.GetUserByIdAsync(userId);

            // Assert
            result.Should().BeNull();
        }
        #endregion

        #region CreateOrUpdateUser Tests
        [Fact]
        public async Task CreateOrUpdateUser_ShouldCreateNewUser_WhenUserDoesNotExist()
        {
            // Arrange
            var firebaseId = "firebase123";
            var userDTO = new UserDTO("John Doe", "john@example.com");
            var expectedUser = new User("John Doe", "john@example.com", firebaseId);

            _mockUserService.Setup(service => service.CreateOrUpdateUser(userDTO, firebaseId))
                            .ReturnsAsync(expectedUser);

            // Act
            var result = await _mockUserService.Object.CreateOrUpdateUser(userDTO, firebaseId);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("John Doe");
            result.Email.Should().Be("john@example.com");
            result.FirebaseId.Should().Be(firebaseId);
        }

        [Fact]
        public async Task CreateOrUpdateUser_ShouldUpdateExistingUser_WhenUserExists()
        {
            // Arrange
            var firebaseId = "firebase123";
            var userDTO = new UserDTO("John Updated", "johnupdated@example.com");
            var existingUser = new User("John Doe", "john@example.com", firebaseId);

            existingUser.UpdateUserInfo(userDTO.Name, userDTO.Email);

            _mockUserService.Setup(service => service.CreateOrUpdateUser(userDTO, firebaseId))
                            .ReturnsAsync(existingUser);

            // Act
            var result = await _mockUserService.Object.CreateOrUpdateUser(userDTO, firebaseId);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("John Updated");
            result.Email.Should().Be("johnupdated@example.com");
            result.FirebaseId.Should().Be(firebaseId);
        }
        #endregion
    }
}
