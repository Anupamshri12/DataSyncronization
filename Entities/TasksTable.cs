using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication4.Entities
{
    [Table("Tasks")]
    public class TaskTable
    {
        [Key]
        public required string TaskId { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public int Completed { get; set; } 
        public int IsDeleted { get; set; }
        public string? ServerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
