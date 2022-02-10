import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApprovalsIWantNewFormComponent } from './approvals-i-want-new-form.component';

describe('ApprovalsIWantNewFormComponent', () => {
  let component: ApprovalsIWantNewFormComponent;
  let fixture: ComponentFixture<ApprovalsIWantNewFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ApprovalsIWantNewFormComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ApprovalsIWantNewFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
