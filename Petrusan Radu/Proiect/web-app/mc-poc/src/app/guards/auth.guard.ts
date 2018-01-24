import { Injectable } from "@angular/core";
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from "@angular/router";
import { Observable } from "rxjs/Observable";
import { AngularFireAuth } from "angularfire2/auth";
import 'rxjs/add/operator/take';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/map';

@Injectable()
export class AuthGuard implements CanActivate {
    constructor(
      private afAuth: AngularFireAuth,
      private router: Router
    ) {}
  
    canActivate(): Observable<boolean> {
      return this.afAuth.authState
        .take(1)
        .map(authState => !!authState)
        .do(auth => !auth ? this.router.navigate(['/login']) : true);
    }
  }