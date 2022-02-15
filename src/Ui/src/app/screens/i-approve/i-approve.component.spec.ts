import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IApproveComponent } from './i-approve.component';

describe('IApproveComponent', () => {
  let component: IApproveComponent;
  let fixture: ComponentFixture<IApproveComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ IApproveComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(IApproveComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
