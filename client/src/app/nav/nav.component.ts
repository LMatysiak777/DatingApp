import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
 model: any ={} 
 currentUser$: Observable<User>
 //using flag to disable/enable elements basing on login status
 //declared boolean = false on startup
//  loggedIn: boolean
 //injecting service into component
  constructor(public accountService: AccountService, private router: Router, private toastr: ToastrService ) { }

  ngOnInit(): void {
    // this.getCurrentUser();
    // this.currentUser$ = this.accountService.currentUser$}
  }
  login() {
    //sending component model to service method and returning Observable
    //Observable must be subscribed() to...
    this.accountService.login(this.model).subscribe(response => {
    //user DTO will be returned in response
    this.router.navigateByUrl('/members ')
    // this.loggedIn = true
    })
    console.log(this.model)
  }

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/');
    // this.loggedIn = false;
  }

  // getCurrentUser() {
  //   // $observable reference
  //   this.accountService.currentUser$.subscribe(user => {
  //   // !!conversion to boolean
  //     this.loggedIn = !!user;
  //   }, error => {
  //     console.log("ERROR !:::");
  //     console.log(error);
  //   })
  // }
}
