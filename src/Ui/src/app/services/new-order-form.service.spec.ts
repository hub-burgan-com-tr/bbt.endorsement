import { TestBed } from '@angular/core/testing';

import { NewOrderFormService } from './new-order-form.service';

describe('NewOrderFormService', () => {
  let service: NewOrderFormService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(NewOrderFormService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
