import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SpinnerLoaderComponent } from './spinner-loader.component';

describe('SpinnerLoaderComponent', () => {
  let component: SpinnerLoaderComponent;
  let fixture: ComponentFixture<SpinnerLoaderComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SpinnerLoaderComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SpinnerLoaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
