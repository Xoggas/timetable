import { Controller, Get, Param, ParseEnumPipe } from '@nestjs/common';
import { DayOfWeek } from './types/DayOfWeek.type';
import { LessonTable } from './schemas/lesson-table.schema';
import { LessonTableService } from './lesson-table.service';

@Controller('lesson-table')
export class LessonTableController {
  constructor(private readonly lessonTableService: LessonTableService) {}

  @Get(':dayOfWeek')
  findByDayOfWeek(
    @Param('dayOfWeek', new ParseEnumPipe(DayOfWeek)) dayOfWeek: DayOfWeek,
  ): Promise<LessonTable> {
    return this.lessonTableService.findByDayOfWeek(dayOfWeek);
  }
}
