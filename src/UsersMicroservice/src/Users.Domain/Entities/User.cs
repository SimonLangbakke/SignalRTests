namespace Users.Domain.Entities;

public class User
{
    public Guid Id { get; set; } 
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    
    public DateOnly Birthday { get; set; }
    public int Age { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsActive { get; set; }
}