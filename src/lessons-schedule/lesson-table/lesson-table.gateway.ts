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
import { ValidatedMessageBody } from 'src/shared/decorators/validated-message-body.decorator';

@WebSocketGateway({ namespace: 'lessons-timetable' })
@UseFilters(new BadRequestTransformationFilter())
export class LessonTableGateway {
  constructor(private readonly lessonTableService: LessonTableService) {}

  @SubscribeMessage('update')
  handleLessonTableUpdate(
    @ValidatedMessageBody() updateLessonTableDto: UpdateLessonTableDto,
    @ConnectedSocket() socket: Socket,
  ): Promise<LessonTable> {
    const updatedTable = this.lessonTableService.update(updateLessonTableDto);
    socket.emit('update', updateLessonTableDto);
    return updatedTable;
  }
}
