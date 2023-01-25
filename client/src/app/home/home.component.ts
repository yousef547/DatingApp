import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerModel = false;
  users: any;
  constructor() { }

  ngOnInit(): void {
  }

  registerToggle() {
    this.registerModel = !this.registerModel;
  }

  // getUser() {
  //   this.http.get('https://localhost:44334/api/Users').subscribe((users) => this.users = users)
  // }

  cancelRegisterMode(event: boolean) {
    this.registerModel = event;
  }

}
