import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from './app.component';

const routes: Routes = [
  {path:"customer",loadChildren : ()=> import('./customer/customer.module').then(m=>m.CustomerModule),pathMatch:'prefix'},
  {path:"eveluator",loadChildren : ()=> import('./eveluator/eveluator.module').then(m=>m.EveluatorModule),pathMatch: 'prefix' },
  {path: '**',redirectTo:'customer',pathMatch:'full'},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
