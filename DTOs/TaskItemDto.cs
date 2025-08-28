using TaskBoardAPI.Models;

namespace TaskBoardAPI.DTOs
{
    public class TaskItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public BoardTaskStatus Status { get; set; }
        public int ListId { get; set; }
        public string ListTitle { get; set; } = "";
    }
}

