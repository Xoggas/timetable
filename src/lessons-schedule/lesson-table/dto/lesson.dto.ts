import { IsMongoId, IsNotEmpty } from 'class-validator';

export class LessonDto {
  @IsNotEmpty()
  @IsMongoId()
  readonly _id: string;
}
