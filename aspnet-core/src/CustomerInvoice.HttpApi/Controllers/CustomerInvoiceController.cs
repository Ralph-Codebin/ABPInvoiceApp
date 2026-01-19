using CustomerInvoice.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace CustomerInvoice.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class CustomerInvoiceController : AbpControllerBase
{
    protected CustomerInvoiceController()
    {
        LocalizationResource = typeof(CustomerInvoiceResource);
    }
}
