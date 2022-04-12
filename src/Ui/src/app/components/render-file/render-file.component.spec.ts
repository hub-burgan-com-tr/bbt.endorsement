import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RenderFileComponent } from './render-file.component';

describe('RenderFileComponent', () => {
  let component: RenderFileComponent;
  let fixture: ComponentFixture<RenderFileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RenderFileComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RenderFileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
