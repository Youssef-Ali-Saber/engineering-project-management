namespace PM.Models
{
    public class InterfacePoint
    {
        public int Id { get; set; }
        public string Nature { get; set; } // Hard Or Soft
        public string Scope { get; set; } // Inter or Exter Or Intra Project Interface
        public string ScopePackage1 { get; set; }
        public string? ScopePackage2 { get; set; }
        public string System1 { get; set; }
        public string? System2 { get; set; }
        public string? ExtraSystem { get; set; }
        public string Category { get; set; }
        public List<BOQ> BOQs { get; set; } = [];
        public List<Activity> Activities { get; set; } = [];
        public string? Responsible { get; set; }
        public string? Consultant { get; set; }
        public string? Accountable { get; set; }
        public string? Informed { get; set; }
        public string? Supported { get; set; }
        public string? Status { get; set; } // pending or aproved or Closed
        public DateTime CreatDate { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public List<Documentation>? Documentations { get; set; } = [];
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public List<Chat> Chat { get; set; } = [];
        public string Description { get; set; }
        public string CreatorId { get; set; }
        public List<int> DepIds { get; set; } = [];
    }
}
