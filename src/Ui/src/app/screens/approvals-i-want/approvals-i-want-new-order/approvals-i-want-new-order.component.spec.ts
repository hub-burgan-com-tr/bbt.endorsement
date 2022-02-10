import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApprovalsIWantNewOrderComponent } from './approvals-i-want-new-order.component';

describe('ApprovalsIWantNewOrderComponent', () => {
  let component: ApprovalsIWantNewOrderComponent;
  let fixture: ComponentFixture<ApprovalsIWantNewOrderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ApprovalsIWantNewOrderComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ApprovalsIWantNewOrderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
