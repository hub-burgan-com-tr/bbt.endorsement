import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MainContentButtonGroupComponent } from './main-content-button-group.component';

describe('MainContentButtonGroupComponent', () => {
  let component: MainContentButtonGroupComponent;
  let fixture: ComponentFixture<MainContentButtonGroupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MainContentButtonGroupComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MainContentButtonGroupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
