using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CustomerInvoice.Data;
using Volo.Abp.DependencyInjection;

namespace CustomerInvoice.EntityFrameworkCore;

public class EntityFrameworkCoreCustomerInvoiceDbSchemaMigrator
    : ICustomerInvoiceDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreCustomerInvoiceDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolve the CustomerInvoiceDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<CustomerInvoiceDbContext>()
            .Database
            .MigrateAsync();
    }
}
