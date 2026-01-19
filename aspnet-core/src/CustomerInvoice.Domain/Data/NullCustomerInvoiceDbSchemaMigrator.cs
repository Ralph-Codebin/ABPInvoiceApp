using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace CustomerInvoice.Data;

/* This is used if database provider does't define
 * ICustomerInvoiceDbSchemaMigrator implementation.
 */
public class NullCustomerInvoiceDbSchemaMigrator : ICustomerInvoiceDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
