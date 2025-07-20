using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TodoApi.Models;
using TodoApi.Services;
using TodoApi.Controllers;
using Xunit;

namespace TodoApi.Tests.Unit
{
    public class TodoControllerTest
    {
        [Fact]
        public async Task GetTodos_ReturnsOkResultWithTodos()
        {
            var todos = new List<Todo> {
                new Todo { Id = 1, Title = "Test 1", IsCompleted = false },
                new Todo { Id = 2, Title = "Test 2", IsCompleted = true }
            };
            var mockService = new Mock<ITodoService>();
            mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(todos);
            var controller = new TodoController(mockService.Object);

            var result = await controller.GetTodos();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnTodos = Assert.IsAssignableFrom<IEnumerable<Todo>>(okResult.Value);
            Assert.Equal(2, ((List<Todo>)returnTodos).Count);
        }

        [Fact]
        public async Task GetTodo_ReturnsNotFound_WhenTodoDoesNotExist()
        {
            var mockService = new Mock<ITodoService>();
            mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((Todo)null);
            var controller = new TodoController(mockService.Object);

            var result = await controller.GetTodo(1);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetTodo_ReturnsOkResult_WhenTodoExists()
        {
            var todo = new Todo { Id = 1, Title = "Test", IsCompleted = false };
            var mockService = new Mock<ITodoService>();
            mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(todo);
            var controller = new TodoController(mockService.Object);

            var result = await controller.GetTodo(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnTodo = Assert.IsType<Todo>(okResult.Value);
            Assert.Equal(1, returnTodo.Id);
        }

        [Fact]
        public async Task CreateTodo_ReturnsCreatedAtAction()
        {
            var todo = new Todo { Id = 1, Title = "Test", IsCompleted = false };
            var mockService = new Mock<ITodoService>();
            mockService.Setup(s => s.CreateAsync(todo)).ReturnsAsync(todo);
            var controller = new TodoController(mockService.Object);

            var result = await controller.CreateTodo(todo);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnTodo = Assert.IsType<Todo>(createdResult.Value);
            Assert.Equal("Test", returnTodo.Title);
        }

        [Fact]
        public async Task UpdateTodo_ReturnsBadRequest_WhenIdMismatch()
        {
            var todo = new Todo { Id = 2, Title = "Test", IsCompleted = false };
            var mockService = new Mock<ITodoService>();
            var controller = new TodoController(mockService.Object);

            var result = await controller.UpdateTodo(1, todo);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task UpdateTodo_ReturnsNotFound_WhenTodoDoesNotExist()
        {
            var todo = new Todo { Id = 1, Title = "Test", IsCompleted = false };
            var mockService = new Mock<ITodoService>();
            mockService.Setup(s => s.UpdateAsync(1, todo)).ReturnsAsync((Todo)null);
            var controller = new TodoController(mockService.Object);

            var result = await controller.UpdateTodo(1, todo);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UpdateTodo_ReturnsNoContent_WhenTodoUpdated()
        {
            var todo = new Todo { Id = 1, Title = "Test", IsCompleted = true };
            var mockService = new Mock<ITodoService>();
            mockService.Setup(s => s.UpdateAsync(1, todo)).ReturnsAsync(todo);
            var controller = new TodoController(mockService.Object);

            var result = await controller.UpdateTodo(1, todo);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteTodo_ReturnsNotFound_WhenTodoDoesNotExist()
        {
            var mockService = new Mock<ITodoService>();
            mockService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(false);
            var controller = new TodoController(mockService.Object);

            var result = await controller.DeleteTodo(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteTodo_ReturnsNoContent_WhenTodoDeleted()
        {
            var mockService = new Mock<ITodoService>();
            mockService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);
            var controller = new TodoController(mockService.Object);

            var result = await controller.DeleteTodo(1);

            Assert.IsType<NoContentResult>(result);
        }
    }
}
