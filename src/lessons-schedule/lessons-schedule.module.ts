import { Module } from '@nestjs/common';
import { TimetableModule } from './timetable/timetable.module';
import { LessonListModule } from './lesson-list/lesson-list.module';

@Module({
  imports: [TimetableModule, LessonListModule]
})
export class LessonsScheduleModule { }
