import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DonwloadButton } from './donwload-button';

describe('DonwloadButton', () => {
  let component: DonwloadButton;
  let fixture: ComponentFixture<DonwloadButton>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DonwloadButton]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DonwloadButton);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
