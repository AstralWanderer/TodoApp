using System.ComponentModel.DataAnnotations;

namespace TodoApp.Models;

public class Todos
{
   [Key]
   public int Id { get; set; }
   
   [Required]
   public string Title { get; set; }
   [Required]
   public bool IsCompleted { get; set; }
   
   [Required]
   public DateOnly DueDate { get; set; }
   
   [Required]
   public TimeOnly DueTime { get; set; }
}