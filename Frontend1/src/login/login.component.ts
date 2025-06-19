import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { HomeComponent } from '../home/home.component';
@Component({
  selector: 'app-login',
  imports: [HomeComponent],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent implements OnInit {
  constructor(private http: HttpClient) {}

  islogin = false;

  ngOnInit() {
    this.checkLogin(); // ✅ Check token on init
  }

  login() {
    this.http
      .post('http://localhost:5000/api/auth/login', {
        Username: 'abhi',
        Password: 'abxxus',
      })
      .subscribe((res: any) => {
        console.log(res.token);
        localStorage.setItem('token', res.token);
        this.checkLogin();
      });
  }

  checkLogin() {
    this.islogin = localStorage.getItem('token') !== null;
  }

  Logout() {
    localStorage.removeItem('token');
    this.islogin = false;
    console.log('Logged out successfully');
  }
}
