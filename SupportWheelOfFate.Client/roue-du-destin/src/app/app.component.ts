import {Component} from '@angular/core';
import {AppService} from './app.service';

interface AppViewModel {
    title: string;
    involvedEngineersCount: number;
    supportDays: any[];
}

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    providers: [AppService],
    styleUrls: ['./app.component.css']
})
export class AppComponent {

    _viewModel: AppViewModel = Object.freeze({title: 'Support wheel of fate', involvedEngineersCount: 4, supportDays: []});

    readonly range = (start, end) => Array.from({length: (end - start)}, (v, k) => k + start);

    constructor(private readonly _service: AppService) {}

    public turnWheel() {
        this._service
            .turnWheel(this._viewModel.involvedEngineersCount)
            .subscribe(this.onSupportDaysReceived.bind(this));
    }

    public selectionChange(value: string) {
        this.mergeAndRefreshViewModel({involvedEngineersCount: value});
    }

    private onSupportDaysReceived(supportDays: any[]) {
        this.mergeAndRefreshViewModel({supportDays: supportDays});
    }

    private  mergeAndRefreshViewModel(chunk) {
        const merge = () => Object.assign({}, this._viewModel, chunk );
        this._viewModel = Object.freeze(merge());
    }
}
