import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ApiService } from '../services/api.service';
import { LoginComponent } from '../login/login.component';
@Component({
  selector: 'app-root',
  imports: [RouterOutlet, LoginComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit {
  constructor(private apiService: ApiService) {}

  public apiResponse: any;
  public count = 0;
  ngOnInit(): void {}

  OnButtonCLick() {
    // this.apiService.getData().subscribe({
    //   next: (res) => {
    //     console.log('Response', res);
    //     this.count++;
    //     this.apiResponse = res.message + '' + this.count;
    //   },
    //   error: (err) => console.log('Error:', err),
    // });
  }
  title = 'Frontend1';
}
