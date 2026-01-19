using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace CustomerInvoice.Customers
{
    /// <summary>
    /// DTO for Customer entity - used for reading customer data
    /// </summary>
    public class CustomerDto : FullAuditedEntityDto<Guid>
    {
        /// <summary>
        /// Customer name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Customer email address
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Customer phone number (optional)
        /// </summary>
        public string? Phone { get; set; }

        /// <summary>
        /// Customer billing address (optional)
        /// </summary>
        public string? BillingAddress { get; set; }
    }
}
