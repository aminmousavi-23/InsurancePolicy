namespace UserService.Entities;

public class Role
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string RoleName { get; set; }
}
