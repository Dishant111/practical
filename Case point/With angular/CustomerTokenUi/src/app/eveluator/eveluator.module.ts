import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { EveluatorRoutingModule } from './eveluator-routing.module';
import { HomeComponent } from './home/home.component';
import { SharedModule } from '../shared/shared.module';
import { ReactiveFormsModule } from '@angular/forms';
import { CreateComponent } from '../customer/create/create.component';


@NgModule({
  declarations: [
    HomeComponent
  ],
  imports: [
    CommonModule,
    EveluatorRoutingModule,
    SharedModule,
    ReactiveFormsModule
  ]
})
export class EveluatorModule { }
