import type { FullAuditedEntityDto } from '@abp/ng.core';

/**
 * Customer DTO for reading customer data
 */
export interface CustomerDto extends FullAuditedEntityDto<string> {
  name: string;
  email: string;
  phone?: string;
  billingAddress?: string;
}

/**
 * DTO for creating a new customer
 */
export interface CreateCustomerDto {
  name: string;
  email: string;
  phone?: string;
  billingAddress?: string;
}

/**
 * DTO for updating an existing customer
 */
export interface UpdateCustomerDto {
  name: string;
  email: string;
  phone?: string;
  billingAddress?: string;
}
