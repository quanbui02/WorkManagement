import { Injectable } from '@angular/core';
import { DatePipe } from '@angular/common';
import { ModuleConfigService } from './module-config.service';
import { HqModuleConfig } from '../../shared/models/module-config';
import { ConfigurationService } from './configuration.service';
@Injectable({
  providedIn: 'root'
})
export class HqCommonService {
    //config: HqModuleConfig;
    constructor(
        private _moduleConfigService: ModuleConfigService,
        private _configurationService: ConfigurationService
    ) {
        //this.config = _moduleConfigService.getConfig();
    }

    checkYear(tuNgay: Date, denNgay: Date, year: number): boolean {
        let tuYear = tuNgay.getFullYear();
        let denYear = denNgay.getFullYear();
        if (tuYear < year || denYear < year || tuYear > year + 1 || denYear > year + 1) {
            return false;
        }
        if (tuNgay < new Date('9-1-' + year) || tuNgay > new Date('6-1-' + (year + 1)) || denNgay < new Date('9-1-' + year) || denNgay > new Date('6-1-' + (year + 1))) {
            return false;
        }
        return true;
    }
    // Lấy về giá trị theo path ''
    getValueByPath(obj: any, path: string): string {
        const paths = path.split('.');
        for (let i = 0; i < paths.length; i++) {
            try {
                obj = obj[paths[i]];
            } catch (err) {
                obj = null;
            }

        }
        return obj;
    }

 exportToCSV(datas: any[], columns: any[], fileName: string): void {
        if (!datas?.length || !columns?.length) return;

        let headerString = columns.map((c: any) => c.header).join(',') + '\n';

        const rowsString: string[] = datas.map((d: any) => {
            let rowString = '';

            columns.forEach((c: any) => {
                let colVal: string | null = null;

                // Lấy giá trị theo dataPath nếu có
                if (c.dataPath) {
                    const colValTmp = this.getValueByPath(d, c.dataPath);
                    if (colValTmp !== undefined && colValTmp !== null) {
                        colVal = colValTmp.toString();
                    }
                } else if (d[c.field] !== undefined && d[c.field] !== null) {
                    colVal = d[c.field].toString();
                }

                // Format Date
                if (c.dateFormat && colVal) {
                    const datePipe = new DatePipe('en-US');
                    colVal = datePipe.transform(colVal, c.dateFormat) ?? colVal;
                }

                // Format mapping
                if (c.dataMapping && Array.isArray(c.dataMapping)) {
                    c.dataMapping.forEach((dm: { id: any; name: any }) => {
                        if (dm.id === d[c.field]) {
                            colVal = dm.name?.toString() ?? '';
                        }
                    });
                }

                // Làm sạch ký tự đặc biệt và nối chuỗi
                rowString += (colVal ?? '')
                    .replace(/,/g, '.')
                    .replace(/\r?\n|\r/g, '') + ',';
            });

            return rowString;
        });

        const csvContent = headerString + rowsString.join('\n');
        const blob = new Blob(['\uFEFF', csvContent], { type: 'text/csv;charset=utf-8;' });

        const link = document.createElement('a');
        link.href = window.URL.createObjectURL(blob);
        link.download = `${fileName}.csv`;
        document.body.appendChild(link); // Required for Firefox
        link.click();
        document.body.removeChild(link);
    }
    
    refreshLogSession() {
        const newKey = this.genGuid();
        sessionStorage.setItem(this._configurationService.logSessionKey, newKey);
    }

    genGuid() {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            const r = Math.random() * 16 | 0, v = c === 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    }
}
