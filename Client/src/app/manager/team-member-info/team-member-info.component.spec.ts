import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TeamMemberInfoComponent } from './team-member-info.component';

describe('TeamMemberInfoComponent', () => {
  let component: TeamMemberInfoComponent;
  let fixture: ComponentFixture<TeamMemberInfoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TeamMemberInfoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TeamMemberInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
