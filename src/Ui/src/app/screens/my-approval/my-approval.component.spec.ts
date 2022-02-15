import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MyApprovalComponent } from './my-approval.component';

describe('DashboardComponent', () => {
  let component: MyApprovalComponent;
  let fixture: ComponentFixture<MyApprovalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MyApprovalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MyApprovalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
