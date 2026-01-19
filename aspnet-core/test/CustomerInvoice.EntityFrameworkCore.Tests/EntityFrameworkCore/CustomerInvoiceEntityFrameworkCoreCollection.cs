using Xunit;

namespace CustomerInvoice.EntityFrameworkCore;

[CollectionDefinition(CustomerInvoiceTestConsts.CollectionDefinitionName)]
public class CustomerInvoiceEntityFrameworkCoreCollection : ICollectionFixture<CustomerInvoiceEntityFrameworkCoreFixture>
{

}
