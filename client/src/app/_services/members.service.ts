import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';

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

  constructor(private http: HttpClient) { }
  getMembers() {
    if (this.members.length > 0) return of(this.members);
    return this.http.get<Member[]>(this.baseUrl + 'Users', httpOptions).pipe(
      map(members => {
        this.members = this.members;
        return members;
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
}

