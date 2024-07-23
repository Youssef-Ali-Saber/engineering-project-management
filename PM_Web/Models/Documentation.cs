
using System.ComponentModel.DataAnnotations.Schema;

namespace PM.Models
{
    public class Documentation
    {
        public int Id { get; set; }
        public string? DocumentationLink { get; set; }
        [NotMapped]
        public IFormFile? DocumentationFile { get; set; }
        public string? DocumentationDescription { get; set; }
        public int? InterfacePointId { get; set; }
        public int? InterfaceAgreementId { get; set; }
    }
}
