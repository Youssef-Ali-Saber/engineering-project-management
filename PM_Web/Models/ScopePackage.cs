namespace PM.Models;

public class ScopePackage
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string ManagerEmail { get; set; } = null!;
    public List<string> TeamEmails { get; set; } = [];
    public int ProjectId { get; set; }
    public Project Project { get; set; }
}

