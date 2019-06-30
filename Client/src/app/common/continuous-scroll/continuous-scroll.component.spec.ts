import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ContinuousScrollComponent } from './continuous-scroll.component';

describe('ContinuousScrollComponent', () => {
  let component: ContinuousScrollComponent;
  let fixture: ComponentFixture<ContinuousScrollComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ContinuousScrollComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ContinuousScrollComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
