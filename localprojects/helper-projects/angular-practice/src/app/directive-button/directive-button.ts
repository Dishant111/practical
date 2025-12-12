import { Component } from '@angular/core';
import { BaseBtn } from '../base-btn';

@Component({
  selector: 'app-directive-button',
  imports: [BaseBtn],
  template: '<button (click)="onClick()" appBaseBtn>Directive button</button>',
  styleUrl: './directive-button.scss',
})
export class DirectiveButton {
  onClick() {
    console.log('Component clicked');
  }
}
