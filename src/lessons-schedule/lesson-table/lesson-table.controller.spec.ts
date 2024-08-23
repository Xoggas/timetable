import { Test, TestingModule } from '@nestjs/testing';
import { LessonTableController } from './lesson-table/lesson-table.controller';

describe('LessonTableController', () => {
  let controller: LessonTableController;

  beforeEach(async () => {
    const module: TestingModule = await Test.createTestingModule({
      controllers: [LessonTableController],
    }).compile();

    controller = module.get<LessonTableController>(LessonTableController);
  });

  it('should be defined', () => {
    expect(controller).toBeDefined();
  });
});
