import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { PaginationResult } from '../shared/models/pagination';
import { TokenListModel } from '../shared/models/token';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class EveluatorService {
  baseUrl = environment.apiUrl;
  httpPostOptions =
  {   
      headers:
          new HttpHeaders (
          {   
              "Content-Type": "application/json"
          }),
      withCredentials: true,
  }
  constructor(private httpclient:HttpClient) { }

  getUnResulvedTokenList(pageNumber:number,pageSize:number){
    
    return this.httpclient.get<PaginationResult<TokenListModel>>(this.baseUrl+`Employee/token/list?pageSize=${pageSize}&pageNo=${pageNumber}`)
   }


}
