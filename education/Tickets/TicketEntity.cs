namespace education.Tickets
{
    public class TicketEntity
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int TicketId { get; set; }
        public DateTime Time { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } 
        public string Description { get; set; } 
    }
}
