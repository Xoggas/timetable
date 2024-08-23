import { Prop, Schema, SchemaFactory } from '@nestjs/mongoose';
import mongoose from 'mongoose';
import { DayOfWeek } from '../types/DayOfWeek.type';
import { Lesson } from 'src/lessons-schedule/lesson-list/schemas/lesson-list.schema';

@Schema()
export class LessonTable {
  @Prop({
    type: String,
    required: true,
  })
  dayOfWeek: DayOfWeek;

  @Prop({
    type: [[{ type: mongoose.Schema.Types.ObjectId, ref: 'Lesson' }]],
    required: true,
  })
  lessons: Lesson[][];
}

export const LessonTableSchema = SchemaFactory.createForClass(LessonTable);
