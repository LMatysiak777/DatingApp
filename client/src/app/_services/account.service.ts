import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, ReplaySubject } from 'rxjs';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})

// service is singleton will stay up with app runtime for example data like baseUrl
// in difference to components which are destroyed when not used
export class AccountService {
// this service provides access to api
baseUrl = "https://localhost:5001/api/";
private currentUserSource = new ReplaySubject<User>(1);
currentUser$ = this.currentUserSource.asObservable();

// inject http client into account service
  constructor(private http: HttpClient) { }

  login(model: any){
    console.log("login from service")
    return this.http.post(this.baseUrl+"account/login", model).pipe(
      map((response: User)=> {
        const user = response;
        if (user) {
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserSource.next(user);

        }
      })
      );
  }

  setCurrentUser(user: User) {
    this.currentUserSource.next(user);
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }
}
