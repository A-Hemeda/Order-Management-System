using System;

namespace OrderManagementSystem.DTOs
{
    public class InvoiceResponse
    {
        public int InvoiceId { get; set; } // Unique identifier for the invoice
        public int OrderId { get; set; } // ID of the associated order
        public DateTime InvoiceDate { get; set; } // Date and time when the invoice was created
        public decimal TotalAmount { get; set; } // Total amount on the invoice
    }

    public class CreateInvoiceRequest
    {
        public int OrderId { get; set; } // ID of the order for which the invoice is created
        public decimal TotalAmount { get; set; } // Total amount for the invoice
    }
} 