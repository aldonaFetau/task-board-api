using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskBoardAPI.Data;
using TaskBoardAPI.Models;

namespace TaskBoardAPI.Repositories
{
    public class ListRepository : IListRepository
    {
        private readonly AppDbContext _context;
        public ListRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TasksList>> GetAllLists() =>
            await _context.TasksList.Include(l => l.Tasks).ToListAsync();

        public async Task<TasksList> GetListById(int id) =>
            await _context.TasksList.Include(l => l.Tasks).FirstOrDefaultAsync(l => l.Id == id);

        public async Task<TasksList> AddList(TasksList list)
        {
            _context.TasksList.Add(list);
            await _context.SaveChangesAsync();
            return list;
        }

        public async Task DeleteList(TasksList list)
        {
  
            if (list != null)
            {
               
                if (list.Tasks != null) _context.Tasks.RemoveRange(list.Tasks);
                _context.TasksList.Remove(list);
                await _context.SaveChangesAsync();
            }
        }
      
    }
}
