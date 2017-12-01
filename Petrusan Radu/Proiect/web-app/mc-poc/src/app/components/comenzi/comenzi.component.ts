import { Component, OnInit } from "@angular/core";
import { AuthService } from "../../services/auth.service";
import { ComenziService } from "../../services/comenzi.service";
import { AngularFirestoreCollection } from "angularfire2/firestore";
import { Observable } from "rxjs/Observable";
import { Comanda } from "../../interfaces/comanda.interface";
import { AngularFireList } from "angularfire2/database";

@Component({
    templateUrl: './comenzi.component.html',
    styleUrls: ['./comenzi.component.css']
})
export class ComenziComponent implements OnInit {

    comenziCollection: AngularFireList<Comanda>;
    comenzi: Observable<Comanda[]>;

    msgVal: string = '';

    constructor(private authService: AuthService, private comenziService: ComenziService) {

    }

    ngOnInit(): void {
        this.comenziCollection = this.comenziService.getComenzi();
        this.comenzi = this.comenziCollection.valueChanges();
    }

    Send(desc: string) {
        let comanda: Comanda = <Comanda>{};
        comanda.message = desc;
        comanda.dataPlasarii = new Date();
        comanda.clientId = this.authService.loggedInUser();

        this.comenziService.addComanda(comanda);

        this.msgVal = '';
    }

    deleteAll() {
        this.comenziService.deleteAll();
    }

    delete(comanda: Comanda) {
        this.comenziService.delete(comanda.$key);
    }
}