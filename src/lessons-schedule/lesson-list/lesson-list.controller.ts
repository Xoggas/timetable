import { Body, Controller, Delete, Get, Post, Put } from "@nestjs/common";
import { LessonListService } from "./lesson-list.service";
import { Lesson } from "./schemas/lesson-list.schema";
import { CreateLessonDto } from "./dto/create-lesson.dto";
import { UpdateLessonDto } from "./dto/update-lesson.dto";
import { DeleteLessonDto } from "./dto/delete-lesson.dto";

@Controller("lesson-list")
export class LessonListController
{
    constructor(private readonly lessonListService: LessonListService)
    {
    }

    @Get()
    findAll(): Promise<Lesson[]>
    {
        return this.lessonListService.findAll();
    }

    @Post()
    create(@Body() createLessonDto: CreateLessonDto): Promise<Lesson>
    {
        return this.lessonListService.create(createLessonDto);
    }

    @Put()
    update(@Body() updateLessonDto: UpdateLessonDto): Promise<Lesson>
    {
        return this.lessonListService.update(updateLessonDto);
    }

    @Delete()
    delete(@Body() deleteLessonDto: DeleteLessonDto): Promise<Lesson>
    {
        return this.lessonListService.delete(deleteLessonDto);
    }
}
