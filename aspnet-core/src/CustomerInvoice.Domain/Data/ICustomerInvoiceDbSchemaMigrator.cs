using System.Threading.Tasks;

namespace CustomerInvoice.Data;

public interface ICustomerInvoiceDbSchemaMigrator
{
    Task MigrateAsync();
}
