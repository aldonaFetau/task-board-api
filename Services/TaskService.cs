using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskBoardAPI.Data;
using TaskBoardAPI.DTOs;
using TaskBoardAPI.Models;
using TaskBoardAPI.Repositories;

namespace TaskBoardAPI.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskItemDto>> GetTasksAsync(int listId, string? search = null);
        Task<TaskItemDto?> AddTaskAsync(TaskItemDto newTask);
        Task<TaskItemDto?> UpdateTaskAsync(int id, TaskItemDto taskPatch);
        Task<bool> DeleteTaskAsync(int id);
    }

    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly AppDbContext _context;

        public TaskService(ITaskRepository taskRepository, AppDbContext context)
        {
            _taskRepository = taskRepository;
            _context = context;
        }

        public async Task<IEnumerable<TaskItemDto>> GetTasksAsync(int listId, string? search = null)
        {
            var tasks = await _taskRepository.GetTasksByListId(listId, search);
            return tasks.Select(MapToDto);
        }

        public async Task<TaskItemDto?> AddTaskAsync(TaskItemDto newTask)
        {
            var listExists = await _context.TasksList.AnyAsync(l => l.Id == newTask.ListId);
            if (!listExists) return null;

            var entity = new TaskItem
            {
                Title = newTask.Title,
                Description = newTask.Description,
                DueDate = newTask.DueDate,
                Status = newTask.Status,
                ListId = newTask.ListId
            };

            var created = await _taskRepository.AddTask(entity);
            return MapToDto(created);
        }

        public async Task<TaskItemDto?> UpdateTaskAsync(int id, TaskItemDto taskPatch)
        {
            var task = await _taskRepository.GetTaskById(id);
            if (task == null) return null;

            var listExists = await _context.TasksList.AnyAsync(l => l.Id == taskPatch.ListId);
            if (!listExists) return null;

            // Update properties
            task.Title = taskPatch.Title;
            task.Description = taskPatch.Description;
            task.DueDate = taskPatch.DueDate;
            task.Status = taskPatch.Status;
            task.ListId = taskPatch.ListId;

            var updated = await _taskRepository.UpdateTask(task);
            return MapToDto(updated);
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            var task = await _taskRepository.GetTaskById(id);
            if (task == null) return false;

            await _taskRepository.DeleteTask(task);
            return true;
        }

        private TaskItemDto MapToDto(TaskItem task) => new TaskItemDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            DueDate = task.DueDate,
            Status = task.Status,
            ListId = task.ListId
        };
    }
}
