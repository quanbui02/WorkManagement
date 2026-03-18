import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ClientLoginComponent } from './client-login.component';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule.forChild([
      { path: '', component: ClientLoginComponent }
    ])
  ],
  declarations: [ClientLoginComponent]
})
export class ClientLoginModule { }
