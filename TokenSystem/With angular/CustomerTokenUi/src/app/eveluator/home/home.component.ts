import { Component, ElementRef, ViewChild, inject } from '@angular/core';
import { PaginationResult } from '../../shared/models/pagination';
import { TokenListModel } from '../../shared/models/token';
import { Subscription, interval } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { CustomerService } from '../../customer/customer.service';
import { TokenHelper } from '../../shared/util/token';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FormControl, FormGroup, Validators,ReactiveFormsModule } from '@angular/forms';
import { EveluatorService } from '../eveluator.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {

  private modalService = inject(NgbModal);
  isModelLoading : boolean = false;
  
  tokenModelForm = new FormGroup({
    id:new  FormControl(0),
    queryType: new FormControl(1,Validators.required),
    address: new FormControl('',Validators.required),
    phone: new FormControl('',Validators.required),
  });

  pagination:PaginationResult<TokenListModel> = {
    totalCount: 100,
    pagesize:10,
    pageNumber:1,
    data:[]
  }
  
  @ViewChild("resolvModel") resolveModel!: ElementRef<any>;

  intervaObservable = interval(2000);
  reloadgridSubscriber : Subscription|null = null

  constructor(
    private toastr: ToastrService,
    private customerService : CustomerService,
    private eveluatorService:EveluatorService) {
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
    this.eveluatorService.getUnResulvedTokenList(
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

  setTokenToProcess(arg0: number) {
    this.customerService.processToken(arg0).subscribe({
      next:(response) =>{console.log(response);this.loadGrid(); } 
    });
  }

  setTokenToPending(arg0: number) {
    this.customerService.pendingToken(arg0).subscribe({
      next:(response) =>{console.log(response);this.loadGrid();} 
    });
  }

  resolveToken(tokenId: number) {
    this.unsubscribeReloadgrid()

      this.isModelLoading =true
      this.modalService.open(this.resolveModel, { scrollable: true });
      
      this.customerService.getTokenById(tokenId).subscribe({
        next:(response) => {
          console.log(response);
          
          this.tokenModelForm.patchValue({
            id:response.id,
            address:response.address,
            phone:response.phone,
            queryType:response.queryId
          })

          this.tokenModelForm.controls.queryType.disable()

          this.isModelLoading =false
        },
        error:response => {
          this.isModelLoading =true
          this.toastr.error("Something went worng please try again later")
          this.modalService.dismissAll()
        }
      })
    }
  
  submitTokenModel() {
    if (!this.tokenModelForm.invalid) {
      this.customerService.ResolveToken(
        {
        id :  this.tokenModelForm.controls.id.value as number,
        address:this.tokenModelForm.controls.address.value as string,
        phoneNumber: this.tokenModelForm.controls.phone.value as string,
        query : this.tokenModelForm.controls.queryType.value as number
      }).subscribe({
          next: (response) => {
            this.toastr.success("Query is resolved")
            this.tokenModelForm.reset()
            this.modalService.dismissAll();
            this.loadGrid()
          } ,
          error:response => {
            this.isModelLoading =true
            this.toastr.error("Something went worng please try again later")
            this.modalService.dismissAll()
          }
        })
    }    
  }

  getData(){
    return this.pagination.data
  }

  getAllQueryName()
  {
    return TokenHelper.getAllQueryName();
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
