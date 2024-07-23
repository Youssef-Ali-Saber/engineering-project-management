namespace PM.Models.ViewModels
{
    public class InterfaceAgreementViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime NeedDate { get; set; }
        public List<Documentation>? Documentations { get; set; } = new List<Documentation>();
        public int InterfacePointId { get; set; }
        public string System { get; set; }
        public string Discipline { get; set; }
    }
}
