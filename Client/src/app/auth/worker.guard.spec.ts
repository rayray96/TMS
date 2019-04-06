import { TestBed, async, inject } from '@angular/core/testing';

import { WorkerGuard } from './worker.guard';

describe('WorkerGuard', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [WorkerGuard]
    });
  });

  it('should ...', inject([WorkerGuard], (guard: WorkerGuard) => {
    expect(guard).toBeTruthy();
  }));
});
