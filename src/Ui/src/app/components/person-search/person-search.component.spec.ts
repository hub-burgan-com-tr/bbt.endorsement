import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PersonSearchComponent } from './person-search.component';

describe('PersonSearchComponent', () => {
  let component: PersonSearchComponent;
  let fixture: ComponentFixture<PersonSearchComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PersonSearchComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PersonSearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
