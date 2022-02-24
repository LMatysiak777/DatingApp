import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';
 

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  // example of Decorator used for parent to child communication
  // @Input() usersFromHomeComponent: any; 
  @Output() cancelRegister = new EventEmitter();
  model: any = {};
  constructor(private accountService: AccountService, private toastr: ToastrService) { }

  ngOnInit(): void {
  }

  register(){
    console.log(this.model)
    this.accountService.register(this.model).subscribe(response => {
      console.log(response);
      this.toastr.success("Register OK");
      this.cancel();}, error => {
        console.log("ERROR:::");
        console.log(error);
        this.toastr.error(error.name+": "+error.error)
        // other toastr methods available:
        // this.toastr.warning("warrning");
        // this.toastr.info("info");
        // this.toastr.success("success")
      })
    }
  

  cancel() {
    this.cancelRegister.emit(false);
  }

}
