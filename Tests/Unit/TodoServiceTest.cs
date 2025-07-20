using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using TodoApi.Models;
using TodoApi.Services;
using Xunit;

namespace TodoApi.Tests.Unit
{
    public class TodoServiceTest
    {
        [Fact]
        public async Task GetAllAsync_ReturnsTodos()
        {
            // Arrange
            var todos = new List<Todo> {
                new Todo { Id = 1, Title = "Test 1", IsCompleted = false },
                new Todo { Id = 2, Title = "Test 2", IsCompleted = true }
            };
            var mockSet = new Mock<ITodoService>();
            mockSet.Setup(s => s.GetAllAsync()).ReturnsAsync(todos);

            // Act
            var result = await mockSet.Object.GetAllAsync();

            // Assert
            Assert.Equal(2, ((List<Todo>)result).Count);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsTodo()
        {
            // Arrange
            var todo = new Todo { Id = 1, Title = "Test", IsCompleted = false };
            var mockSet = new Mock<ITodoService>();
            mockSet.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(todo);

            // Act
            var result = await mockSet.Object.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
        {
            var mockSet = new Mock<ITodoService>();
            mockSet.Setup(s => s.GetByIdAsync(999)).ReturnsAsync((Todo)null);

            var result = await mockSet.Object.GetByIdAsync(999);

            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_ReturnsCreatedTodo()
        {
            // Arrange
            var todo = new Todo { Id = 1, Title = "Test", IsCompleted = false };
            var mockSet = new Mock<ITodoService>();
            mockSet.Setup(s => s.CreateAsync(todo)).ReturnsAsync(todo);

            // Act
            var result = await mockSet.Object.CreateAsync(todo);

            // Assert
            Assert.Equal("Test", result.Title);
        }

        [Fact]
        public async Task CreateAsync_ReturnsTodo_WithEmptyTitle()
        {
            var todo = new Todo { Id = 2, Title = "", IsCompleted = false };
            var mockSet = new Mock<ITodoService>();
            mockSet.Setup(s => s.CreateAsync(todo)).ReturnsAsync(todo);

            var result = await mockSet.Object.CreateAsync(todo);

            Assert.Equal("", result.Title);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsUpdatedTodo()
        {
            // Arrange
            var todo = new Todo { Id = 1, Title = "Updated", IsCompleted = true };
            var mockSet = new Mock<ITodoService>();
            mockSet.Setup(s => s.UpdateAsync(1, todo)).ReturnsAsync(todo);

            // Act
            var result = await mockSet.Object.UpdateAsync(1, todo);

            // Assert
            Assert.Equal("Updated", result.Title);
            Assert.True(result.IsCompleted);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsNull_WhenNotFound()
        {
            var todo = new Todo { Id = 999, Title = "NotFound", IsCompleted = false };
            var mockSet = new Mock<ITodoService>();
            mockSet.Setup(s => s.UpdateAsync(999, todo)).ReturnsAsync((Todo)null);

            var result = await mockSet.Object.UpdateAsync(999, todo);

            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsTrue()
        {
            // Arrange
            var mockSet = new Mock<ITodoService>();
            mockSet.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

            // Act
            var result = await mockSet.Object.DeleteAsync(1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse_WhenNotFound()
        {
            var mockSet = new Mock<ITodoService>();
            mockSet.Setup(s => s.DeleteAsync(999)).ReturnsAsync(false);

            var result = await mockSet.Object.DeleteAsync(999);

            Assert.False(result);
        }
    }
}
