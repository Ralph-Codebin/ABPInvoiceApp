import { Injectable } from '@angular/core';
import { RestService } from '@abp/ng.core';
import type { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import type {
  InvoiceDto,
  CreateInvoiceDto,
  UpdateInvoiceDto,
  UpdateInvoiceStatusDto,
  CreateLineItemDto,
  UpdateLineItemDto,
} from './models';

@Injectable({
  providedIn: 'root',
})
export class InvoiceService {
  apiName = 'Default';

  constructor(private restService: RestService) {}

  /**
   * Get a paginated list of invoices
   */
  getList(input: PagedAndSortedResultRequestDto) {
    return this.restService.request<any, PagedResultDto<InvoiceDto>>(
      {
        method: 'GET',
        url: '/api/app/invoice',
        params: {
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
          sorting: input.sorting,
        },
      },
      { apiName: this.apiName }
    );
  }

  /**
   * Get an invoice by ID
   */
  get(id: string) {
    return this.restService.request<any, InvoiceDto>(
      {
        method: 'GET',
        url: `/api/app/invoice/${id}`,
      },
      { apiName: this.apiName }
    );
  }

  /**
   * Get all invoices for a specific customer
   */
  getByCustomerId(customerId: string) {
    return this.restService.request<any, InvoiceDto[]>(
      {
        method: 'GET',
        url: `/api/app/invoice/by-customer/${customerId}`,
      },
      { apiName: this.apiName }
    );
  }

  /**
   * Create a new invoice
   */
  create(input: CreateInvoiceDto) {
    return this.restService.request<any, InvoiceDto>(
      {
        method: 'POST',
        url: '/api/app/invoice',
        body: input,
      },
      { apiName: this.apiName }
    );
  }

  /**
   * Update an existing invoice
   */
  update(id: string, input: UpdateInvoiceDto) {
    return this.restService.request<any, InvoiceDto>(
      {
        method: 'PUT',
        url: `/api/app/invoice/${id}`,
        body: input,
      },
      { apiName: this.apiName }
    );
  }

  /**
   * Update invoice status
   */
  updateStatus(id: string, input: UpdateInvoiceStatusDto) {
    return this.restService.request<any, InvoiceDto>(
      {
        method: 'PUT',
        url: `/api/app/invoice/${id}/status`,
        body: input,
      },
      { apiName: this.apiName }
    );
  }

  /**
   * Delete an invoice
   */
  delete(id: string) {
    return this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/invoice/${id}`,
      },
      { apiName: this.apiName }
    );
  }

  /**
   * Add a line item to an invoice
   */
  addLineItem(invoiceId: string, input: CreateLineItemDto) {
    return this.restService.request<any, InvoiceDto>(
      {
        method: 'POST',
        url: `/api/app/invoice/${invoiceId}/line-item`,
        body: input,
      },
      { apiName: this.apiName }
    );
  }

  /**
   * Update a line item on an invoice
   */
  updateLineItem(invoiceId: string, lineItemId: string, input: UpdateLineItemDto) {
    return this.restService.request<any, InvoiceDto>(
      {
        method: 'PUT',
        url: `/api/app/invoice/${invoiceId}/line-item/${lineItemId}`,
        body: input,
      },
      { apiName: this.apiName }
    );
  }

  /**
   * Remove a line item from an invoice
   */
  removeLineItem(invoiceId: string, lineItemId: string) {
    return this.restService.request<any, InvoiceDto>(
      {
        method: 'DELETE',
        url: `/api/app/invoice/${invoiceId}/line-item/${lineItemId}`,
      },
      { apiName: this.apiName }
    );
  }
}
