import { environment } from './../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

 baseUrl : string =  environment.apiUrl
authUser : User|null= null;

  constructor(private httpClient:HttpClient ,private router : Router) { }

  public get User() : User | null {
    return this.authUser;
  }
  
  public initializeUser() {
    this.httpClient.get<User>(`${this.baseUrl}Employee/user`)
    .subscribe({
      next: resposne => {
        this.authUser = resposne;
        this.router.navigateByUrl("/eveluator");
      }
    })
  }

  public reLogin() {
    this.authUser = null; 
    this.router.navigateByUrl("/eveluator/login");
  }

 public login(username:string,password:string){
    const model = {
      userName : username,
      password : password
    }

    return this.httpClient.post(this.baseUrl+`Employee/login`,model)
    .subscribe({
      next : (response) => {
         this.initializeUser();
      }
    })
   }

}

interface User{
  id:number,
  userName:string,
  queryId:number
}