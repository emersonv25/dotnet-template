using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Template.Data;
using Template.Domain.Entities;
using Template.Infra.Data.Helpers;

namespace Template.Tests.Infra.Data.Helpers
{
    public class PaginationHelperTests
    {
        private readonly AppDbContext _dbContext;

        public PaginationHelperTests()
        {
            // Configuração do banco de dados em memória
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Nome do banco de dados em memória
                .Options;
            _dbContext = new AppDbContext(options);
        }
        [Fact]
        public async Task CreateAsync_ShouldReturnCorrectPagedList_WhenPageNumberAndPageSizeAreValid()
        {
            // Arrange
            var data = new[]
            {
            new User("John Doe", "john@example.com", "firebase123"),
            new User("Jane Smith", "jane@example.com", "firebase456"),
            new User("Mike Johnson", "mike@example.com", "firebase789"),
            new User("Lucy Brown", "lucy@example.com", "firebase101")
        };

            // Adicionando os dados ao DbContext em memória
            await _dbContext.Users.AddRangeAsync(data);
            await _dbContext.SaveChangesAsync();

            var pageNumber = 1;
            var pageSize = 2;

            // Act
            var result = await PaginationHelper.CreateAsync(_dbContext.Users.AsQueryable(), pageNumber, pageSize);

            // Assert
            result.Should().NotBeNull();
            result.CurrentPage.Should().Be(1);
            result.PageSize.Should().Be(2);
            result.TotalCount.Should().Be(4);
            result.TotalPages.Should().Be(2); // 4 items / 2 per page = 2 pages
            result.Count.Should().Be(2); // Only 2 items should be returned for the first page
            result[0].Name.Should().Be("John Doe");
            result[1].Name.Should().Be("Jane Smith");
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnEmptyList_WhenPageNumberExceedsTotalPages()
        {
            // Arrange
            var data = new[]
            {
                new User("John Doe", "john@example.com", "firebase123"),
                new User("Jane Smith", "jane@example.com", "firebase456")
            }.AsQueryable();

            // Adicionando os dados ao DbContext em memória
            await _dbContext.Users.AddRangeAsync(data);
            await _dbContext.SaveChangesAsync();

            var pageNumber = 3; // Exceeds the total pages (2 items, page size = 2)
            var pageSize = 2;

            // Act
            var result = await PaginationHelper.CreateAsync(_dbContext.Users.AsQueryable(), pageNumber, pageSize);

            // Assert
            result.Should().NotBeNull();
            result.CurrentPage.Should().Be(3);
            result.TotalPages.Should().Be(1); // Only 1 page is available (2 items / 2 per page)
            result.Count.Should().Be(0); // No items on the 3rd page
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCorrectPagedList_WhenPageSizeIsSmallerThanTotalItems()
        {
            // Arrange
            var data = new[]
            {
                new User("John Doe", "john@example.com", "firebase123"),
                new User("Jane Smith", "jane@example.com", "firebase456"),
                new User("Mike Johnson", "mike@example.com", "firebase789"),
                new User("Lucy Brown", "lucy@example.com", "firebase101"),
                new User("Anna White", "anna@example.com", "firebase102")
            }.AsQueryable();

            // Adicionando os dados ao DbContext em memória
            await _dbContext.Users.AddRangeAsync(data);
            await _dbContext.SaveChangesAsync();

            var pageNumber = 2;
            var pageSize = 2;

            // Act
            var result = await PaginationHelper.CreateAsync(_dbContext.Users.AsQueryable(), pageNumber, pageSize);

            // Assert
            result.Should().NotBeNull();
            result.CurrentPage.Should().Be(2);
            result.TotalCount.Should().Be(5); // 5 items total
            result.TotalPages.Should().Be(3); // 5 items / 2 per page = 3 pages
            result.Count.Should().Be(2); // 2 items should be returned for the second page
            result[0].Name.Should().Be("Mike Johnson");
            result[1].Name.Should().Be("Lucy Brown");
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnEmptyList_WhenDataIsEmpty()
        {
            // Arrange
            var data = Enumerable.Empty<User>().AsQueryable();

            // Adicionando os dados ao DbContext em memória
            await _dbContext.Users.AddRangeAsync(data);
            await _dbContext.SaveChangesAsync();

            var pageNumber = 1;
            var pageSize = 2;

            // Act
            var result = await PaginationHelper.CreateAsync(_dbContext.Users.AsQueryable(), pageNumber, pageSize);

            // Assert
            result.Should().NotBeNull();
            result.Count.Should().Be(0);
            result.TotalCount.Should().Be(0);
            result.TotalPages.Should().Be(0);
        }
    }
}
