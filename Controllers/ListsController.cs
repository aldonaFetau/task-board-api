//using Microsoft.AspNetCore.Mvc;
//using System.Threading.Tasks;
//using TaskBoardAPI.Services;
//using TaskBoardAPI.Models;

//namespace TaskBoardAPI.Controllers
//{
//	[ApiController]
//	[Route("api/[controller]")]
//	public class ListsController : ControllerBase
//	{
//		private readonly ListService _listService;

//		public ListsController(ListService listService)
//		{
//			_listService = listService;
//		}

//		[HttpGet]
//		public async Task<IActionResult> GetLists()
//		{
//			var lists = await _listService.GetAllLists();
//			return Ok(lists);
//		}

//		[HttpGet("{id}")]
//		public async Task<IActionResult> GetList(int id)
//		{
//			var list = await _listService.GetListById(id);
//			if (list == null) return NotFound();
//			return Ok(list);
//		}

//		[HttpPost]
//		public async Task<IActionResult> CreateList(TasksList list)
//		{
//			var newList = await _listService.AddList(list);
//			return CreatedAtAction(nameof(GetList), new { id = newList.Id }, newList);
//		}

//		[HttpDelete("{id}")]
//		public async Task<IActionResult> DeleteList(int id)
//		{
//			await _listService.DeleteList(id);
//			return NoContent();
//		}
//	}
//}
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskBoardAPI.Data;
using TaskBoardAPI.DTOs;
using TaskBoardAPI.Models;

namespace TaskBoardAPI.Controllers
{
    [ApiController]
    [Route("lists")]
    public class ListsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ListsController(AppDbContext context)
        {
            _context = context;
        }

        // GET /lists
        [HttpGet]
        public async Task<ActionResult<List<TasksListDto>>> GetLists()
        {
            var lists = await _context.TasksList
                .Include(l => l.Tasks)
                .ToListAsync();

            var listsDto = lists.Select(l => new TasksListDto
            {
                Id = l.Id,
                Title = l.Title,
                Tasks = l.Tasks?.Select(t => new TaskItemDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    DueDate = t.DueDate,
                    Status = t.Status
                }).ToList() ?? new List<TaskItemDto>()
            }).ToList();

            return Ok(listsDto);
        }

        // POST /lists
        [HttpPost]
        public async Task<ActionResult<TasksListDto>> CreateList([FromBody] TasksListDto newList)
        {
            var entity = new TasksList
            {
                Title = newList.Title
            };
            _context.TasksList.Add(entity);
            await _context.SaveChangesAsync();

            newList.Id = entity.Id;
            return CreatedAtAction(nameof(GetLists), new { id = entity.Id }, newList);
        }

        // DELETE /lists/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteList(int id)
        {
            var list = await _context.TasksList
                .Include(l => l.Tasks)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (list == null) return NotFound();

            // Delete all tasks in the list first
            if (list.Tasks != null) _context.Tasks.RemoveRange(list.Tasks);

            _context.TasksList.Remove(list);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
