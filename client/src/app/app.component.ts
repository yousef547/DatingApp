import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'The Dating App';
  users: any;

  /**
   *
   */
  constructor(private http: HttpClient) {

  }
  ngOnInit() {
    this.getUser();
  }

  getUser() {
    this.http.get('https://localhost:44334/api/Users').subscribe((res) => {
      this.users = res;
      console.log(res);
    }, error => {
      console.log(error);
    })
  }
}
