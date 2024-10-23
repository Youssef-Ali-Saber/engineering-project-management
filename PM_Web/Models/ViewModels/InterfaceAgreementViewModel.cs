namespace PM.Models.ViewModels
{
    public class InterfaceAgreementViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime NeedDate { get; set; }
        public List<DateTime>? ModifiedDates { get; set; }
        public List<Documentation>? Documentations { get; set; } = [];
        public int InterfacePointId { get; set; }
        public string System1 { get; set; }
        public string System2 { get; set; }
        public string Discipline { get; set; }
        public DateTime? CloseDate { get; set; }
        public string Status { get; set; }
    }
}
