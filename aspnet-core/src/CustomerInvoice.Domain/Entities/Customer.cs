using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace CustomerInvoice.Entities
{
    /// <summary>
    /// Represents a customer in the system
    /// </summary>
    public class Customer : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// Customer name (required)
        /// </summary>
        [Required]
        [MaxLength(CustomerConsts.MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// Customer email address (required, unique)
        /// </summary>
        [Required]
        [MaxLength(CustomerConsts.MaxEmailLength)]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Customer phone number (optional)
        /// </summary>
        [MaxLength(CustomerConsts.MaxPhoneLength)]
        public string? Phone { get; set; }

        /// <summary>
        /// Customer billing address (optional)
        /// </summary>
        [MaxLength(CustomerConsts.MaxBillingAddressLength)]
        public string? BillingAddress { get; set; }

        /// <summary>
        /// Navigation property for customer's invoices
        /// </summary>
        public virtual ICollection<Invoice> Invoices { get; set; }

        /// <summary>
        /// Protected constructor for ORM
        /// </summary>
        protected Customer()
        {
            Invoices = new List<Invoice>();
        }

        /// <summary>
        /// Creates a new customer
        /// </summary>
        public Customer(Guid id, string name, string email, string phone = null, string billingAddress = null)
            : base(id)
        {
            SetName(name);
            SetEmail(email);
            Phone = phone;
            BillingAddress = billingAddress;
            Invoices = new List<Invoice>();
        }

        /// <summary>
        /// Sets the customer name with validation
        /// </summary>
        public void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(Name),
                CustomerConsts.MaxNameLength
            );
        }

        /// <summary>
        /// Sets the customer email with validation
        /// </summary>
        public void SetEmail(string email)
        {
            Email = Check.NotNullOrWhiteSpace(
                email,
                nameof(Email),
                CustomerConsts.MaxEmailLength
            );
        }

        /// <summary>
        /// Updates customer details
        /// </summary>
        public void Update(string name, string email, string? phone, string? billingAddress)
        {
            SetName(name);
            SetEmail(email);
            Phone = phone;
            BillingAddress = billingAddress;
        }
    }

    /// <summary>
    /// Constants for Customer entity
    /// </summary>
    public static class CustomerConsts
    {
        public const int MaxNameLength = 200;
        public const int MaxEmailLength = 256;
        public const int MaxPhoneLength = 50;
        public const int MaxBillingAddressLength = 500;
    }
}
