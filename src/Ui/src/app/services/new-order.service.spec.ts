import { TestBed } from '@angular/core/testing';

import { NewOrderService } from './new-order.service';

describe('NewOrderService', () => {
  let service: NewOrderService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(NewOrderService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
