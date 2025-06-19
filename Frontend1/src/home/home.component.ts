import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { error } from 'node:console';

@Component({
  selector: 'app-home',
  imports: [],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export class HomeComponent implements OnInit {
  constructor(private http: HttpClient) {}
  apiResponse = 'none';

  ngOnInit() {}

  Getdata() {
    const headers = new HttpHeaders({
      Authorization: 'Bearer ' + localStorage.getItem('token'),
    });
    this.http
      .get('http://localhost:5000/api/test/secure', { headers })
      .subscribe(
        (res: any) => {
          console.log(res);
          this.apiResponse = res.message;
        },
        (error) => {
          console.error(error);
          this.apiResponse = 'none';
        }
      );
  }
}
