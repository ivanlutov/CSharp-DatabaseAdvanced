namespace BusTicketSystem.Models
{
    public class BankAccount
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}