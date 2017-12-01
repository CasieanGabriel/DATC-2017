import { Component, HostBinding, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
 import { moveIn } from '../../router.animations';

@Component({
  selector: 'login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  animations: [moveIn()],
  host: {'[@moveIn': ''}
})
export class LoginComponent implements OnInit {

  constructor(private auth: AuthService) {
  }

  
  ngOnInit(): void {
   
  }

  facebookLogin() {
    this.auth.loginWithFacebook();
  }

  googleLogin() {
    this.auth.loginWithGoogle();
  }

  logout() {
    this.auth.logout();
  }
  
}