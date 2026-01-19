using System;
using System.Threading.Tasks;
using CustomerInvoice.Entities;
using CustomerInvoice.Services;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace CustomerInvoice.Data
{
    /// <summary>
    /// Seeds sample customer and invoice data for testing
    /// </summary>
    public class CustomerInvoiceDataSeederContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<Customer, Guid> _customerRepository;
        private readonly IRepository<Invoice, Guid> _invoiceRepository;
        private readonly IRepository<LineItem, Guid> _lineItemRepository;
        private readonly IInvoiceNumberGenerator _invoiceNumberGenerator;

        public CustomerInvoiceDataSeederContributor(
            IRepository<Customer, Guid> customerRepository,
            IRepository<Invoice, Guid> invoiceRepository,
            IRepository<LineItem, Guid> lineItemRepository,
            IInvoiceNumberGenerator invoiceNumberGenerator)
        {
            _customerRepository = customerRepository;
            _invoiceRepository = invoiceRepository;
            _lineItemRepository = lineItemRepository;
            _invoiceNumberGenerator = invoiceNumberGenerator;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            // Check if sample data already exists
            if (await _customerRepository.GetCountAsync() > 0)
            {
                return; // Data already seeded
            }

            // Seed sample customers
            var customer1 = new Customer(
                Guid.NewGuid(),
                "Acme Corporation",
                "contact@acme.com",
                "+1-555-0100",
                "123 Main St, New York, NY 10001"
            );

            var customer2 = new Customer(
                Guid.NewGuid(),
                "Global Tech Solutions",
                "info@globaltech.com",
                "+1-555-0200",
                "456 Tech Blvd, San Francisco, CA 94105"
            );

            var customer3 = new Customer(
                Guid.NewGuid(),
                "Blue Ocean Enterprises",
                "sales@blueocean.com",
                "+1-555-0300",
                "789 Ocean Dr, Miami, FL 33139"
            );

            await _customerRepository.InsertAsync(customer1);
            await _customerRepository.InsertAsync(customer2);
            await _customerRepository.InsertAsync(customer3);

            // Seed sample invoices for customer1
            var invoice1 = new Invoice(
                Guid.NewGuid(),
                await _invoiceNumberGenerator.GenerateAsync(),
                customer1.Id,
                DateTime.Now.AddDays(-30),
                DateTime.Now.AddDays(-15),
                InvoiceStatus.Draft // Create as Draft first to allow adding line items
            )
            {
                TaxAmount = 50.00m
            };

            var lineItem1_1 = new LineItem(
                Guid.NewGuid(),
                invoice1.Id,
                "Website Development Services",
                40m, // 40 hours
                125.00m // $125/hour
            );

            var lineItem1_2 = new LineItem(
                Guid.NewGuid(),
                invoice1.Id,
                "Hosting Setup and Configuration",
                5m,
                100.00m
            );

            invoice1.AddLineItem(lineItem1_1);
            invoice1.AddLineItem(lineItem1_2);

            // Now update status to Paid after line items are added
            invoice1.UpdateStatus(InvoiceStatus.Paid);

            await _invoiceRepository.InsertAsync(invoice1);

            // Seed invoice for customer2 (pending)
            var invoice2 = new Invoice(
                Guid.NewGuid(),
                await _invoiceNumberGenerator.GenerateAsync(),
                customer2.Id,
                DateTime.Now.AddDays(-7),
                DateTime.Now.AddDays(23),
                InvoiceStatus.Sent
            )
            {
                TaxAmount = 120.00m
            };

            var lineItem2_1 = new LineItem(
                Guid.NewGuid(),
                invoice2.Id,
                "Cloud Infrastructure Setup",
                80m,
                150.00m
            );

            var lineItem2_2 = new LineItem(
                Guid.NewGuid(),
                invoice2.Id,
                "Security Audit",
                20m,
                175.00m
            );

            var lineItem2_3 = new LineItem(
                Guid.NewGuid(),
                invoice2.Id,
                "Performance Optimization",
                15m,
                160.00m
            );

            invoice2.AddLineItem(lineItem2_1);
            invoice2.AddLineItem(lineItem2_2);
            invoice2.AddLineItem(lineItem2_3);

            await _invoiceRepository.InsertAsync(invoice2);

            // Seed draft invoice for customer3
            var invoice3 = new Invoice(
                Guid.NewGuid(),
                await _invoiceNumberGenerator.GenerateAsync(),
                customer3.Id,
                DateTime.Now,
                DateTime.Now.AddDays(30),
                InvoiceStatus.Draft
            )
            {
                TaxAmount = 75.00m
            };

            var lineItem3_1 = new LineItem(
                Guid.NewGuid(),
                invoice3.Id,
                "Mobile App Development",
                100m,
                125.00m
            );

            invoice3.AddLineItem(lineItem3_1);

            await _invoiceRepository.InsertAsync(invoice3);
        }
    }
}
