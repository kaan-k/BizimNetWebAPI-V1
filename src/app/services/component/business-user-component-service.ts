import { Injectable } from '@angular/core';
import { BusinessUserService } from '../common/business-user-service';
import { ToastrService } from 'ngx-toastr';
import { firstValueFrom } from 'rxjs';
import { BusinessUser } from '../../models/businessUser/businessUser';

@Injectable({
  providedIn: 'root'
})
export class BusinessUserComponentService {

  constructor(
    private businessUserService: BusinessUserService,
    private toastrService: ToastrService
  ) {}

  async getAllUser() {
    const observable = this.businessUserService.getAll();
    const response = await firstValueFrom(observable);
    return response.data;
  }

  async getById(id: string) {
    const observable = this.businessUserService.getById(id);
    const response = await firstValueFrom(observable);
    return response.data;
  }

  async deleteUser(id: string, callBackfunction?: () => void) {
    const observable = this.businessUserService.deleteUser(id);
    const promiseData = firstValueFrom(observable);

    promiseData.then(response => {
      this.toastrService.success(response.message);
      callBackfunction?.();
    }).catch((error: any) => {
      this.toastrService.error(error.error);
    });
  }

  async updateUser(
    user: BusinessUser,
    callBackfunction?: () => void,
    callBackError?: (err: any) => void
  ) {
    const observable = this.businessUserService.updateUser(user);
    const promiseData = firstValueFrom(observable);

    promiseData.then(response => {
      this.getById(user.id); // this is not awaited; you may want to add `await` if it's needed
      this.toastrService.success(response.message);
      callBackfunction?.();
    }).catch((error: any) => {
      callBackError?.(error);
    });
  }

  async addUser(
    user: BusinessUser,
    callBackfunction?: () => void,
    callBackError?: (err: any) => void
  ) {
    const observable = this.businessUserService.addUser(user);
    const promiseData = firstValueFrom(observable);

    promiseData.then(response => {
      this.toastrService.success(response.message);
      callBackfunction?.();
    }).catch((error: any) => {
      callBackError?.(error);
    });
  }

  isAuthenticated() {
    return this.businessUserService.isAuthenticated();
  }
}
