import { Injectable } from "@angular/core";
import { AngularFireDatabase, AngularFireList } from "angularfire2/database";
import { Comanda } from "../interfaces/comanda.interface";

@Injectable()
export class ComenziService {
    private basePath: string = 'comenzi/';

    constructor(private db: AngularFireDatabase) {

    }

    public getComenzi(): AngularFireList<Comanda> {
        return this.db.list<Comanda>(this.basePath);
    }

    public addComanda(comanda: Comanda) {
        this.db.list(this.basePath).push(comanda);
    }

    public deleteAll() {
        this.db.list(this.basePath).remove();
    }

    public delete(key: string) {
        this.db.list(this.basePath + key).remove(key);
    }
}