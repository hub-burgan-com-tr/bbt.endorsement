import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApprovalsIWantNewOrderDetailComponent } from './approvals-i-want-new-order-detail.component';

describe('ApprovalsIWantNewOrderDetailComponent', () => {
  let component: ApprovalsIWantNewOrderDetailComponent;
  let fixture: ComponentFixture<ApprovalsIWantNewOrderDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ApprovalsIWantNewOrderDetailComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ApprovalsIWantNewOrderDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
