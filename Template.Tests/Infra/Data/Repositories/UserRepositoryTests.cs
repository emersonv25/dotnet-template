using Microsoft.EntityFrameworkCore;
using Template.Data.Repositories;
using Template.Data;
using Template.Domain.Entities;
using FluentAssertions;

namespace Template.Tests.Infra.Data.Repositories
{
    public class UserRepositoryTests
    {
        private readonly AppDbContext _dbContext;
        private readonly UserRepository _userRepository;

        public UserRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())  // Cria um banco de dados em memória único por teste
                .Options;

            _dbContext = new AppDbContext(options);
            _userRepository = new UserRepository(_dbContext);

            //_dbContext.Database.EnsureDeleted();  // Garante que o banco de dados é limpo
            //_dbContext.Database.EnsureCreated();  // Garante que o banco de dados é recriado
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User("John Doe", "john@example.com", "firebase123")
            {
                Id = userId
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _userRepository.GetByIdAsync(userId);

            // Assert
            result.Should().NotBeNull();  // Ensure the result is not null
            result?.Name.Should().Be("John Doe");
            result?.Email.Should().Be("john@example.com");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act
            var result = await _userRepository.GetByIdAsync(userId);

            // Assert
            result.Should().BeNull();  // Ensure the result is null if no user is found
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnPagedUsers()
        {
            // Arrange
            var users = new[]
            {
            new User("John Doe", "john@example.com", "firebase123"),
            new User("Jane Smith", "jane@example.com", "firebase456")
        };

            await _dbContext.Users.AddRangeAsync(users);
            await _dbContext.SaveChangesAsync();

            var pageNumber = 1;
            var pageSize = 10;

            // Act
            var result = await _userRepository.GetAllAsync(pageNumber, pageSize);

            // Assert
            result.Should().NotBeNull();
            result.Count.Should().Be(2); // There are 2 users in the mock data
            result[0].Name.Should().Be("John Doe");
            result[1].Name.Should().Be("Jane Smith");
            result.CurrentPage.Should().Be(pageNumber);
            result.PageSize.Should().Be(pageSize);
        }

        [Fact]
        public async Task GetByFirebaseIdAsync_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var firebaseId = "firebase123";
            var user = new User("John Doe", "john@example.com", firebaseId);

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _userRepository.GetByFirebaseIdAsync(firebaseId);

            // Assert
            result.Should().NotBeNull();  // Ensure the result is not null
            result?.Name.Should().Be("John Doe");
        }

        [Fact]
        public async Task AddAsync_ShouldAddUser()
        {
            // Arrange
            var user = new User("John Doe", "john@example.com", "firebase123");

            // Act
            await _userRepository.AddAsync(user, Guid.NewGuid());

            // Assert
            var result = await _dbContext.Users.FindAsync(user.Id);
            result.Should().NotBeNull(); // Ensure the user was added
            result?.Name.Should().Be("John Doe");
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateUser()
        {
            // Arrange
            var user = new User("John Doe", "john@example.com", "firebase123");
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            user.Name = "Updated Name";

            // Act
            await _userRepository.UpdateAsync(user, Guid.NewGuid());

            // Assert
            var updatedUser = await _dbContext.Users.FindAsync(user.Id);
            updatedUser.Should().NotBeNull();
            updatedUser?.Name.Should().Be("Updated Name");
        }

    }
}

