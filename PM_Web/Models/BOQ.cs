namespace PM.Models;

public class BOQ
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public Project Project { get; set; }

    public int Quantity { get; set; }
    public string Name { get; set; }
    public decimal Cost { get; set; }
    public string Unit { get; set; }

    public List<InterfacePoint> InterfacePoints { get; set; }

}

