using System.Collections.Generic;
using System.Threading.Tasks;
using TaskBoardAPI.Models;
using TaskBoardAPI.Repositories;

namespace TaskBoardAPI.Services
{
	public class TaskService
	{
		private readonly ITaskRepository _taskRepository;

		public TaskService(ITaskRepository taskRepository)
		{
			_taskRepository = taskRepository;
		}

		public Task<IEnumerable<TaskItem>> GetTasksByListId(int listId) =>
			_taskRepository.GetTasksByListId(listId);

		public Task<TaskItem> GetTaskById(int id) =>
			_taskRepository.GetTaskById(id);

		public Task<TaskItem> AddTask(TaskItem task) =>
			_taskRepository.AddTask(task);

		public Task<TaskItem> UpdateTask(TaskItem task) =>
			_taskRepository.UpdateTask(task);

		public Task DeleteTask(int id) =>
			_taskRepository.DeleteTask(id);
	}
}
