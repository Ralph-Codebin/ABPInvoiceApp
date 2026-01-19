import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CustomerDto } from '../../proxy/customers/models';
import { InvoiceDto } from '../../proxy/invoices/models';
import { CustomerService } from '../../proxy/customers/customer.service';
import { InvoiceService } from '../../proxy/invoices/invoice.service';

@Component({
  selector: 'app-customer-detail',
  templateUrl: './customer-detail.component.html',
  styleUrls: ['./customer-detail.component.scss'],
})
export class CustomerDetailComponent implements OnInit {
  customer: CustomerDto | null = null;
  invoices: InvoiceDto[] = [];
  loading = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private customerService: CustomerService,
    private invoiceService: InvoiceService
  ) {}

  ngOnInit() {
    const customerId = this.route.snapshot.paramMap.get('id');
    if (customerId) {
      this.loadCustomer(customerId);
      this.loadCustomerInvoices(customerId);
    }
  }

  loadCustomer(id: string) {
    this.loading = true;
    this.customerService.get(id).subscribe({
      next: (customer) => {
        this.customer = customer;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        this.router.navigate(['/customers']);
      },
    });
  }

  loadCustomerInvoices(customerId: string) {
    this.invoiceService.getByCustomerId(customerId).subscribe({
      next: (invoices) => {
        this.invoices = invoices;
      },
      error: (error) => {
        console.error('Error loading invoices:', error);
      },
    });
  }

  editCustomer() {
    if (this.customer) {
      this.router.navigate(['/customers'], {
        queryParams: { edit: this.customer.id }
      });
    }
  }

  viewInvoice(invoiceId: string) {
    this.router.navigate(['/invoices', invoiceId]);
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

  goBack() {
    this.router.navigate(['/customers']);
  }

  getTotalInvoiceAmount(): number {
    return this.invoices.reduce((sum, inv) => sum + inv.grandTotal, 0);
  }
}
