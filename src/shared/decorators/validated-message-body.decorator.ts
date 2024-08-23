import { PipeTransform, Type, ValidationPipe } from '@nestjs/common';
import { MessageBody } from '@nestjs/websockets';

export function ValidatedMessageBody(
  ...pipes: (Type<PipeTransform> | PipeTransform)[]
): ParameterDecorator {
  return MessageBody(...pipes, new ValidationPipe());
}
