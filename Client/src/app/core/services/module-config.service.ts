import { Injectable, InjectionToken, Optional, Inject } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { ModuleConfig } from '../../shared/configs/module-config';


export function moduleConfigFunc() {
    return new InjectionToken<ModuleConfig>('');
}

@Injectable({
    providedIn: 'root'
})
export class ModuleConfigService {
    protected _config: ModuleConfig;

    constructor(@Optional() @Inject(moduleConfigFunc) moduleConfigVal: any = null) {
        // console.log('module config inject', moduleConfigVal());
        this._config = moduleConfigVal() ||
            <ModuleConfig>{
                ApiFileUpload: `${environment.apiDomain.fileEndpoint}/upload`
            };
    }

    getConfig() {
        return this._config;
    }
}
