import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PotentialMembersComponent } from './potential-members.component';

describe('PotentialMembersComponent', () => {
  let component: PotentialMembersComponent;
  let fixture: ComponentFixture<PotentialMembersComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PotentialMembersComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PotentialMembersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
