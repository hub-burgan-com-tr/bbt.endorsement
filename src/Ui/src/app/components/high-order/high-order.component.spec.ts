import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HighOrderComponent } from './high-order.component';

describe('HighOrderComponent', () => {
  let component: HighOrderComponent;
  let fixture: ComponentFixture<HighOrderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HighOrderComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HighOrderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
