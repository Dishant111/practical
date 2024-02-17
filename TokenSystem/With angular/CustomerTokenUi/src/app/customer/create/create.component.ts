import { Component, ElementRef, ViewChild, inject } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TokenHelper } from '../../shared/util/token';
import { CustomerService } from '../customer.service';
import { ToastrService } from 'ngx-toastr';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-create',
  templateUrl: './create.component.html',
  styleUrl: './create.component.css'
})
export class CreateComponent {
  
  private modalService = inject(NgbModal);
  
  tokenModelForm = new FormGroup({
    queryType: new FormControl(1,Validators.required)
  });
  
  createdTokenNo : number = 0;
  
  @ViewChild("createdModel") createdModel!: ElementRef<any>;

  constructor(private customerService:CustomerService,private toastr: ToastrService,
    ) {
    
  }
  getAllQueryName()
  {
    return TokenHelper.getAllQueryName();
  }

  submitTokenModel() {
    if (!this.tokenModelForm.invalid) {
      this.customerService.CreateToken({
        query : this.tokenModelForm.controls.queryType.value as number
      }).subscribe({
        next : (response) => {
          if (response.id > 0) {
            this.toastr.success("Query has been raised")
            this.createdTokenNo =  response.id         
            this.modalService.open(this.createdModel) 
          }else{
            this.toastr.error("unable to raise query")
          }
        } 
      })
    }  
  }
}
