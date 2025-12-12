import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DirectiveButton } from './directive-button';

describe('DirectiveButton', () => {
  let component: DirectiveButton;
  let fixture: ComponentFixture<DirectiveButton>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DirectiveButton]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DirectiveButton);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
