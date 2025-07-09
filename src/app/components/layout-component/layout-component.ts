import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { SidebarComponent } from '../sidebar-component/sidebar-component';
import { PageNameCardComponent } from '../page-name-card-component/page-name-card-component';
import { Router, RouterModule, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-layout-component',
  imports: [CommonModule,SidebarComponent,PageNameCardComponent,RouterModule,RouterOutlet],
  templateUrl: './layout-component.html',
  styleUrl: './layout-component.css'
})
export class LayoutComponent {
constructor(private router:Router) {}
  ngOnInit():void{
    if (typeof window !== 'undefined' && typeof localStorage !== 'undefined') {
      
    // const expirationDate = new Date(localStorage.getItem('expiration'));
    // const currentDate = new Date();
    // if (currentDate> expirationDate) {
    //     localStorage.removeItem("token");
    //     localStorage.removeItem("expiration");
    //     localStorage.removeItem("userId");
    //     localStorage.removeItem("employeeId");
    //     this.router.navigate(['/user-login']);
    // }
    // if (localStorage.getItem("lng") == null) {
    //   localStorage.setItem("lng","tr"); 
    // }
    // if(localStorage.getItem("paginationLimit") == null){
    //   localStorage.setItem("paginationLimit","6");
    // }
    }
  }
}
