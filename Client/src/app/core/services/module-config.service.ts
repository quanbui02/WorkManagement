import { Injectable, InjectionToken, Optional, Inject } from '@angular/core';
import { environment } from '../../../environments/environment';
import { ModuleConfig } from '../../shared/configs/module-config';

// ✅ tạo token cố định, không phải function
export const MODULE_CONFIG = new InjectionToken<ModuleConfig>('MODULE_CONFIG');

@Injectable({
  providedIn: 'root'
})
export class ModuleConfigService {
  //protected _config: ModuleConfig;

//   constructor(@Optional() @Inject(MODULE_CONFIG) private moduleConfigVal: ModuleConfig | null) {
//     this._config = moduleConfigVal || {
//       ApiFileUpload: `${environment.apiDomain.fileEndpoint}/upload`
//     };
//   }

//   getConfig() {
//     return this._config;
//   }
}
