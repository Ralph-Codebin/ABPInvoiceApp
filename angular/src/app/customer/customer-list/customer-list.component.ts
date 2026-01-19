import { Component, OnInit } from '@angular/core';
import { ListService, PagedResultDto } from '@abp/ng.core';
import { Confirmation, ConfirmationService } from '@abp/ng.theme.shared';
import { CustomerDto } from '../../proxy/customers/models';
import { CustomerService } from '../../proxy/customers/customer.service';

@Component({
  selector: 'app-customer-list',
  templateUrl: './customer-list.component.html',
  styleUrls: ['./customer-list.component.scss'],
  providers: [ListService],
})
export class CustomerListComponent implements OnInit {
  customers = { items: [], totalCount: 0 } as PagedResultDto<CustomerDto>;

  isModalOpen = false;
  selectedCustomer = {} as CustomerDto;

  constructor(
    public readonly list: ListService,
    private customerService: CustomerService,
    private confirmation: ConfirmationService
  ) {}

  ngOnInit() {
    const customerStreamCreator = (query) => this.customerService.getList(query);

    this.list.hookToQuery(customerStreamCreator).subscribe((response) => {
      this.customers = response;
    });
  }

  createCustomer() {
    this.selectedCustomer = {} as CustomerDto;
    this.isModalOpen = true;
  }

  editCustomer(id: string) {
    this.customerService.get(id).subscribe((customer) => {
      this.selectedCustomer = customer;
      this.isModalOpen = true;
    });
  }

  save() {
    if (this.selectedCustomer.id) {
      this.customerService
        .update(this.selectedCustomer.id, this.selectedCustomer)
        .subscribe(() => {
          this.isModalOpen = false;
          this.list.get();
        });
    } else {
      this.customerService.create(this.selectedCustomer).subscribe(() => {
        this.isModalOpen = false;
        this.list.get();
      });
    }
  }

  delete(id: string) {
    this.confirmation.warn('::AreYouSure', '::AreYouSureToDelete').subscribe((status) => {
      if (status === Confirmation.Status.confirm) {
        this.customerService.delete(id).subscribe(() => this.list.get());
      }
    });
  }
}
