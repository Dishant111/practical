import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HomeDemo } from './home-demo';

describe('HomeDemo', () => {
  let component: HomeDemo;
  let fixture: ComponentFixture<HomeDemo>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HomeDemo]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HomeDemo);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
