import { TestBed } from '@angular/core/testing';

import { EveluatorService } from './eveluator.service';

describe('EveluatorService', () => {
  let service: EveluatorService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(EveluatorService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
