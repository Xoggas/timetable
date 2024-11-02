import { IsMongoId, IsNotEmpty } from 'class-validator';

export class DeleteLessonDto {
  @IsMongoId()
  @IsNotEmpty()
  readonly id: string;
}
