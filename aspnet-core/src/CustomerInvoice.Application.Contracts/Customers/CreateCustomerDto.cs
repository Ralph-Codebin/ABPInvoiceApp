using System.ComponentModel.DataAnnotations;

namespace CustomerInvoice.Customers
{
    /// <summary>
    /// DTO for creating a new customer
    /// </summary>
    public class CreateCustomerDto
    {
        /// <summary>
        /// Customer name
        /// </summary>
        [Required]
        [StringLength(CustomerConsts.MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// Customer email address
        /// </summary>
        [Required]
        [EmailAddress]
        [StringLength(CustomerConsts.MaxEmailLength)]
        public string Email { get; set; }

        /// <summary>
        /// Customer phone number (optional)
        /// </summary>
        [StringLength(CustomerConsts.MaxPhoneLength)]
        public string? Phone { get; set; }

        /// <summary>
        /// Customer billing address (optional)
        /// </summary>
        [StringLength(CustomerConsts.MaxBillingAddressLength)]
        public string? BillingAddress { get; set; }
    }
}
