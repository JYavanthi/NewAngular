import { TestBed } from '@angular/core/testing';

import { UiStateServiceService } from './ui-state.service.service';

describe('UiStateServiceService', () => {
  let service: UiStateServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UiStateServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
