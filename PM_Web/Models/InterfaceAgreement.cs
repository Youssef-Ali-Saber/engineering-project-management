namespace PM.Models
{
    public class InterfaceAgreement
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public DateTime NeedDate { get; set; }
        public List<DateTime>? ModifiedDates { get; set; }
        public string AccountableTeamMemberEmail { get; set; }
        public string System1 { get; set; }
        public string System2 { get; set; }
        public string Discipline { get; set; }
        public List<Documentation> Documentations { get; set; } = [];
        public List<Chat> Chat { get; set; } = [];
        public int InterfacePointId { get; set; }
        public InterfacePoint InterfacePoint { get; set; }
    }
}
