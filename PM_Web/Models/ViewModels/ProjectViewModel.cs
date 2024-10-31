using System.ComponentModel.DataAnnotations;

namespace PM.Models.ViewModels
{
    public class ProjectViewModel
    {
        public int Id { get; set; }

        [Required]
        public string ProjectName { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string ProjectNature { get; set; }

        [Required]
        public string ProjectType { get; set; }

        [Required]
        public int JVPartners { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal ProjectValue { get; set; }

        [Required]
        public string ProjectStage { get; set; }

        [Required]
        public string DeliveryStrategies { get; set; }

        [Required]
        public string ContractingStrategies { get; set; }

        public List<string> Owners { get; set; }

        public List<ScopePackageViewModel> ScopePackages { get; set; } = new List<ScopePackageViewModel>();

        public List<BOQViewModel> BOQs { get; set; } = new List<BOQViewModel>();

        public List<ActivityViewModel> Activities { get; set; } = new List<ActivityViewModel>();

        public List<string> Systems { get; set; }

        public List<DepartmentViewModel> Departments { get; set; } = new List<DepartmentViewModel>();
    }

    public class TeamMember
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class ScopePackageViewModel
    {
        public string Name { get; set; }
        public TeamMember InterfaceManager { get; set; }
    }

    public class DepartmentViewModel
    {
        public string Name { get; set; }
        public TeamMember TeamManager { get; set; }
        public List<TeamMember> TeamMembers { get; set; } = new List<TeamMember>();
    }

    public class BOQViewModel
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Cost { get; set; }
        public string Unit { get; set; }
    }

    public class ActivityViewModel
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
    }

}
