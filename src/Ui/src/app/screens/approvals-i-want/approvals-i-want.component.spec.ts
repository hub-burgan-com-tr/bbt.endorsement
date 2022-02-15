import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApprovalsIWantComponent } from './approvals-i-want.component';

describe('ApprovalsIWantComponent', () => {
  let component: ApprovalsIWantComponent;
  let fixture: ComponentFixture<ApprovalsIWantComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ApprovalsIWantComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ApprovalsIWantComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
