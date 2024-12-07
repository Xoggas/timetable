import { Module } from "@nestjs/common";
import { LessonListService } from "./lesson-list.service";
import { LessonListController } from "./lesson-list.controller";
import { MongooseModule } from "@nestjs/mongoose";
import { Lesson, LessonSchema } from "./schemas/lesson-list.schema";

@Module({
    imports: [
        MongooseModule.forFeature([{ name: Lesson.name, schema: LessonSchema }])
    ],
    controllers: [LessonListController],
    providers: [LessonListService],
    exports: [LessonListService]
})
export class LessonListModule
{
}
