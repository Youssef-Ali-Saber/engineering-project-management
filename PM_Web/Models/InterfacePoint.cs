﻿
using System.ComponentModel.DataAnnotations.Schema;

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
        public string Category { get; set; } // Phaysical & Funcation or contractual & Organizational Or Resource Or Regulatory Or Other
        public BOQ? BOQ { get; set; }
        public Activity? Activity { get; set; }
        public string? Responsible { get; set; }
        public string? Consultant { get; set; }
        public string? Accountable { get; set; }
        public string? Informed { get; set; }
        public string? Supported { get; set; }
        public string? Status { get; set; } // pending or aproved or Closed
        public DateTime CreatDate { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public List<Documentation>? Documentations { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public List<Chat> chat { get; set; }


    }

    public class Documentation
    {
        public int Id { get; set; }
        public string? DocumentationLink { get; set; }
        [NotMapped]
        public IFormFile? DocumentationFile { get; set; }
        public string? DocumentationDescription { get; set; }
        public int InterfacePointId { get; set; }
    }

    public class Chat
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Sender { get; set; }
        public DateTime Time { get; set; }
        public int InterfacePointId { get; set; }
    }
}