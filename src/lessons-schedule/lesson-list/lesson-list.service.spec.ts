import { Model } from "mongoose";
import { Test, TestingModule } from "@nestjs/testing";
import { LessonListService } from "./lesson-list.service";
import { Lesson } from "./schemas/lesson-list.schema";
import { getModelToken } from "@nestjs/mongoose";
import { CreateLessonDto } from "./dto/create-lesson.dto";

describe("LessonListService", () =>
{
    const lessonMock = {
        name: "Math"
    };

    const lessonModelMock = {
        find: jest.fn().mockImplementation(() =>
        {
            return {
                exec: (): Lesson[] => [lessonMock]
            };
        }),
        save: jest.fn().mockResolvedValue(lessonMock),
        findByIdAndUpdate: jest.fn().mockImplementation(() =>
        {
            return {
                exec: (): Lesson => lessonMock
            };
        }),
        findByIdAndDelete: jest.fn().mockResolvedValue(lessonMock)
    };

    let service: LessonListService;
    let model: Model<Lesson>;

    beforeEach(async () =>
    {
        const moduleRef: TestingModule = await Test.createTestingModule({
            providers: [
                LessonListService,
                {
                    provide: getModelToken(Lesson.name),
                    useValue: lessonModelMock
                }
            ]
        }).compile();

        service = moduleRef.get(LessonListService);
        model = moduleRef.get<Model<Lesson>>(getModelToken(Lesson.name));

        jest.clearAllMocks();
    });

    test("service should be defined", () =>
    {
        expect(service).toBeDefined();
    });

    describe("findAll", () =>
    {
        const expected: Lesson[] = [lessonMock];

        let got: Lesson[];

        beforeEach(async () =>
        {
            got = await service.findAll();
        });

        test("should call lessonModel.find", () =>
        {
            expect(lessonModelMock.find).toHaveBeenCalled();
        });

        test("should return an array of lessons", () =>
        {
            expect(got).toEqual(expected);
        });
    });

    describe("create", () =>
    {
        const createLessonDto: CreateLessonDto = {
            ...lessonMock
        };

        const expected: Lesson = lessonMock;

        let got: Lesson;

        beforeEach(async () =>
        {
            got = await service.create(createLessonDto);
        });

        test("should return a created lesson", () =>
        {
            expect(got).toEqual(expected);
        });
    });
});
