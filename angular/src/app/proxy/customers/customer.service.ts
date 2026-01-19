import { Injectable } from '@angular/core';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import type { CustomerDto, CreateCustomerDto, UpdateCustomerDto } from './models';

@Injectable({
  providedIn: 'root',
})
export class CustomerService {
  apiName = 'Default';

  constructor(private restService: RestService) {}

  /**
   * Get a paginated list of customers
   */
  getList(input: PagedAndSortedResultRequestDto) {
    return this.restService.request<any, PagedResultDto<CustomerDto>>(
      {
        method: 'GET',
        url: '/api/app/customer',
        params: {
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
          sorting: input.sorting
        },
      },
      { apiName: this.apiName }
    );
  }

  /**
   * Get a customer by ID
   */
  get(id: string) {
    return this.restService.request<any, CustomerDto>(
      {
        method: 'GET',
        url: `/api/app/customer/${id}`,
      },
      { apiName: this.apiName }
    );
  }

  /**
   * Get a customer by email address
   */
  getByEmail(email: string) {
    return this.restService.request<any, CustomerDto>(
      {
        method: 'GET',
        url: `/api/app/customer/by-email/${email}`,
      },
      { apiName: this.apiName }
    );
  }

  /**
   * Search customers by name or email
   */
  search(searchTerm: string) {
    return this.restService.request<any, CustomerDto[]>(
      {
        method: 'GET',
        url: '/api/app/customer/search',
        params: { searchTerm },
      },
      { apiName: this.apiName }
    );
  }

  /**
   * Create a new customer
   */
  create(input: CreateCustomerDto) {
    return this.restService.request<any, CustomerDto>(
      {
        method: 'POST',
        url: '/api/app/customer',
        body: input,
      },
      { apiName: this.apiName }
    );
  }

  /**
   * Update an existing customer
   */
  update(id: string, input: UpdateCustomerDto) {
    return this.restService.request<any, CustomerDto>(
      {
        method: 'PUT',
        url: `/api/app/customer/${id}`,
        body: input,
      },
      { apiName: this.apiName }
    );
  }

  /**
   * Delete a customer
   */
  delete(id: string) {
    return this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/customer/${id}`,
      },
      { apiName: this.apiName }
    );
  }
}
