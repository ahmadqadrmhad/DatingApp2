import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  @Output() cancleRegister = new EventEmitter();
  model :any ={} ;
  /**
   *
   */
  constructor( private accountservice : AccountService) {}
  ngOnInit(): void {
   
  }
 

  register(){
    this.accountservice.register(this.model).subscribe({
      next: () =>{      
        this.cancle();
      },
      error: error => console.log(error)
    });
  }


  cancle(){

    console.log("Cancelled");
    this.cancleRegister.emit(false);
  }

}
