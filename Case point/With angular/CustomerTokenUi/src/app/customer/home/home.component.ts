import { Pagination } from './../../shared/models/pagination';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { PaginationResult } from '../../shared/models/pagination';
import { CustomerService } from '../customer.service';
import { TokenListModel } from '../../shared/models/token';
import { TokenHelper } from "../../shared/util/token";
import { Observable } from 'rxjs/internal/Observable';
import { Subscription, interval } from 'rxjs';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit,OnDestroy{
  
  pagination:PaginationResult<TokenListModel> = {
    totalCount: 100,
    pagesize:10,
    pageNumber:1,
    data:[]
  }

  intervaObservable = interval(2000);
  reloadgridSubscriber : Subscription|null = null

  constructor(
    private toastr: ToastrService,
    private customerService : CustomerService) {
    // this.toastr.error('everything is broken', 'Major Error');
  }

  ngOnDestroy(): void {
    this.unsubscribeReloadgrid();
  }

  ngOnInit(): void {
    this.loadGrid()    
  }

  unsubscribeReloadgrid(){
    if (this.reloadgridSubscriber) {
      this.reloadgridSubscriber.unsubscribe()
    }
  }

  subscribeReloadGrid(){
    this.reloadgridSubscriber= this.intervaObservable.subscribe(x=>{
      this.loadGrid();
    })
  }

  loadGrid()
  {  
    this.customerService.getUnResulvedTokenList(
      this.pagination.pageNumber,
      this.pagination.pagesize).subscribe(
        {
          next:response=>{
            this.pagination= response;
            if (!this.reloadgridSubscriber) {
              this.subscribeReloadGrid();
            }  
          }
        })
  }


  
  onPagerChanged($event: number) {
    this.pagination.pageNumber= $event;
    this.loadGrid();
  }

  getData(){
    return this.pagination.data
  }

  getQueryName(queryType:number)
  {
    return TokenHelper.getQueryName(queryType);
  }

  getStatusName(statusType:number)
  {
    return TokenHelper.getStatusName(statusType);
  }

  getStatusclass(statusType:number)
  {
    switch (statusType) {
      case 2:
        return "table-warning"
        break;
      case 3:
        return "table-success"
        break;
    }
    return "table-primary"
  }
}
