using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskBoardAPI.Data;
using TaskBoardAPI.Models;

namespace TaskBoardAPI.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<TaskItem>> GetTasksByListId(int listId, string? title)
        {
            var query = _context.Tasks.AsQueryable();

            if (listId > 0)
                query = query.Where(t => t.ListId == listId);

            if (!string.IsNullOrWhiteSpace(title))
                query = query.Where(t => t.Title.Contains(title));

            return await query.ToListAsync();
        }

        public async Task<TaskItem> GetTaskById(int id)
        {
            return await _context.Tasks.FindAsync(id);
        }

        public async Task<TaskItem> AddTask(TaskItem task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<TaskItem> UpdateTask(TaskItem task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task DeleteTask(TaskItem task)
        {
            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }
        }
    }
}

