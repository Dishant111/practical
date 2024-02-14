import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Pagination } from '../models/pagination';

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrl: './pagination.component.css'
})
export class PaginationComponent implements OnInit  {
  
  @Input() pagination : Pagination = 
  {
    totalCount: 0,
    pagesize:0,
    pageNumber:0,
  }
  totalpages: number=0;

  @Output() pageChanged = new EventEmitter<number>(); 

  ngOnInit(): void {
    console.log("init called");
  }
  
  getTotalPages(){   
    this.totalpages = Math.ceil(this.pagination.totalCount/this.pagination.pagesize);
    return this.totalpages;
  }
  loadGrid(pageno: number) {
     this.pageChanged.emit(pageno)    
  }

  getPaginationnumbers()
  {
    let result = Array.from([...Array(5).keys()].map(i => this.pagination.pageNumber + i - 2));
    return result; 
  }
}
