import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { CustomerRoutingModule } from './customer-routing.module';
import { CustomerListComponent } from './customer-list/customer-list.component';
import { CustomerDetailComponent } from './customer-detail/customer-detail.component';

@NgModule({
  declarations: [CustomerListComponent, CustomerDetailComponent],
  imports: [SharedModule, CustomerRoutingModule],
})
export class CustomerModule {}
