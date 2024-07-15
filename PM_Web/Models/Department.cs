namespace PM.Models;

public class Department
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string TeamManagerEmail { get; set; }
    public List<string> TeamMembersEmails { get; set; }
    public int ProjectId { get; set; }
    public Project Project { get; set; }

}

