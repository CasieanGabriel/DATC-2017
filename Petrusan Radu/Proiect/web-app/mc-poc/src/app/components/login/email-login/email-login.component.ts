import { Component } from '@angular/core';
import { AuthService } from '../../../services/auth.service';
import { moveIn, fallIn } from '../../../router.animations';
import { Router } from '@angular/router';
import { FlashMessagesService } from 'angular2-flash-messages';

@Component({
  selector: 'email-login',
  templateUrl: './email-login.component.html',
  styleUrls: ['./email-login.component.css'],
  animations: [moveIn(), fallIn()],
  host: { '[@moveIn]': '' }
})
export class EmailLoginComponent {

  state: string = '';
  error: any;

  constructor(private auth: AuthService, private router: Router, private flashMessage: FlashMessagesService) {
    if (this.auth.loggedInUser()) {

    }
  }

  onSubmit(formData) {
    if (formData.valid) {
      console.log(formData.value);
      this.auth.loginWithEmail(formData.value.email, formData.value.password).then(
        (success) => {
          this.flashMessage.show('Logarea a avut loc cu succes!', {
            cssClass: 'alert-success',
            timeout: 3000
          });
          this.router.navigate(['']);
        }).catch(
        (err) => {
          this.error = err;
          this.flashMessage.show('A apărut o eroare!', {
            cssClass: 'alert-danger',
            timeout: 3000
          });
        })
    }
  }

}