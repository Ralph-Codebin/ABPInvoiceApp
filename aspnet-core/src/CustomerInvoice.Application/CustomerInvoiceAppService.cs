using System;
using System.Collections.Generic;
using System.Text;
using CustomerInvoice.Localization;
using Volo.Abp.Application.Services;

namespace CustomerInvoice;

/* Inherit your application services from this class.
 */
public abstract class CustomerInvoiceAppService : ApplicationService
{
    protected CustomerInvoiceAppService()
    {
        LocalizationResource = typeof(CustomerInvoiceResource);
    }
}
