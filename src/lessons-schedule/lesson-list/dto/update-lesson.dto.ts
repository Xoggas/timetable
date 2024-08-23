import { IsMongoId, IsNotEmpty, IsString } from 'class-validator';

export class UpdateLessonDto {
  @IsNotEmpty()
  @IsMongoId()
  readonly id: string;

  @IsNotEmpty()
  @IsString()
  readonly name: string;
}
