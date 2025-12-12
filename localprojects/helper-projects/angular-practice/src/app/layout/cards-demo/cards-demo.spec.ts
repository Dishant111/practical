import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CardsDemo } from './cards-demo';

describe('CardsDemo', () => {
  let component: CardsDemo;
  let fixture: ComponentFixture<CardsDemo>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CardsDemo]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CardsDemo);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
