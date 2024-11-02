import { Test, TestingModule } from '@nestjs/testing';
import { LessonListController } from './lesson-list.controller';
import { LessonListService } from './lesson-list.service';
import { Lesson } from './schemas/lesson-list.schema';
import { CreateLessonDto } from './dto/create-lesson.dto';
import { UpdateLessonDto } from './dto/update-lesson.dto';
import { DeleteLessonDto } from './dto/delete-lesson.dto';

describe('LessonListController', () => {
  const lessonModelMock = {
    name: 'Math'
  };

  const lessonListServiceMock = {
    findAll: jest.fn().mockResolvedValue([lessonModelMock]),
    create: jest.fn().mockResolvedValue(lessonModelMock),
    update: jest.fn().mockResolvedValue(lessonModelMock),
    delete: jest.fn().mockResolvedValue(lessonModelMock)
  };

  let controller: LessonListController;
  let service: LessonListService;

  beforeEach(async () => {
    const moduleRef: TestingModule = await Test.createTestingModule({
      controllers: [LessonListController],
      providers: [
        {
          provide: LessonListService,
          useValue: lessonListServiceMock
        }
      ],
    }).compile();

    controller = moduleRef.get(LessonListController);
    service = moduleRef.get(LessonListService);

    jest.clearAllMocks();
  });

  test('controller should be defined', () => {
    expect(controller).toBeDefined();
  });

  describe('findAll', () => {
    const expected = [lessonModelMock];

    let got: Lesson[];

    beforeEach(async () => {
      got = await controller.findAll();
    });

    test('should call lessonListService.findAll', () => {
      expect(service.findAll).toHaveBeenCalled();
    });

    test('should return an array of lessons', async () => {
      expect(got).toEqual(expected);
    });
  });

  describe('create', () => {
    const createLessonDto: CreateLessonDto = {
      ...lessonModelMock,
    };

    const expected: Lesson = lessonModelMock;

    let got: Lesson;

    beforeEach(async () => {
      got = await controller.create(createLessonDto);
    });

    test('should call lessonListService.create with the dto', () => {
      expect(service.create).toHaveBeenCalledWith(createLessonDto);
    });

    test('should return a created lesson', () => {
      expect(got).toEqual(expected);
    });
  });

  describe('update', () => {
    const updateLessonDto: UpdateLessonDto = {
      id: 'random-id',
      ...lessonModelMock,
    };

    const expected: Lesson = lessonModelMock;

    let got: Lesson;

    beforeEach(async () => {
      got = await controller.update(updateLessonDto);
    });

    test('should call lessonListService.update with the dto', () => {
      expect(service.update).toHaveBeenCalledWith(updateLessonDto);
    });

    test('should return an updated lesson', () => {
      expect(got).toEqual(expected);
    });

    test('should not throw any exceptions', () => {
      expect(service.update).not.toThrow();
    });
  });

  describe('delete', () => {
    const deleteLessonDto: DeleteLessonDto = {
      id: 'random-id',
    };

    const expected: Lesson = lessonModelMock;

    let got: Lesson;

    beforeEach(async () => {
      got = await controller.delete(deleteLessonDto);
    });

    test('should call lessonListService.delete with the dto', () => {
      expect(service.delete).toHaveBeenCalledWith(deleteLessonDto);
    });

    test('should return a deleted lesson', () => {
      expect(got).toEqual(expected);
    });
  });
});
