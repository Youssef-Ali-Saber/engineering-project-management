namespace PM.Models;

public class Activity
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public Project Project { get; set; }

    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public List<InterfacePoint> InterfacePoints { get; set; }
}

