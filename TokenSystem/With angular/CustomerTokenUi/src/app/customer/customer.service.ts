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
   return this.httpclient.get<PaginationResult<TokenListModel>>(this.baseUrl+`Token/unresolved/list?pageSize=${pageSize}&pageNo=${pageNumber}`,{ withCredentials: true })
  }

  processToken(tokenId:number){
    return this.httpclient.put<any>(this.baseUrl+`Token/Process/${tokenId}`,null,{ withCredentials: true });
  } 

  pendingToken(tokenId:number){
    return this.httpclient.put<any>(this.baseUrl+`Token/Pending/${tokenId}`,null,{ withCredentials: true });
   }

   getTokenById(tokenId:number){
    return this.httpclient.get<Token>(this.baseUrl+`Token/Token/${tokenId}`,{ withCredentials: true })
   }

   ResolveToken(token:ResolveToken){
    return this.httpclient.post(this.baseUrl+`Token/Resolve`,token,{ withCredentials: true })
   }

   CreateToken(token:TokenCreateModel){
    return this.httpclient.post<Token>(this.baseUrl+`Token/Create`,token,{ withCredentials: true })
   }
}
