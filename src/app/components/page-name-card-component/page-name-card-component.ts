import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot, NavigationEnd, Router } from '@angular/router';

@Component({
  selector: 'app-page-name-card-component',
  imports: [CommonModule],
  templateUrl: './page-name-card-component.html',
  styleUrl: './page-name-card-component.css'
})
export class PageNameCardComponent {
breadCrum: string = '';

 

  constructor(private router: Router, private route: ActivatedRoute) { }


  ngOnInit(): void {
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        const currentRoute = this.router.routerState.snapshot.root;
        this.breadCrum = this.getBreadcrumb(currentRoute);
      }
    });
  }
  private getBreadcrumb(route: ActivatedRouteSnapshot): string {
    while (route.firstChild) {
      route = route.firstChild;
    }
    return route.data["breadcrumb"];  
  }
}
