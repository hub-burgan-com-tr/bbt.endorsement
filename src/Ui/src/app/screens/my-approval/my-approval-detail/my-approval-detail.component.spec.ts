import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MyApprovalDetailComponent } from './my-approval-detail.component';

describe('MyApprovalDetailComponent', () => {
  let component: MyApprovalDetailComponent;
  let fixture: ComponentFixture<MyApprovalDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MyApprovalDetailComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MyApprovalDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
