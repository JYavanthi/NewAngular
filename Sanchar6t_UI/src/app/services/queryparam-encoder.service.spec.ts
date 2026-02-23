import { TestBed } from '@angular/core/testing';

import { QueryparamEncoderService } from './queryparam-encoder.service';

describe('QueryparamEncoderService', () => {
  let service: QueryparamEncoderService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(QueryparamEncoderService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
