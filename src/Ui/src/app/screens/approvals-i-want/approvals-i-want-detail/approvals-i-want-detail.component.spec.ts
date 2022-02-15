import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApprovalsIWantDetailComponent } from './approvals-i-want-detail.component';

describe('ApprovalsIWantDetailComponent', () => {
  let component: ApprovalsIWantDetailComponent;
  let fixture: ComponentFixture<ApprovalsIWantDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ApprovalsIWantDetailComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ApprovalsIWantDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
