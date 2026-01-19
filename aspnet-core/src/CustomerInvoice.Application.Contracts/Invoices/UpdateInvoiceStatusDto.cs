using System.ComponentModel.DataAnnotations;

namespace CustomerInvoice.Invoices
{
    /// <summary>
    /// DTO for updating invoice status
    /// </summary>
    public class UpdateInvoiceStatusDto
    {
        /// <summary>
        /// New invoice status
        /// </summary>
        [Required]
        public InvoiceStatus Status { get; set; }
    }
}
