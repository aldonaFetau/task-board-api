using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskBoardAPI.Data;
using TaskBoardAPI.DTOs;
using TaskBoardAPI.Models;
using TaskBoardAPI.Services;

namespace TaskBoardAPI.Controllers
{
    [ApiController]
    [Route("tasks")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }


        // GET /tasks?listId=1&search=keyword
        [HttpGet]
        public async Task<IActionResult> GetTasks([FromQuery] int listId, [FromQuery] string? search)
        {
            var tasks = await _taskService.GetTasksAsync(listId, search);
            return Ok(tasks);
        }


        // POST /tasks
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskItemDto newTask)
        {
            var created = await _taskService.AddTaskAsync(newTask);
            if (created == null) return BadRequest($"TasksList with Id {newTask.ListId} does not exist.");
            return CreatedAtAction(nameof(GetTasks), new { listId = created.ListId }, created);
        }

        // PUT /tasks/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskItemDto taskPatch)
        {
            var updated = await _taskService.UpdateTaskAsync(id, taskPatch);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        // DELETE /tasks/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var deleted = await _taskService.DeleteTaskAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
