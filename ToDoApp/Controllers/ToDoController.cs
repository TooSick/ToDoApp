using Microsoft.AspNetCore.Mvc;
using ToDoApp.API.ViewModels;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Enums;
using ToDoApp.Domain.Filters;
using ToDoApp.Domain.Interfaces;

namespace ToDoApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly IService<ToDoItem, ToDoFilter> _toDoService;

        public ToDoController(IService<ToDoItem, ToDoFilter> toDoService)
        {
            _toDoService = toDoService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllToDoItems([FromQuery] ToDoFilterViewModel filter)
        {
            var toDoFilter = new ToDoFilter
            {
                DueDate = filter.DueDate,
            };

            if (!string.IsNullOrEmpty(filter.Priority) && Enum.TryParse(filter.Priority, out Priority priority))
                toDoFilter.Priority = (int)priority;
            if (!string.IsNullOrEmpty(filter.Status) && Enum.TryParse(filter.Status, out Status status))
                toDoFilter.Status = (int)status;

            var toDoItems = (await _toDoService.GetAllAsync(toDoFilter))
                .Select(i =>
                {
                    return new ToDoItemViewModel
                    {
                        Id = i.Id,
                        Title = i.Title,
                        Description = i.Description,
                        DueDate = i.DueDate,
                        Priority = i.Priority.ToString(),
                        Status = i.Status.ToString(),
                    };
                });

            return Ok(toDoItems);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetToDoItemById(int id)
        {
            var toDoItem = await _toDoService.GetByIdAsync(id);

            if (toDoItem == null)
                return NotFound(new { Id = id, Message = $"ToDo whith id={id} do not exist!" });

            var toDoItemDto = new ToDoItemViewModel
            {
                Id = toDoItem.Id,
                Title = toDoItem.Title,
                Description = toDoItem.Description,
                DueDate = toDoItem.DueDate,
                Priority = toDoItem.Priority.ToString(),
                Status = toDoItem.Status.ToString(),
            };

            return Ok(toDoItemDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateToDoItem([FromBody] CreateToDoItemViewModel toDoItemDto)
        {
            var toDoItem = new ToDoItem
            {
                Title = toDoItemDto.Title,
                Description = toDoItemDto.Description,
                DueDate = toDoItemDto.DueDate,
                Status = toDoItemDto.Status,
                Priority = toDoItemDto.Priority
            };

            var createdId = await _toDoService.CreateAsync(toDoItem);
            var createdToDoItem = await _toDoService.GetByIdAsync(createdId);

            var createToDoItemDto = new ToDoItemViewModel
            {
                Id = createdToDoItem.Id,
                Title = createdToDoItem.Title,
                Description = createdToDoItem.Description,
                Status = createdToDoItem.Status.ToString(),
                DueDate = createdToDoItem.DueDate,
                Priority = createdToDoItem.Priority.ToString(),
            };

            return CreatedAtAction(nameof(GetToDoItemById),
                new { id = createdId }, createToDoItemDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateToDoItem(int id, [FromBody] UpdateToDoItemViewModel updateToDoItem)
        {
            var toDoItem = new ToDoItem
            {
                Id = id,
                Title = updateToDoItem.Title,
                Description = updateToDoItem.Description,
                Priority = updateToDoItem.Priority,
                DueDate = updateToDoItem.DueDate,
                Status = updateToDoItem.Status,
            };

            await _toDoService.UpdateAsync(toDoItem);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDoItem(int id)
        {
            await _toDoService.DeleteAsync(id);
            return Ok();
        }
    }
}
