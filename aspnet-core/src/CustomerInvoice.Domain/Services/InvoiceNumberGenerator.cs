using System;
using System.Linq;
using System.Threading.Tasks;
using CustomerInvoice.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace CustomerInvoice.Services
{
    /// <summary>
    /// Service for generating sequential invoice numbers
    /// </summary>
    public class InvoiceNumberGenerator : DomainService, IInvoiceNumberGenerator
    {
        private readonly IRepository<Invoice, Guid> _invoiceRepository;

        public InvoiceNumberGenerator(IRepository<Invoice, Guid> invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        /// <summary>
        /// Generates the next sequential invoice number in the format: INV-YYYYMM-XXXX
        /// Example: INV-202601-0001
        /// </summary>
        public async Task<string> GenerateAsync()
        {
            var now = DateTime.Now;
            var yearMonth = now.ToString("yyyyMM");
            var prefix = $"INV-{yearMonth}-";

            // Get the last invoice number for the current month
            var lastInvoice = await _invoiceRepository
                .GetQueryableAsync()
                .ContinueWith(t =>
                {
                    var query = t.Result;
                    return query
                        .Where(i => i.InvoiceNumber.StartsWith(prefix))
                        .OrderByDescending(i => i.InvoiceNumber)
                        .FirstOrDefault();
                });

            int nextNumber = 1;

            if (lastInvoice != null)
            {
                // Extract the sequence number from the last invoice number
                var lastNumberPart = lastInvoice.InvoiceNumber.Substring(prefix.Length);
                if (int.TryParse(lastNumberPart, out int lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
            }

            // Format: INV-YYYYMM-XXXX (4-digit sequence number)
            return $"{prefix}{nextNumber:D4}";
        }
    }
}
