using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaskBoardAPI.Models
{
    public class TasksList
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public ICollection<TaskItem>? Tasks { get; set; }
    }
}
