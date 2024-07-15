namespace PM.Models;

public class Project
{
    public int Id { get; set; }
    public string ProjectName { get; set; }
    public string Location { get; set; }
    public List<Owner>? Owners { get; set; }
    public string ProjectNature { get; set; }
    public string ProjectType { get; set; }
    public List<ScopePackage>? ScopePackages { get; set; }
    public int JVPartners { get; set; }
    public decimal ProjectValue { get; set; }
    public string ProjectStage { get; set; }
    public string DeliveryStrategies { get; set; }
    public string ContractingStrategies { get; set; }
    public List<BOQ>? BOQs { get; set; }
    public List<Activity>? Activities { get; set; }
    public List<_System>? Systems { get; set; }
    public List<Department>? Departments { get; set; }
    public string OwnerId { get; set; }
    public ApplicationUser Owner { get; set; }
}

public class _System
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int projectId { get; set; }
}

public class Owner
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int projectId { get; set; }
}