using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace CustomerInvoice.Services
{
    /// <summary>
    /// Interface for invoice number generation service
    /// </summary>
    public interface IInvoiceNumberGenerator : IDomainService
    {
        /// <summary>
        /// Generates the next sequential invoice number
        /// </summary>
        /// <returns>The generated invoice number</returns>
        Task<string> GenerateAsync();
    }
}
