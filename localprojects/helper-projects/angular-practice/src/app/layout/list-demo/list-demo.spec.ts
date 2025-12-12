import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListDemo } from './list-demo';

describe('ListDemo', () => {
  let component: ListDemo;
  let fixture: ComponentFixture<ListDemo>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListDemo]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ListDemo);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
