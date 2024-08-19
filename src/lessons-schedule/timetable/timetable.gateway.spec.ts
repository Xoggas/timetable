import { Test, TestingModule } from '@nestjs/testing';
import { TimetableGateway } from './timetable.gateway';
import { TimetableService } from './timetable.service';

describe('TimetableGateway', () => {
  let gateway: TimetableGateway;

  beforeEach(async () => {
    const module: TestingModule = await Test.createTestingModule({
      providers: [TimetableGateway, TimetableService],
    }).compile();

    gateway = module.get<TimetableGateway>(TimetableGateway);
  });

  it('should be defined', () => {
    expect(gateway).toBeDefined();
  });
});
