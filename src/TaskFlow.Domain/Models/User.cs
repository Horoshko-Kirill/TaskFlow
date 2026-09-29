namespace TaskFlow.Domain.Models;

public class User
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string Email { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public ICollection<Task> AssignedTasks { get; set; } = new  List<Task>();
    
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}