import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DropdownlistComponent } from './dropdownlist.component';

describe('DropdownlistComponent', () => {
  let component: DropdownlistComponent;
  let fixture: ComponentFixture<DropdownlistComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DropdownlistComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DropdownlistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
