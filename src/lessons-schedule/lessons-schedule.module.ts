import { Module } from '@nestjs/common';
import { LessonListModule } from './lesson-list/lesson-list.module';
import { LessonTableModule } from './lesson-table/lesson-table.module';

@Module({
  imports: [LessonListModule, LessonTableModule],
})
export class LessonsScheduleModule {}
