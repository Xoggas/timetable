import { Test, TestingModule } from '@nestjs/testing';
import { LessonTableGateway } from './lesson-table.gateway';

describe('LessonTableGateway', () => {
  let gateway: LessonTableGateway;

  beforeEach(async () => {
    const module: TestingModule = await Test.createTestingModule({
      providers: [LessonTableGateway],
    }).compile();

    gateway = module.get<LessonTableGateway>(LessonTableGateway);
  });

  it('should be defined', () => {
    expect(gateway).toBeDefined();
  });
});
