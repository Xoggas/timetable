import { Test, TestingModule } from '@nestjs/testing';
import { LessonListController } from './lesson-list.controller';
import { LessonListService } from './lesson-list.service';

describe('LessonListController', () => {
  let controller: LessonListController;

  beforeEach(async () => {
    const module: TestingModule = await Test.createTestingModule({
      controllers: [LessonListController],
      providers: [LessonListService],
    }).compile();

    controller = module.get<LessonListController>(LessonListController);
  });

  it('should be defined', () => {
    expect(controller).toBeDefined();
  });
});
