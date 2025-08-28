using System.Collections.Generic;
using System.Threading.Tasks;
using TaskBoardAPI.Models;

namespace TaskBoardAPI.Repositories
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskItem>> GetTasksByListId(int listId, string? title);
        Task<TaskItem> GetTaskById(int id);
        Task<TaskItem> AddTask(TaskItem task);
        Task<TaskItem> UpdateTask(TaskItem task);
        Task DeleteTask(TaskItem id);
    }
}
