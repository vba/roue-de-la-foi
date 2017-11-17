import {Injectable} from '@angular/core';
import {Http} from '@angular/http';
import 'rxjs/add/operator/map'

@Injectable()
export class AppService {

    private readonly WheelUrl = 'http://localhost:5000/api/wheel/';

    constructor(private readonly _http: Http) {
    }

    public turnWheel(involvedEngineersCount: number) {
        return this._http
            .get(`${this.WheelUrl}${involvedEngineersCount}`)
            .map(x => x.json());
    }

}
