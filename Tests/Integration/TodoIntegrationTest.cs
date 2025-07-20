using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using TodoApi.Models;
using Xunit;

namespace TodoApi.Tests.Integration
{
    public class TodoIntegrationTests : IClassFixture<WebApplicationFactory<TodoApi.Program>>
    {
        private readonly HttpClient _client;

        public TodoIntegrationTests(WebApplicationFactory<TodoApi.Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetTodos_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/todos");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PostTodo_CreatesTodo()
        {
            var todo = new Todo { Title = "Integration test", IsCompleted = false };
            var response = await _client.PostAsJsonAsync("/api/todos", todo);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var created = await response.Content.ReadFromJsonAsync<Todo>();
            Assert.Equal("Integration test", created?.Title);
        }

        [Fact]
        public async Task GetTodoById_ReturnsOk_WhenExists()
        {
            var todo = new Todo { Title = "GetById", IsCompleted = false };
            var postResponse = await _client.PostAsJsonAsync("/api/todos", todo);
            var created = await postResponse.Content.ReadFromJsonAsync<Todo>();

            var response = await _client.GetAsync($"/api/todos/{created.Id}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<Todo>();
            Assert.Equal("GetById", result?.Title);
        }

        [Fact]
        public async Task GetTodoById_ReturnsNotFound_WhenNotExists()
        {
            var response = await _client.GetAsync("/api/todos/99999");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PutTodo_UpdatesTodo_WhenExists()
        {
            var todo = new Todo { Title = "ToUpdate", IsCompleted = false };
            var postResponse = await _client.PostAsJsonAsync("/api/todos", todo);
            var created = await postResponse.Content.ReadFromJsonAsync<Todo>();

            created.Title = "Updated";
            created.IsCompleted = true;
            var putResponse = await _client.PutAsJsonAsync($"/api/todos/{created.Id}", created);
            Assert.Equal(HttpStatusCode.NoContent, putResponse.StatusCode);

            var getResponse = await _client.GetAsync($"/api/todos/{created.Id}");
            var updated = await getResponse.Content.ReadFromJsonAsync<Todo>();
            Assert.Equal("Updated", updated?.Title);
            Assert.True(updated?.IsCompleted);
        }

        [Fact]
        public async Task PutTodo_ReturnsNotFound_WhenNotExists()
        {
            var todo = new Todo { Id = 99999, Title = "NotFound", IsCompleted = false };
            var response = await _client.PutAsJsonAsync("/api/todos/99999", todo);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteTodo_DeletesTodo_WhenExists()
        {
            var todo = new Todo { Title = "ToDelete", IsCompleted = false };
            var postResponse = await _client.PostAsJsonAsync("/api/todos", todo);
            var created = await postResponse.Content.ReadFromJsonAsync<Todo>();

            var deleteResponse = await _client.DeleteAsync($"/api/todos/{created.Id}");
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

            var getResponse = await _client.GetAsync($"/api/todos/{created.Id}");
            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
        }

        [Fact]
        public async Task DeleteTodo_ReturnsNotFound_WhenNotExists()
        {
            var response = await _client.DeleteAsync("/api/todos/99999");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PostTodo_ReturnsBadRequest_WhenTitleMissing()
        {
            var todo = new Todo { Title = "", IsCompleted = false };
            var response = await _client.PostAsJsonAsync("/api/todos", todo);
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.UnprocessableEntity);
        }
    }
}