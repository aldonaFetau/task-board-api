using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskBoardAPI.Data;
using TaskBoardAPI.DTOs;
using TaskBoardAPI.Models;

namespace TaskBoardAPI.Controllers
{
    [ApiController]
    [Route("tasks")]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TasksController(AppDbContext context)
        {
            _context = context;
        }

        // GET /tasks?listId=1
        [HttpGet]
        public async Task<ActionResult<List<TaskItemDto>>> GetTasks([FromQuery] int listId)
        {
            var tasks = await _context.Tasks
                .Where(t => t.ListId == listId)
                .ToListAsync();

            var tasksDto = tasks.Select(t => new TaskItemDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                DueDate = t.DueDate,
                Status = t.Status,
                ListId = t.ListId
            }).ToList();

            return Ok(tasksDto);
        }

        // POST /tasks
        [HttpPost]
        public async Task<ActionResult<TaskItemDto>> CreateTask([FromBody] TaskItemDto newTask)
        {
            // check if list exists
            var listExists = await _context.TasksList.AnyAsync(l => l.Id == newTask.ListId);
            if (!listExists)
                return BadRequest($"TasksList with Id {newTask.ListId} does not exist.");

            var entity = new TaskItem
            {
                Title = newTask.Title,
                Description = newTask.Description,
                DueDate = newTask.DueDate,
                Status = newTask.Status,
                ListId = newTask.ListId
            };

            _context.Tasks.Add(entity);
            await _context.SaveChangesAsync();

        
            var createdDto = new TaskItemDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                DueDate = entity.DueDate,
                Status = entity.Status,
                ListId = entity.ListId
            };

            return CreatedAtAction(nameof(GetTasks), new { listId = entity.ListId }, createdDto);
        }

        // PUT /tasks/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<TaskItemDto>> UpdateTask(int id, [FromBody] TaskItemDto taskPatch)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return NotFound();

      
            var listExists = await _context.TasksList.AnyAsync(l => l.Id == taskPatch.ListId);
            if (!listExists)
                return BadRequest($"TasksList with Id {taskPatch.ListId} does not exist.");

            // Update task properties
            task.Title = taskPatch.Title;
            task.Description = taskPatch.Description;
            task.DueDate = taskPatch.DueDate;
            task.Status = taskPatch.Status;
            task.ListId = taskPatch.ListId;

            await _context.SaveChangesAsync();

            var updatedDto = new TaskItemDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                Status = task.Status,
                ListId = task.ListId
            };

            return Ok(updatedDto);
        }

        // DELETE /tasks/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return NotFound();

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
