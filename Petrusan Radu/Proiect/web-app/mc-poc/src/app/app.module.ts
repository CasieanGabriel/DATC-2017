// Modules
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { AngularFireDatabaseModule, AngularFireDatabase, AngularFireList } from 'angularfire2/database';
import { AngularFireAuthModule, AngularFireAuth } from 'angularfire2/auth';
import { AngularFireModule } from 'angularfire2';
import { RouterModule, Routes } from '@angular/router';
import { FlashMessagesModule } from 'angular2-flash-messages';
import { AngularFirestoreModule } from 'angularfire2/firestore';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

// Components
import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { HomeComponent } from './components/home/home.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { ComenziComponent } from './components/comenzi/comenzi.component';

// Services
import { AuthService } from './services/auth.service';
import { GlobalVariables } from './services/global-variables.service';
import { ComenziService } from './services/comenzi.service';

// Guards
import { AuthGuard } from './guards/auth.guard';
import { EmailLoginComponent } from './components/login/email-login/email-login.component';
import { RegisterComponent } from './components/register/register.component';

export const firebaseConfig = {
  apiKey: "AIzaSyBhMfBKF1yNxt4ZgjWlO4pzcuV1B7-yP3w",
    authDomain: "mcpocdeva.firebaseapp.com",
    databaseURL: "https://mcpocdeva.firebaseio.com",
    projectId: "mcpocdeva",
    storageBucket: "",
    messagingSenderId: "1085237414328"
};

const appRoutes: Routes = [
  {path: '', component: HomeComponent},
  {path: 'login', component: LoginComponent},
  {path: 'register', component: RegisterComponent},
  {path: 'email-login', component: EmailLoginComponent},
  {path: 'comenzi', component: ComenziComponent, canActivate:[AuthGuard]}
];

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    NavbarComponent,
    HomeComponent,
    ComenziComponent,
    EmailLoginComponent,
    RegisterComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
    AngularFireModule.initializeApp(firebaseConfig),
    AngularFireDatabaseModule,
    AngularFireAuthModule,
    AngularFirestoreModule,
    FlashMessagesModule,
    RouterModule.forRoot(appRoutes),
    BrowserAnimationsModule
  ],
  providers: [
    AuthService,
    AuthGuard,
    ComenziService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
