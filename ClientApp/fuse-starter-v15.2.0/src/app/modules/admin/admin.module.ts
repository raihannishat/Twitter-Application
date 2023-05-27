import { NgModule } from '@angular/core';
import { AdminRoutingModule } from './admin-routing.module';
import { UserDetailComponent } from './user-detail/user-detail.component';
import { SharedModule } from 'app/shared/shared.module';



@NgModule({
  declarations: [
    UserDetailComponent
  ],
  imports: [
    SharedModule,
    AdminRoutingModule
  ],
})
export class AdminModule { }
