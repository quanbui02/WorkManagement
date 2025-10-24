import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { OAuthStorage } from 'angular-oauth2-oidc';
import { PermissionTypes } from '../constants';
import { UserService } from './user.service';
import * as _ from 'lodash';

@Injectable({
  providedIn: 'root'
})
export class AuthorizeService {

  constructor(
    private readonly userService: UserService,
    private readonly authStorage: OAuthStorage,
    private readonly http: HttpClient
  ) {}

  async fetchAuthorization(): Promise<void> {
    const user = this.userService.getBasicUserInfo();

    if (!user) {
      throw new Error('ERR_FETCH_USEREMPTY');
    }

    if (user.issuperuser) {
      this.authStorage.setItem('permissions', JSON.stringify({ superUser: true }));
      return;
    }

    let dataJson: any = {};
    try {
      dataJson = JSON.parse(user.permissions);
    } catch {
      console.warn('⚠️ Invalid permissions JSON for user:', user.userId);
      dataJson = {};
    }

    Object.keys(dataJson).forEach(key => {
      dataJson[key] = this.objectKeysToCamelCase(dataJson[key]);
    });

    this.authStorage.setItem('permissions', JSON.stringify(dataJson));
  }

  private objectKeysToCamelCase<T extends Record<string, any>>(entity: T): T {
    if (!_.isObject(entity)) return entity;

    const result = _.mapKeys(entity, (_value, key) => _.camelCase(key)) as Record<string, any>;
    return _.mapValues(result, (value: any) => this.objectKeysToCamelCase(value)) as T;
  }

  validated(permissionRequired: any, type: PermissionTypes): boolean {
    try {
      const permissionsRaw = this.authStorage.getItem('permissions');
      if (!permissionsRaw) return false;

      const granted = JSON.parse(permissionsRaw);
      if (!granted) return false;

      if (granted.superUser) return true;

      let accessGranted = true;
      const services = Object.keys(permissionRequired);

      for (const serviceName of services) {
        if (!granted.hasOwnProperty(serviceName)) {
          accessGranted = false;
          break;
        }

        const controllers = Object.keys(permissionRequired[serviceName]);
        for (const controllerName of controllers) {
          if (!granted[serviceName].hasOwnProperty(controllerName)) {
            accessGranted = false;
            break;
          }

          const bitRequired = permissionRequired[serviceName][controllerName];
          const bitGranted = granted[serviceName][controllerName];

          const bitMatch = (bitGranted & bitRequired) !== 0;
          if (!bitMatch && bitRequired !== 0) {
            accessGranted = false;
            break;
          }

          if (!bitMatch && bitRequired !== 0) {
            if (type === PermissionTypes.PAGE || type === PermissionTypes.CONTROL) {
              accessGranted = false;
              break;
            }
          }
        }
      }

      return accessGranted;
    } catch {
      return false;
    }
  }
}
