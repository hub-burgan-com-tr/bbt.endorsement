import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IApproveDetailComponent } from './i-approve-detail.component';

describe('IApproveDetailComponent', () => {
  let component: IApproveDetailComponent;
  let fixture: ComponentFixture<IApproveDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ IApproveDetailComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(IApproveDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
