namespace OrderManagementSystem.Models
{
    public class Invoice
    {
        public int InvoiceId { get; set; } // Unique identifier for the invoice
        public int OrderId { get; set; } // Foreign key to the order
        public DateTime InvoiceDate { get; set; } = DateTime.UtcNow; // Date and time when the invoice was created
        public decimal TotalAmount { get; set; } // Total amount on the invoice

        public Order Order { get; set; } // Navigation property to the order
    }
}
