import { Component, OnInit } from '@angular/core';
import { ListService, PagedResultDto } from '@abp/ng.core';
import { Confirmation, ConfirmationService } from '@abp/ng.theme.shared';
import { InvoiceDto, InvoiceStatus, CreateLineItemDto } from '../../proxy/invoices/models';
import { InvoiceService } from '../../proxy/invoices/invoice.service';
import { CustomerService } from '../../proxy/customers/customer.service';
import { CustomerDto } from '../../proxy/customers/models';

@Component({
  selector: 'app-invoice-list',
  templateUrl: './invoice-list.component.html',
  styleUrls: ['./invoice-list.component.scss'],
  providers: [ListService],
})
export class InvoiceListComponent implements OnInit {
  invoices = { items: [], totalCount: 0 } as PagedResultDto<InvoiceDto>;
  customers: CustomerDto[] = [];

  isModalOpen = false;
  selectedInvoice = {} as InvoiceDto;

  invoiceStatuses = InvoiceStatus;
  statusKeys: number[] = [];

  constructor(
    public readonly list: ListService,
    private invoiceService: InvoiceService,
    private customerService: CustomerService,
    private confirmation: ConfirmationService
  ) {
    this.statusKeys = Object.keys(InvoiceStatus)
      .filter(key => !isNaN(Number(key)))
      .map(key => Number(key));
  }

  ngOnInit() {
    const invoiceStreamCreator = (query) => this.invoiceService.getList(query);

    this.list.hookToQuery(invoiceStreamCreator).subscribe((response) => {
      this.invoices = response;
    });

    // Load customers for dropdown
    this.customerService.getList({ skipCount: 0, maxResultCount: 1000 }).subscribe((result) => {
      this.customers = result.items;
    });
  }

  createInvoice() {
    this.selectedInvoice = {
      lineItems: [],
      taxAmount: 0,
    } as InvoiceDto;
    this.isModalOpen = true;
  }

  editInvoice(id: string) {
    this.invoiceService.get(id).subscribe((invoice) => {
      this.selectedInvoice = invoice;
      this.isModalOpen = true;
    });
  }

  save() {
    if (this.selectedInvoice.id) {
      // Update existing invoice
      this.invoiceService
        .update(this.selectedInvoice.id, {
          invoiceDate: this.selectedInvoice.invoiceDate,
          dueDate: this.selectedInvoice.dueDate,
          taxAmount: this.selectedInvoice.taxAmount,
        })
        .subscribe(() => {
          this.isModalOpen = false;
          this.list.get();
        });
    } else {
      // Create new invoice
      this.invoiceService
        .create({
          customerId: this.selectedInvoice.customerId,
          invoiceDate: this.selectedInvoice.invoiceDate,
          dueDate: this.selectedInvoice.dueDate,
          taxAmount: this.selectedInvoice.taxAmount || 0,
          lineItems: this.selectedInvoice.lineItems || [],
        })
        .subscribe(() => {
          this.isModalOpen = false;
          this.list.get();
        });
    }
  }

  delete(id: string) {
    this.confirmation.warn('::AreYouSure', '::AreYouSureToDelete').subscribe((status) => {
      if (status === Confirmation.Status.confirm) {
        this.invoiceService.delete(id).subscribe(() => this.list.get());
      }
    });
  }

  updateStatus(id: string, status: InvoiceStatus) {
    this.invoiceService.updateStatus(id, { status }).subscribe(() => {
      this.list.get();
    });
  }

  addLineItem() {
    if (!this.selectedInvoice.lineItems) {
      this.selectedInvoice.lineItems = [];
    }
    this.selectedInvoice.lineItems.push({
      description: '',
      quantity: 1,
      unitPrice: 0,
      total: 0,
    } as any);
  }

  removeLineItem(index: number) {
    this.selectedInvoice.lineItems.splice(index, 1);
  }

  calculateLineItemTotal(lineItem: any) {
    lineItem.total = lineItem.quantity * lineItem.unitPrice;
  }

  getStatusName(status: InvoiceStatus): string {
    return InvoiceStatus[status];
  }

  getStatusBadgeClass(status: InvoiceStatus): string {
    switch (status) {
      case InvoiceStatus.Draft:
        return 'bg-secondary';
      case InvoiceStatus.Pending:
        return 'bg-warning';
      case InvoiceStatus.Sent:
        return 'bg-info';
      case InvoiceStatus.Paid:
        return 'bg-success';
      case InvoiceStatus.Cancelled:
        return 'bg-danger';
      default:
        return 'bg-secondary';
    }
  }
}
