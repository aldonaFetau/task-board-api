using System.Collections.Generic;
using System.Threading.Tasks;
using TaskBoardAPI.Models;

namespace TaskBoardAPI.Repositories
{
    public interface IListRepository
    {
        Task<IEnumerable<TasksList>> GetAllLists();
        Task<TasksList> GetListById(int id);
        Task<TasksList> AddList(TasksList list);  
        Task DeleteList(int id);
    }
}
