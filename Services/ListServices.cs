using System.Collections.Generic;
using System.Threading.Tasks;
using TaskBoardAPI.Models;
using TaskBoardAPI.Repositories;

namespace TaskBoardAPI.Services
{
    public class ListService
    {
        private readonly IListRepository _listRepository;

        public ListService(IListRepository listRepository)
        {
            _listRepository = listRepository;
        }

        public Task<IEnumerable<TasksList>> GetAllLists() => _listRepository.GetAllLists();
        public Task<TasksList> GetListById(int id) => _listRepository.GetListById(id);
        public Task<TasksList> AddList(TasksList list) => _listRepository.AddList(list);
        public Task DeleteList(int id) => _listRepository.DeleteList(id);
    }
}
