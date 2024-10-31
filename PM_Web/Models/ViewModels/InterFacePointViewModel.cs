namespace PM.Models.ViewModels
{
    public class InterFacePointViewModel
    {
        public int Id { get; set; }
        public string Nature { get; set; } // Hard Or Soft
        public string Scope { get; set; } // Inter or Exter Or Intra Project Interface
        public string ScopePackage1 { get; set; }
        public string? ScopePackage2 { get; set; }
        public string System1 { get; set; }
        public string? System2 { get; set; }
        public string? ExtraSystem { get; set; }
        public string Category { get; set; } // Phaysical & Funcation or contractual & Organizational Or Resource Or Regulatory Or Other
        public List<string> BOQs { get; set; }
        public List<string> Activities { get; set; }
        public string? Responsible { get; set; }
        public string? Consultant { get; set; }
        public string? Accountable { get; set; }
        public string? Informed { get; set; }
        public string? Supported { get; set; }
        public List<Documentation>? Documentations { get; set; } = new List<Documentation>();
        public string Description { get; set; }
        public int ProjectId { get; set; }
    }
}
