import { Module } from '@nestjs/common';
import { LessonsScheduleModule } from './lessons-schedule/lessons-schedule.module';
import { BellsScheduleModule } from './bells-schedule/bells-schedule.module';
import { MongooseModule } from '@nestjs/mongoose';

@Module({
  imports: [
    MongooseModule.forRoot('mongodb://localhost:27017/timetable'),
    LessonsScheduleModule, 
    BellsScheduleModule
  ]
})
export class AppModule { }
