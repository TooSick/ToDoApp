using Moq;
using ToDoApp.Application.AsyncDataServices;
using ToDoApp.Application.Services;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Enums;
using ToDoApp.Domain.Filters;
using ToDoApp.Domain.Interfaces;

namespace ToDoApp.Application.Tests
{
    public class ToDoServiceTests
    {
        private readonly Mock<IRepository<ToDoItem, ToDoFilter>> _mockRepo;
        private readonly Mock<IMessageBusClient<ToDoItem>> _mockBus;
        private readonly ToDoService _toDoService;

        public ToDoServiceTests()
        {
            _mockRepo = new Mock<IRepository<ToDoItem, ToDoFilter>>();
            _mockBus = new Mock<IMessageBusClient<ToDoItem>>();
            _toDoService = new ToDoService(_mockRepo.Object, _mockBus.Object);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnNewItemId()
        {
            var newToDoItem = new ToDoItem { Title = "Test Task", Priority = Priority.High, DueDate = DateTime.UtcNow };
            _mockRepo.Setup(repo => repo.CreateAsync(newToDoItem)).ReturnsAsync(1);

            var result = await _toDoService.CreateAsync(newToDoItem);

            Assert.Equal(1, result);
            _mockRepo.Verify(repo => repo.CreateAsync(newToDoItem), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallRepositoryDelete()
        {
            var idToDelete = 1;
            _mockRepo.Setup(repo => repo.DeleteAsync(idToDelete)).Returns(Task.CompletedTask);

            await _toDoService.DeleteAsync(idToDelete);

            _mockRepo.Verify(repo => repo.DeleteAsync(idToDelete), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnFilteredToDoItems()
        {
            var filter = new ToDoFilter { Priority = (int?)Priority.Medium };
            var expectedItems = new List<ToDoItem>
        {
            new ToDoItem { Id = 1, Title = "Task 1", Priority = Priority.Medium },
            new ToDoItem { Id = 2, Title = "Task 2", Priority = Priority.Medium }
        };

            _mockRepo.Setup(repo => repo.GetAllAsync(filter)).ReturnsAsync(expectedItems);

            var result = await _toDoService.GetAllAsync(filter);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal(expectedItems, result);
            _mockRepo.Verify(repo => repo.GetAllAsync(filter), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnItem_WhenIdExists()
        {
            var id = 1;
            var expectedItem = new ToDoItem { Id = id, Title = "Test Task" };

            _mockRepo.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(expectedItem);

            var result = await _toDoService.GetByIdAsync(id);

            Assert.NotNull(result);
            Assert.Equal(expectedItem, result);
            _mockRepo.Verify(repo => repo.GetByIdAsync(id), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenIdDoesNotExist()
        {
            var id = 1;

            _mockRepo.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync((ToDoItem?)null);

            var result = await _toDoService.GetByIdAsync(id);

            Assert.Null(result);
            _mockRepo.Verify(repo => repo.GetByIdAsync(id), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldCallRepositoryUpdate()
        {
            var itemToUpdate = new ToDoItem { Id = 1, Title = "Updated Task" };
            _mockRepo.Setup(repo => repo.UpdateAsync(itemToUpdate)).Returns(Task.CompletedTask);

            await _toDoService.UpdateAsync(itemToUpdate);

            _mockRepo.Verify(repo => repo.UpdateAsync(itemToUpdate), Times.Once);
        }
    }
}