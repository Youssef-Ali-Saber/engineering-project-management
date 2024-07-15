namespace PM.Models;

public class ScopePackage
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ManagerEmail { get; set; }
    public int ProjectId { get; set; }
    public Project Project { get; set; }

}

