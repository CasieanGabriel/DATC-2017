import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { moveIn, fallIn } from '../../router.animations';
import { Router } from '@angular/router';

@Component({
  selector: 'register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
  // animations: [moveIn(), fallIn()],
  // host: {'[@moveIn]': ''}
})
export class RegisterComponent implements OnInit {

  state: string = '';
  error: any;

  constructor(private auth: AuthService, private router: Router) {
  }

  ngOnInit() {

  }

  onSubmit(formData) {
    if (formData.valid) {
      this.auth.createUser(formData.value.email, formData.value.password, formData.value.firstName + " " + formData.value.lastName).then(
        (success) => {
          this.router.navigate(['']);
        }).catch(
          (err) => {
            this.error = err;
          })
    }
  }
  
}