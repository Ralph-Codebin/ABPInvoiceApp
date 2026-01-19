import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { InvoiceRoutingModule } from './invoice-routing.module';
import { InvoiceListComponent } from './invoice-list/invoice-list.component';
import { InvoiceDetailComponent } from './invoice-detail/invoice-detail.component';

@NgModule({
  declarations: [InvoiceListComponent, InvoiceDetailComponent],
  imports: [SharedModule, InvoiceRoutingModule],
})
export class InvoiceModule {}
