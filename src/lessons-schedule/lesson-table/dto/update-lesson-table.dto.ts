import { Type } from 'class-transformer';
import { DayOfWeek } from '../types/DayOfWeek.type';
import { IsArray, IsDefined, IsEnum, ValidateNested } from 'class-validator';
import { LessonDto } from './lesson.dto';

export class UpdateLessonTableDto {
  @IsDefined()
  @IsEnum(DayOfWeek)
  readonly dayOfWeek: DayOfWeek;

  @IsDefined()
  @IsArray()
  @ValidateNested({ each: true })
  @Type(() => LessonDto)
  readonly lessons: LessonDto[][];
}
