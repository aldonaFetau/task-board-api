namespace TaskBoardAPI.DTOs
{
    public class TasksListDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<TaskItemDto> Tasks { get; set; } = new();
    }
}
