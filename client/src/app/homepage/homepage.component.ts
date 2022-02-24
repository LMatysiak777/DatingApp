import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
 

@Component({
  selector: 'app-homepage',
  templateUrl: './homepage.component.html',
  styleUrls: ['./homepage.component.css']
})
export class HomepageComponent implements OnInit {
  registerMode = false; 
  users: any;

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
  //example of initializing method to fetch user DTO from API
    // this.getUsers()
  }

  // flag for toggling DOM generation of certain sections on hompage component
  registerToggle() {
    this.registerMode =!this.registerMode;
  }
  // example of method for fetching users DTO from API 
  // getUsers() {
  //   this.http.get("https://localhost:5001/api/users/").subscribe(users=> this.users = users)
  // }

  cancelRegisterMode(event: boolean) {
    this.registerMode = event; 
    
  }

}
