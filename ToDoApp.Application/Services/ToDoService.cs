using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Filters;
using ToDoApp.Domain.Interfaces;

namespace ToDoApp.Application.Services
{
    public class ToDoService : IService<ToDoItem, ToDoFilter>
    {
        private readonly IRepository<ToDoItem, ToDoFilter> _toDoRepo;


        public ToDoService(IRepository<ToDoItem, ToDoFilter> toDorepo)
        {
            _toDoRepo = toDorepo;
        }


        public async Task<int> CreateAsync(ToDoItem entity)
        {
            return await _toDoRepo.CreateAsync(entity);
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
            await _toDoRepo.UpdateAsync(entity);
        }
    }
}
