import { IsMongoId, IsNotEmpty, IsString } from 'class-validator';

export class UpdateLessonDto {
  @IsMongoId()
  @IsNotEmpty()
  readonly id: string;

  @IsNotEmpty()
  @IsString()
  readonly name: string;
}
