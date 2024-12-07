import { Module } from "@nestjs/common";
import { LessonTableService } from "./lesson-table.service";
import { MongooseModule } from "@nestjs/mongoose";
import { LessonTable, LessonTableSchema } from "./schemas/lesson-table.schema";
import { LessonTableController } from "./lesson-table.controller";
import {
    Lesson,
    LessonSchema
} from "../lesson-list/schemas/lesson-list.schema";
import { LessonTableGateway } from "./lesson-table.gateway";

@Module({
    imports: [
        MongooseModule.forFeature([
            { name: LessonTable.name, schema: LessonTableSchema }
        ]),
        MongooseModule.forFeature([{ name: Lesson.name, schema: LessonSchema }])
    ],
    providers: [LessonTableService, LessonTableGateway],
    controllers: [LessonTableController]
})
export class LessonTableModule
{
}
