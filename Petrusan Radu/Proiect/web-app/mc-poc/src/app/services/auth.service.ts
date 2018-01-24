import { Injectable } from "@angular/core";
import { Http } from "@angular/http";
import { AngularFireAuth } from "angularfire2/auth";
import * as firebase from 'firebase/app';
import { FlashMessagesService } from 'angular2-flash-messages';
import { Router } from "@angular/router";

@Injectable()
export class AuthService {

    user: firebase.User = null;

    constructor(private http: Http, public afAuth: AngularFireAuth, public flashMessage: FlashMessagesService, private router: Router) {
        this.init();
    }

    private init(): void {
        this.afAuth.authState.subscribe(r => {
            this.user = r;
        })
    }

    loginWithFacebook() {
        this.afAuth.auth.signInWithPopup(new firebase.auth.FacebookAuthProvider).then((resp) => {
            //this.user = this.afAuth.authState.take(1);
            this.flashMessage.show('Logarea a avut loc cu succes!', {
                cssClass: 'alert-success', 
                timeout: 3000});
                this.router.navigate(['']);
        }, 
    (err) => {
        this.flashMessage.show('A apărut o eroare!',{
            cssClass: 'alert-danger',
            timeout: 3000
          });
    });
    }

    loginWithGoogle() {
        this.afAuth.auth.signInWithPopup(new firebase.auth.GoogleAuthProvider).then(r => {
            this.flashMessage.show('Logarea a avut loc cu succes!', {
                cssClass: 'alert-success', 
                timeout: 3000});
                this.router.navigate(['']);
        }, 
        (err) => {
            this.flashMessage.show('A apărut o eroare!',{
                cssClass: 'alert-danger',
                timeout: 3000
              });
        });
    }

    logout() {
        this.afAuth.auth.signOut().then(r => {
            this.flashMessage.show('Ați fost deconectat cu succes!',{
                cssClass: 'alert-success',
                timeout: 3000
              });
        },
    (err) => {
        this.flashMessage.show('A apărut o eroare!',{
            cssClass: 'alert-danger',
            timeout: 3000
          });
    });
    }

    loginWithEmail(email: string, password: string) {
        return this.afAuth.auth.signInWithEmailAndPassword(email, password);
    }

    loggedInUser(): string {
        this.afAuth.authState.subscribe(u => {
            if (u !== null)
                return u.uid;

            return null;
        });

        return null;
    }        

    createUser(email: string, password: string, name: string) {
        return this.afAuth.auth.createUserWithEmailAndPassword(email, password).then(r => {
            try {
                let u = <firebase.User>r;
                u.updateProfile({displayName: name, photoURL: ''});
            }
            finally {

            }
        });
    }

}