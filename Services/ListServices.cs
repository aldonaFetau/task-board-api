using System.Collections.Generic;
using System.Threading.Tasks;
using TaskBoardAPI.DTOs;
using TaskBoardAPI.Models;
using TaskBoardAPI.Repositories;

namespace TaskBoardAPI.Services
{
       public interface IListService
    {
        Task<IEnumerable<TasksListDto>> GetAllListsAsync();
        Task<TasksListDto?> AddListAsync(TasksListDto newList);
        Task<bool> DeleteListAsync(int id);
    }

    public class ListService : IListService
    {
        private readonly IListRepository _listRepository;

        public ListService(IListRepository listRepository)
        {
            _listRepository = listRepository;
        }

 

        public async Task<IEnumerable<TasksListDto>> GetAllListsAsync()
        {
            var lists = await _listRepository.GetAllLists();
            return lists.Select(MapToDto);
        }

        public async Task<TasksListDto?> AddListAsync(TasksListDto newList)
        {
            var entity = new TasksList { Title = newList.Title };
            var created = await _listRepository.AddList(entity);
            return MapToDto(created);
        }

        public async Task<bool> DeleteListAsync(int id)
        {
            var list = await _listRepository.GetListById(id);
            if (list == null) return false;

            await _listRepository.DeleteList(list);
            return true;
        }

        private TasksListDto MapToDto(TasksList list) => new TasksListDto
        {
            Id = list.Id,
            Title = list.Title,
            Tasks = list.Tasks?.Select(t => new TaskItemDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                DueDate = t.DueDate,
                Status = t.Status,
                ListId = t.ListId,
                ListTitle = list.Title
            }).ToList() ?? new List<TaskItemDto>()
        };
    }

}
