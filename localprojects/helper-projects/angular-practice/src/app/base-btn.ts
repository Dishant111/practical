import { Directive, HostListener } from '@angular/core';

@Directive({
  selector: '[appBaseBtn]',
  host: {
    class: 'btn',
  },
})
export class BaseBtn {
  constructor() {}
  @HostListener('click', ['$event']) onlcick(event: MouseEvent) {
    console.log('directive BaseBtn clicked', event);
    event.stopPropagation();
  }
}
