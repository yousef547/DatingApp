import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of, pipe } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { PaginationResult } from '../_models/pagination';
import { UserParams } from '../_models/userParans';
import { AccountService } from './account.service';
import { User } from '../_models/user';
import { getPaginatedResult, getPaginationHeaders } from './PaginationHelper';

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
  memberCache = new Map();
  user: User;
  userParams : UserParams;
  

  constructor(private http: HttpClient,private accountServivce:AccountService) { 
    this.accountServivce.currentUser$.pipe(take(1)).subscribe(user=>{
      this.user = user;
      this.userParams = new UserParams(user);
    })
  }

  getUserParams(){
    return this.userParams;
  }

  setUserParams(params){
    this.userParams = params;
  }
  resetUserParams() {
    this.userParams = new UserParams(this.user);
    return this.userParams;
  }
  getMembers(userParams: UserParams) {
    var response = this.memberCache.get(Object.values(userParams).join('-'));
    console.log(response);
    if (response) {
      return of(response);
    }
    let params = getPaginationHeaders(userParams.pageNumber, userParams.pageSize);
    params = params.append('minAge', userParams.minAge.toString());
    params = params.append('maxAge', userParams.maxAge.toString());
    params = params.append('gender', userParams.gender);
    params = params.append('order', userParams.orderBy);

    return getPaginatedResult<Member[]>(this.baseUrl + 'Users', params,this.http)
      .pipe(map(response => {
        this.memberCache.set(Object.values(userParams).join('-'), response);
        return response
      }))
  }



  getMember(username: string) {
    const member = [...this.memberCache.values()]
    .reduce((arr, elem)=> arr.concat(elem.result),[])
    .find((member:Member) => member.userName == username);
    if(member){
      return of(member);
    }
    return this.http.get<Member>(this.baseUrl + `Users/${username}`, httpOptions);
  }

  updateMember(member: Member) {
    return this.http.put(this.baseUrl + 'Users', member, httpOptions).pipe(
      map(() => {
        const index = this.members.indexOf(member);
        this.members[index] = member;
      })
    );
  }

  setMainPhoto(photoId: Number) {
    return this.http.put(this.baseUrl + `Users/set-main-photo/${photoId}`, {}, httpOptions);
  }

  deletePhoto(photoId: number) {
    return this.http.delete(this.baseUrl + `Users/delete-photo/${photoId}`, httpOptions);
  }

  addLike(username:string){
    return this.http.post(this.baseUrl + 'likes/'+username,{},httpOptions);
  }

  getLikes(predicate:string,pageNumber,pageSize){
    let params = getPaginationHeaders(pageNumber,pageSize);
    params = params.append('predicate',predicate);
    return getPaginatedResult<Partial<Member[]>>(this.baseUrl + 'likes',params,this.http);
  }


}

