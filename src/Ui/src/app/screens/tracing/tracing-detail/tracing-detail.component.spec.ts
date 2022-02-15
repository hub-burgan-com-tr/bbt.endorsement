import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TracingDetailComponent } from './tracing-detail.component';

describe('TracingDetailComponent', () => {
  let component: TracingDetailComponent;
  let fixture: ComponentFixture<TracingDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TracingDetailComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TracingDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
