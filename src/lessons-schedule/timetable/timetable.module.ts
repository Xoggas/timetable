import { Module } from '@nestjs/common';
import { TimetableService } from './timetable.service';
import { TimetableGateway } from './timetable.gateway';
import { MongooseModule } from '@nestjs/mongoose';

@Module({
  imports: [
    MongooseModule.forRoot('mongodb://localhost:27017/lessons-timetable', {
      connectionName: 'lessons-timetable',
    })
  ],
  providers: [TimetableGateway, TimetableService],
})
export class TimetableModule { }
