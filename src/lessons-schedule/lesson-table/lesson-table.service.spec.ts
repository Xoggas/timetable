import { Test, TestingModule } from '@nestjs/testing';
import { LessonTableService } from './lesson-table.service';

describe('LessonTableService', () => {
  let service: LessonTableService;

  beforeEach(async () => {
    const module: TestingModule = await Test.createTestingModule({
      providers: [LessonTableService],
    }).compile();

    service = module.get<LessonTableService>(LessonTableService);
  });

  it('should be defined', () => {
    expect(service).toBeDefined();
  });
});
