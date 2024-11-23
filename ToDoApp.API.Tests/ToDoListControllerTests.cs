using Microsoft.AspNetCore.Mvc;
using Moq;
using ToDoApp.API.Controllers;
using ToDoApp.API.ViewModels;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Enums;
using ToDoApp.Domain.Filters;
using ToDoApp.Domain.Interfaces;

namespace ToDoApp.API.Tests
{
    public class ToDoListControllerTests
    {
        private readonly Mock<IService<ToDoItem, ToDoFilter>> _serviceMock;
        private readonly ToDoListController _controller;

        public ToDoListControllerTests()
        {
            _serviceMock = new Mock<IService<ToDoItem, ToDoFilter>>();
            _controller = new ToDoListController(_serviceMock.Object);
        }

        [Fact]
        public async Task GetAllToDoItems_ShouldReturnOkWithItems()
        {
            var items = new List<ToDoItem>
            {
                new ToDoItem { Id = 1, Title = "Task 1", Priority = Priority.High, Status = Status.New },
                new ToDoItem { Id = 2, Title = "Task 2", Priority = Priority.Low, Status = Status.Completed }
            };

            _serviceMock
                .Setup(s => s.GetAllAsync(It.IsAny<ToDoFilter>()))
                .ReturnsAsync(items);

            var result = await _controller.GetAllToDoItems(new ToDoFilterViewModel());
            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = Assert.IsAssignableFrom<IEnumerable<ToDoItemViewModel>>(okResult.Value);

            Assert.Equal(2, data.Count());
            Assert.Contains(data, i => i.Title == "Task 1");
        }

        [Fact]
        public async Task GetToDoItemById_ShouldReturnOkWithItem()
        {
            var item = new ToDoItem
            {
                Id = 1,
                Title = "Task 1",
                Priority = Priority.High,
                Status = Status.New
            };

            _serviceMock
                .Setup(s => s.GetByIdAsync(1))
                .ReturnsAsync(item);

            var result = await _controller.GetToDoItemById(1);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = Assert.IsType<ToDoItemViewModel>(okResult.Value);

            Assert.Equal(1, data.Id);
            Assert.Equal("Task 1", data.Title);
        }

        [Fact]
        public async Task GetToDoItemById_ShouldReturnNotFound_WhenItemDoesNotExist()
        {
            _serviceMock
                .Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((ToDoItem)null);

            var result = await _controller.GetToDoItemById(1);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task CreateToDoItem_ShouldReturnCreatedAtActionWithItem()
        {
            var newItem = new ToDoItem
            {
                Id = 1,
                Title = "New Task",
                Priority = Priority.High,
                Status = Status.New
            };

            _serviceMock
                .Setup(s => s.CreateAsync(It.IsAny<ToDoItem>()))
                .ReturnsAsync(1);
            _serviceMock
                .Setup(s => s.GetByIdAsync(1))
                .ReturnsAsync(newItem);

            var createModel = new CreateToDoItemViewModel
            {
                Title = "New Task",
                Description = "New Description",
                Priority = Priority.High,
                Status = Status.New,
                DueDate = new System.DateTime(2024, 12, 31)
            };

            var result = await _controller.CreateToDoItem(createModel);
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var data = Assert.IsType<ToDoItemViewModel>(createdResult.Value);

            Assert.Equal(1, data.Id);
            Assert.Equal("New Task", data.Title);
        }

        [Fact]
        public async Task UpdateToDoItem_ShouldReturnOk()
        {
            var updateModel = new UpdateToDoItemViewModel
            {
                Title = "Updated Task",
                Description = "Updated Description",
                Priority = Priority.Low,
                Status = Status.Completed,
                DueDate = new System.DateTime(2024, 11, 23)
            };

            var result = await _controller.UpdateToDoItem(1, updateModel);

            var okResult = Assert.IsType<OkResult>(result);
            _serviceMock.Verify(s => s.UpdateAsync(It.IsAny<ToDoItem>()), Times.Once);
        }

        [Fact]
        public async Task DeleteToDoItem_ShouldReturnOk()
        {
            var result = await _controller.DeleteToDoItem(1);

            var okResult = Assert.IsType<OkResult>(result);
            _serviceMock.Verify(s => s.DeleteAsync(1), Times.Once);
        }
    }
}