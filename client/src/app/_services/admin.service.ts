import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';

const httpOptions = {
  headers: new HttpHeaders({
    Authorization: 'Bearer ' + JSON.parse(localStorage.getItem('user'))?.token
  })
}
@Injectable({
  providedIn: 'root'
})
export class AdminService {
base = environment.apiUrl;
  constructor(private http:HttpClient) { }

  getUserWithRoles(){
    return this.http.get<Partial<User[]>>(this.base + "admin/users-with-roles",httpOptions);
  }

  updateUserRoles(username:string,roles:string[]){
    return this.http.post(this.base + 'admin/edit-roles/' + username + '?roles=' + roles,{},httpOptions)
  }


}
