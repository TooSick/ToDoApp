using ToDoApp.Application.AsyncDataServices;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Filters;
using ToDoApp.Domain.Interfaces;

namespace ToDoApp.Application.Services
{
    public class ToDoService : IService<ToDoItem, ToDoFilter>
    {
        private readonly IRepository<ToDoItem, ToDoFilter> _toDoRepo;
        private readonly IMessageBusClient<ToDoItem> _messageBus;

        public ToDoService(IRepository<ToDoItem, ToDoFilter> toDorepo, IMessageBusClient<ToDoItem> messageBus)
        {
            _toDoRepo = toDorepo;
            _messageBus = messageBus;
        }


        public async Task<int> CreateAsync(ToDoItem entity)
        {
            var id = await _toDoRepo.CreateAsync(entity);
            var createdToDoItem = await GetByIdAsync(id);

            await _messageBus.PublishAsync(createdToDoItem);
            return id;
        }

        public async Task DeleteAsync(int id)
        {
            await _toDoRepo.DeleteAsync(id);
        }

        public async Task<IEnumerable<ToDoItem>> GetAllAsync(ToDoFilter filter)
        {
            return await _toDoRepo.GetAllAsync(filter);
        }

        public async Task<ToDoItem?> GetByIdAsync(int id)
        {
            return await _toDoRepo.GetByIdAsync(id);
        }

        public async Task UpdateAsync(ToDoItem entity)
        {
            await _messageBus.PublishAsync(entity);
            await _toDoRepo.UpdateAsync(entity);
        }
    }
}
