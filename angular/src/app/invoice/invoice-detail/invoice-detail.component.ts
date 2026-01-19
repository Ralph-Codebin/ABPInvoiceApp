import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Confirmation, ConfirmationService } from '@abp/ng.theme.shared';
import { ToasterService } from '@abp/ng.theme.shared';
import { InvoiceDto, LineItemDto, UpdateInvoiceStatusDto } from '../../proxy/invoices/models';
import { InvoiceService } from '../../proxy/invoices/invoice.service';
import { CreateLineItemDto, UpdateLineItemDto } from '../../proxy/invoices/models';

@Component({
  selector: 'app-invoice-detail',
  templateUrl: './invoice-detail.component.html',
  styleUrls: ['./invoice-detail.component.scss'],
})
export class InvoiceDetailComponent implements OnInit {
  invoice: InvoiceDto | null = null;
  loading = false;

  // Line item management
  isAddingLineItem = false;
  isEditingLineItem = false;
  editingLineItemId: string | null = null;
  newLineItem: CreateLineItemDto = {
    description: '',
    quantity: 1,
    unitPrice: 0,
  };
  editLineItem: UpdateLineItemDto = {
    description: '',
    quantity: 1,
    unitPrice: 0,
  };

  // Status update
  isUpdatingStatus = false;
  newStatus: number | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private invoiceService: InvoiceService,
    private confirmation: ConfirmationService,
    private toaster: ToasterService
  ) {}

  ngOnInit() {
    const invoiceId = this.route.snapshot.paramMap.get('id');
    if (invoiceId) {
      this.loadInvoice(invoiceId);
    }
  }

  loadInvoice(id: string) {
    this.loading = true;
    this.invoiceService.get(id).subscribe({
      next: (invoice) => {
        this.invoice = invoice;
        this.newStatus = invoice.status;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        this.router.navigate(['/invoices']);
      },
    });
  }

  getStatusBadgeClass(status: number): string {
    switch (status) {
      case 0: return 'badge bg-secondary'; // Draft
      case 1: return 'badge bg-warning'; // Pending
      case 2: return 'badge bg-info'; // Sent
      case 3: return 'badge bg-success'; // Paid
      case 4: return 'badge bg-danger'; // Cancelled
      default: return 'badge bg-secondary';
    }
  }

  getStatusName(status: number): string {
    switch (status) {
      case 0: return 'Draft';
      case 1: return 'Pending';
      case 2: return 'Sent';
      case 3: return 'Paid';
      case 4: return 'Cancelled';
      default: return 'Unknown';
    }
  }

  canEdit(): boolean {
    return this.invoice ? this.invoice.status !== 3 && this.invoice.status !== 4 : false;
  }

  editInvoice() {
    if (this.invoice) {
      this.router.navigate(['/invoices'], {
        queryParams: { edit: this.invoice.id }
      });
    }
  }

  viewCustomer() {
    if (this.invoice?.customerId) {
      this.router.navigate(['/customers', this.invoice.customerId]);
    }
  }

  // Line Item Management
  showAddLineItem() {
    this.isAddingLineItem = true;
    this.newLineItem = {
      description: '',
      quantity: 1,
      unitPrice: 0,
    };
  }

  cancelAddLineItem() {
    this.isAddingLineItem = false;
  }

  saveNewLineItem() {
    if (!this.invoice || !this.newLineItem.description) return;

    this.invoiceService.addLineItem(this.invoice.id, this.newLineItem).subscribe({
      next: () => {
        this.toaster.success('::LineItemAdded');
        this.isAddingLineItem = false;
        this.loadInvoice(this.invoice!.id);
      },
      error: (error) => {
        this.toaster.error('::ErrorAddingLineItem');
        console.error('Error adding line item:', error);
      },
    });
  }

  startEditLineItem(lineItem: LineItemDto) {
    this.isEditingLineItem = true;
    this.editingLineItemId = lineItem.id;
    this.editLineItem = {
      description: lineItem.description,
      quantity: lineItem.quantity,
      unitPrice: lineItem.unitPrice,
    };
  }

  cancelEditLineItem() {
    this.isEditingLineItem = false;
    this.editingLineItemId = null;
  }

  saveEditLineItem() {
    if (!this.invoice || !this.editingLineItemId) return;

    this.invoiceService
      .updateLineItem(this.invoice.id, this.editingLineItemId, this.editLineItem)
      .subscribe({
        next: () => {
          this.toaster.success('::LineItemUpdated');
          this.isEditingLineItem = false;
          this.editingLineItemId = null;
          this.loadInvoice(this.invoice!.id);
        },
        error: (error) => {
          this.toaster.error('::ErrorUpdatingLineItem');
          console.error('Error updating line item:', error);
        },
      });
  }

  deleteLineItem(lineItemId: string) {
    if (!this.invoice) return;

    this.confirmation.warn('::AreYouSure', '::AreYouSureToDelete').subscribe((status) => {
      if (status === Confirmation.Status.confirm) {
        this.invoiceService.removeLineItem(this.invoice!.id, lineItemId).subscribe({
          next: () => {
            this.toaster.success('::LineItemDeleted');
            this.loadInvoice(this.invoice!.id);
          },
          error: (error) => {
            this.toaster.error('::ErrorDeletingLineItem');
            console.error('Error deleting line item:', error);
          },
        });
      }
    });
  }

  calculateLineItemTotal(quantity: number, unitPrice: number): number {
    return quantity * unitPrice;
  }

  // Status Management
  updateStatus() {
    if (!this.invoice || this.newStatus === null || this.newStatus === this.invoice.status) return;

    this.isUpdatingStatus = true;
    const statusDto: UpdateInvoiceStatusDto = { status: this.newStatus };

    this.invoiceService.updateStatus(this.invoice.id, statusDto).subscribe({
      next: () => {
        this.toaster.success('::InvoiceStatusUpdated');
        this.isUpdatingStatus = false;
        this.loadInvoice(this.invoice!.id);
      },
      error: (error) => {
        this.toaster.error('::ErrorUpdatingStatus');
        this.isUpdatingStatus = false;
        console.error('Error updating status:', error);
      },
    });
  }

  goBack() {
    this.router.navigate(['/invoices']);
  }
}
