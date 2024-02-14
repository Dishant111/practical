import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { ResolveToken, Token, TokenCreateModel, TokenListModel } from '../shared/models/token';
import { PaginationResult } from '../shared/models/pagination';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {
  baseUrl = environment.apiUrl;
  
  constructor(private httpclient:HttpClient) { }

  getUnResulvedTokenList(pageNumber:number,pageSize:number){
   return this.httpclient.get<PaginationResult<TokenListModel>>(this.baseUrl+`Token/unresolved/list?pageSize=${pageSize}&pageNo=${pageNumber}`)
  }

  processToken(tokenId:number){
    return this.httpclient.put<any>(this.baseUrl+`Token/Process/${tokenId}`,null);
  } 

  pendingToken(tokenId:number){
    return this.httpclient.put<any>(this.baseUrl+`Token/Pending/${tokenId}`,null);
   }

   getTokenById(tokenId:number){
    return this.httpclient.get<Token>(this.baseUrl+`Token/Token/${tokenId}`)
   }

   ResolveToken(token:ResolveToken){
    return this.httpclient.post(this.baseUrl+`Token/Resolve`,token)
   }

   CreateToken(token:TokenCreateModel){
    return this.httpclient.post<Token>(this.baseUrl+`Token/Create`,token)
   }
}
