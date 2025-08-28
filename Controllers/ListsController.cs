
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskBoardAPI.Data;
using TaskBoardAPI.DTOs;
using TaskBoardAPI.Models;
using TaskBoardAPI.Services;

namespace TaskBoardAPI.Controllers
{
    [ApiController]
    [Route("lists")]
    public class ListsController : ControllerBase
    {
        private readonly IListService _listService;

        public ListsController(IListService listService)
        {
            _listService = listService;
        }

        // GET /lists
        [HttpGet]
        public async Task<ActionResult<List<TasksListDto>>> GetLists()
        {
            var lists = await _listService.GetAllListsAsync();
            return Ok(lists);
        }

        // POST /lists
        [HttpPost]
        public async Task<ActionResult<TasksListDto>> CreateList([FromBody] TasksListDto newList)
        {
            var created = await _listService.AddListAsync(newList);
            return CreatedAtAction(nameof(GetLists), new { id = created!.Id }, created);
        }

        // DELETE /lists/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteList(int id)
        {
           
            var deleted = await _listService.DeleteListAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
