import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MyApprovalCompletedComponent } from './my-approval-completed.component';

describe('MyApprovalCompletedComponent', () => {
  let component: MyApprovalCompletedComponent;
  let fixture: ComponentFixture<MyApprovalCompletedComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MyApprovalCompletedComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MyApprovalCompletedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
