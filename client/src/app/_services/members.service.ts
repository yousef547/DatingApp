import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { PaginationResult } from '../_models/pagination';

const httpOptions = {
  headers: new HttpHeaders({
    Authorization: 'Bearer ' + JSON.parse(localStorage.getItem('user'))?.token
  })
}

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;
  members: Member[] = [];
  paginationResult:PaginationResult<Member[]> = new PaginationResult<Member[]>();

  constructor(private http: HttpClient) { }
  getMembers(page?:number,itemsPerPage?:number) {
    let params = new HttpParams();
    if(page !== null && itemsPerPage !== null){
      params = params.append('pageNumber',page.toString());
      params = params.append('pageSize',itemsPerPage.toString());
    }
    return this.http.get<Member[]>(this.baseUrl + 'Users',{
      headers: httpOptions.headers,
      observe:'response',params
    }).pipe(
      map(response => {
       this.paginationResult.result = response.body;
       if(response.headers.get('Pagination') != null){
          this.paginationResult.pagination = JSON.parse(response.headers.get('Pagination'))
       }
       return this.paginationResult;
      })
    );
  }

  getMember(username: string) {
    const member = this.members.find(x=>x.userName == username);
    if(member !== undefined) return of(member);
    return this.http.get<Member>(this.baseUrl + `Users/${username}`, httpOptions);
  }

  updateMember(member: Member) {
    return this.http.put(this.baseUrl + 'Users', member, httpOptions).pipe(
      map(()=>{
        const index = this.members.indexOf(member);
        this.members[index] = member;
      })
    );

   
  }

  setMainPhoto(photoId:Number){
    return this.http.put(this.baseUrl + `Users/set-main-photo/${photoId}`,{},httpOptions); 
  }
  deletePhoto(photoId:number) {
    return this.http.delete(this.baseUrl + `Users/delete-photo/${photoId}`,httpOptions);
  }
}

