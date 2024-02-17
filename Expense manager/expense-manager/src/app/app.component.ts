import { Component, ViewChild } from '@angular/core';
import { MatSidenav } from '@angular/material/sidenav';
import { Subscription } from 'rxjs';
import { navItems } from './navItems';
import { productcards } from './productcards';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'expense-manager';
  navItems = navItems;

  @ViewChild('leftsidenav')
  public sidenav!: MatSidenav;

  //get options from service
  private layoutChangesSubscription = Subscription.EMPTY;
  private isMobileScreen = false;
  private isContentWidthFixed = true;
  private isCollapsedWidthFixed = false;
  private htmlElement!: HTMLHtmlElement;

  get isOver(): boolean {
    return this.isMobileScreen;
  }
  
  productcards: productcards[] = [
    {
      id: 1,
      imgSrc: '/assets/images/products/s4.jpg',
      title: 'Boat Headphone',
      price: '285',
      rprice: '375',
    },
    {
      id: 2,
      imgSrc: '/assets/images/products/s5.jpg',
      title: 'MacBook Air Pro',
      price: '285',
      rprice: '375',
    },
    {
      id: 3,
      imgSrc: '/assets/images/products/s7.jpg',
      title: 'Red Valvet Dress',
      price: '285',
      rprice: '375',
    },
    {
      id: 4,
      imgSrc: '/assets/images/products/s11.jpg',
      title: 'Cute Soft Teddybear',
      price: '285',
      rprice: '375',
    },
  ];
  ngOnDestroy() {
    this.layoutChangesSubscription.unsubscribe();
  }

  toggleCollapsed() {
    this.isContentWidthFixed = false;
  }
  onSidenavClosedStart() {
    this.isContentWidthFixed = false;
  }

  onSidenavOpenedChange(isOpened: boolean) {
    this.isCollapsedWidthFixed = !this.isOver;
  }
}



