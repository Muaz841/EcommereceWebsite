import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { filter } from 'rxjs/operators';
import { MenuItem } from 'primeng/api';


@Component({
  selector: 'app-breadcrumb',
  template: `
<div class="breadcrumb-container">
  <ng-container *ngFor="let item of menuItems; let last = last">
    <ng-container *ngIf="!last">
      <a 
        *ngIf="item.route; else externalLink" 
        [routerLink]="item.route" 
        class="breadcrumb-item"
      >
        {{ item.label }}
      </a>
      <ng-template #externalLink>
        <a [href]="item.url" class="breadcrumb-item">{{ item.label }}</a>
      </ng-template>
      <span class="breadcrumb-separator">></span>
    </ng-container>

    <!-- Last Item (Non-clickable) -->
    <span *ngIf="last" class="breadcrumb-item-active">
      {{ item.label }}
    </span>
  </ng-container>
</div>

`,
  styleUrls: ['./breadcrumb.css']
})
export class BreadcrumbComponent implements OnInit {
  menuItems: MenuItem[] = [];
  home = { icon: 'pi pi-home', label: '', url: '/home' };  

  constructor(private router: Router, private activatedRoute: ActivatedRoute,private cd :ChangeDetectorRef) {
    this.menuItems = this.createBreadcrumbs(this.activatedRoute.root);
  }

  ngOnInit(): void {
    
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe(() => {              
        this.setHomeLabel(this.activatedRoute.root);                
        this.cd.markForCheck();
        this.cd.detectChanges();
      });
  }

  private setHomeLabel(route: ActivatedRoute): void {
    
    if (route.snapshot.data && route.snapshot.data['breadcrumb']) {
      this.home.label = route.snapshot.data['breadcrumb'];  
    } else {
      this.home.label = 'Home';  
    }
  }

  private createBreadcrumbs(
    route: ActivatedRoute,        
    url: string = '',
    breadcrumbs: MenuItem[] = [],
    isParent: boolean = true 
  ): MenuItem[] {
    const children: ActivatedRoute[] = route.children;

    if (children.length === 0) {
      return breadcrumbs; 
    }

    
    if (isParent && breadcrumbs.length === 0 && route.snapshot.data['breadcrumb']) {
      breadcrumbs.push(this.home);  
    }

    
    for (const child of children) {
      const routeURL: string = child.snapshot.url.map(segment => segment.path).join('/');
      
      if (routeURL !== '') {
        url += `/${routeURL}`; 
      }

     
      const label = child.snapshot.data['breadcrumb']; 
      if (label) {
        breadcrumbs.push({ label, url });
      }

      
      breadcrumbs = this.createBreadcrumbs(child, url, breadcrumbs, false); 
    }

    return breadcrumbs; 
  }
}
