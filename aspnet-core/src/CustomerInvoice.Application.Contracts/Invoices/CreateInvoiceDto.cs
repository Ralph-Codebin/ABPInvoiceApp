using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CustomerInvoice.Invoices
{
    /// <summary>
    /// DTO for creating a new invoice
    /// </summary>
    public class CreateInvoiceDto
    {
        /// <summary>
        /// Customer ID this invoice belongs to
        /// </summary>
        [Required]
        public Guid CustomerId { get; set; }

        /// <summary>
        /// Invoice date (defaults to current date if not provided)
        /// </summary>
        public DateTime? InvoiceDate { get; set; }

        /// <summary>
        /// Due date (optional)
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Tax amount (defaults to 0)
        /// </summary>
        [Range(0, double.MaxValue)]
        public decimal TaxAmount { get; set; }

        /// <summary>
        /// Line items to create with the invoice
        /// </summary>
        public List<CreateLineItemDto> LineItems { get; set; }

        public CreateInvoiceDto()
        {
            LineItems = new List<CreateLineItemDto>();
        }
    }
}
