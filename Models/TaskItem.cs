using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TaskBoardAPI.Models
{
    public class TaskItem
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        [Required]
       public BoardTaskStatus Status { get; set; } // "To Do", "In Progress", "Completed" 
        [ForeignKey("TasksList")]
        public int ListId { get; set; }

        [JsonIgnore] //prevent circular serialization
        public TasksList? TasksList { get; set; }

    }
    [JsonConverter(typeof(JsonStringEnumConverter))] // <- serialize as string
    public enum BoardTaskStatus
    {
        ToDo,       
        InProgress, 
        Completed   
    }

}
