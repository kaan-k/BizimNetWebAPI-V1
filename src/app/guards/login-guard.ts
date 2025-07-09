import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivateFn, Router, RouterStateSnapshot } from '@angular/router';
import { BusinessUserComponentService } from '../services/component/business-user-component-service';

export const loginGuard: CanActivateFn = (route : ActivatedRouteSnapshot, state:RouterStateSnapshot) => {
//   const router = inject(Router)
//   const userAuthComponentService=inject(BusinessUserComponentService)
//   if (userAuthComponentService.isAuthenticated()) {
//     return true
//   }
//   router.navigate(['/user-login']);
//   return false;
return true
 };
