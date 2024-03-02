import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { EveluatorService } from '../eveluator.service';
import { NotExpr } from '@angular/compiler';
import { AuthService } from '../../core/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

  constructor(private authService: AuthService) {
    
  }

  loginForm = new FormGroup({
    username: new FormControl("",Validators.required),
    password: new FormControl('',Validators.required)
  });

  login(){
    if(!this.loginForm.invalid)  
    this.authService.login(
      this.loginForm.controls.username.value as string, 
      this.loginForm.controls.password.value as string);
  }
  
}
