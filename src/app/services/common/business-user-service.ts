import { Injectable } from '@angular/core';
import { BizimNetHttpClientService } from '../httpClient/bizimnet-http-client-service';
import { BusinessUser } from '../../models/businessUser/businessUser';
import { ResponseModel } from '../../models/responseModel';
import { Observable } from 'rxjs';
import { ListResponseModel } from '../../models/listResponseModel';
import { SingleResponseModel } from '../../models/singleResponseModel';
import { BusinessUserDto } from '../../models/businessUser/businessUserDto';
import { StorageService } from '../storage-sevice';

@Injectable({
  providedIn: 'root'
})
export class BusinessUserService extends BizimNetHttpClientService {

  private _controller: string = "BusinessUser";
  addUser(user:BusinessUser){
    const observable=this.post<ResponseModel | BusinessUser>({controller:this._controller,action:"Add"},user) as Observable<ResponseModel>
    return observable
  } 
  updateUser(user:BusinessUser){
    const observable=this.post<ResponseModel | BusinessUser>({controller:this._controller,action:"Update"},user) as Observable<ResponseModel>
    return observable
  } 
  deleteUser(id:string){
    const observable=this.get<ResponseModel>({controller:this._controller,action:"Delete",queryString:`id=${id}`}) 
    return observable
  }
  getAll(){
    return this.get<ListResponseModel<BusinessUserDto>>({controller:this._controller,action:"GetAll"})
  }
  getById(id:string){
    return this.get<SingleResponseModel<BusinessUserDto>>({controller:this._controller,action:"GetByUser",queryString:`id=${id}`})
  } 
  isAuthenticated() {
    if (this.localStorageGet("token")) {
      return true;
    }
    else {
      return false;
    }
  }
}
