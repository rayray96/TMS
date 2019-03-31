import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FiredMemberComponent } from './fired-member.component';

describe('FiredMemberComponent', () => {
  let component: FiredMemberComponent;
  let fixture: ComponentFixture<FiredMemberComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FiredMemberComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FiredMemberComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
