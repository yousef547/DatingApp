import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  registerForm:FormGroup; 
  maxDate:Date;
  validationErrors:string[] =[];
  constructor(private accountService: AccountService,private fb:FormBuilder, private toastr: ToastrService
  ,private route:Router) { }

  ngOnInit(): void {
    this.initalizeForm(); 
    this.maxDate = new Date;
    this.maxDate.setFullYear(this.maxDate.getFullYear() -18);
  }


  initalizeForm(){
    this.registerForm = this.fb.group({
      gender:['male'],
      username:['',Validators.required],
      KnownAs:['',Validators.required],
      dateOfBirth:['',Validators.required],
      city:['',Validators.required],
      country:['',Validators.required],
      password:['',[Validators.required,Validators.minLength(4),Validators.maxLength(8)]],
      confirmPassword:['',[Validators.required,this.matchValues('password')]]
    })
    this.registerForm.controls.password.valueChanges.subscribe(()=>{
      this.registerForm.controls.confirmPassword.updateValueAndValidity();
    })
  }

matchValues(matchTo:string): ValidatorFn {
  return (control:AbstractControl)=>{
    return control?.value === control?.parent?.controls[matchTo].value
    ?null:{isMatching:true}
  }
}

  register(): void {
    this.accountService.register(this.registerForm.value).subscribe(response => {
      console.log(response);
      this.route.navigateByUrl('/members');
    }, error => {
      console.log(error);
      this.validationErrors = error;
    })
  }


  cancel() {
    this.cancelRegister.emit(false);
  }

}
