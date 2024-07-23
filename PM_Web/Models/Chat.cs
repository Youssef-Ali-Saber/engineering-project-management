namespace PM.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Sender { get; set; }
        public DateTime Time { get; set; }
        public int? InterfacePointId { get; set; }
        public int? InterfaceAgreementId { get; set; }
    }
}
