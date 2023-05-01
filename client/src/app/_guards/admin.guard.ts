import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {
  constructor(private toster:ToastrService,private acountService:AccountService){

  }
  canActivate(): Observable<boolean> {
    return this.acountService.currentUser$.pipe(
      map(user => {
        console.log(user);
        if(user.roles.includes('Admin') || user.roles.includes("Moderator")){
          return true;
        };
        this.toster.error('you cannot enter this area');
      })
    );
  }
  
}
