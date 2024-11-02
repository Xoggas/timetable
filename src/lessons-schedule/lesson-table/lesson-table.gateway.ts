import {
  ConnectedSocket,
  SubscribeMessage,
  WebSocketGateway,
} from '@nestjs/websockets';
import { UpdateLessonTableDto } from './dto/update-lesson-table.dto';
import { Socket } from 'socket.io';
import { LessonTableService } from './lesson-table.service';
import { LessonTable } from './schemas/lesson-table.schema';
import { UseFilters } from '@nestjs/common';
import { BadRequestTransformationFilter } from 'src/shared/filters/bad-request-transformation.filter';
import { ValidateMessageBody } from 'src/shared/decorators/validate-message-body.decorator';

@WebSocketGateway({ namespace: 'lessons-timetable' })
@UseFilters(new BadRequestTransformationFilter())
export class LessonTableGateway {
  constructor(private readonly lessonTableService: LessonTableService) {}

  @SubscribeMessage('update')
  handleLessonTableUpdate(
    @ValidateMessageBody() updateLessonTableDto: UpdateLessonTableDto,
    @ConnectedSocket() socket: Socket,
  ): Promise<LessonTable> {
    const updatedTable = this.lessonTableService.update(updateLessonTableDto);
    socket.emit('update', updateLessonTableDto);
    return updatedTable;
  }
}
