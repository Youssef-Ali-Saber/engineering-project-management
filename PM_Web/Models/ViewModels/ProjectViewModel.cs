using System.ComponentModel.DataAnnotations;

namespace PM01.Models.ViewModels
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

        public List<Owner> Owners { get; set; } = new List<Owner>();

        public List<ScopePackage> ScopePackages { get; set; } = new List<ScopePackage>();

        public List<BOQ> BOQs { get; set; } = new List<BOQ>();

        public List<Activity> Activities { get; set; } = new List<Activity>();
    }
}
