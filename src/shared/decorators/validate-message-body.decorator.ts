import { PipeTransform, Type, ValidationPipe } from '@nestjs/common';
import { MessageBody } from '@nestjs/websockets';

export function ValidateMessageBody(
  ...pipes: (Type<PipeTransform> | PipeTransform)[]
): ParameterDecorator {
  return MessageBody(...pipes, new ValidationPipe());
}
