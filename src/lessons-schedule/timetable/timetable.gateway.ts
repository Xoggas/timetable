import { MessageBody, SubscribeMessage, WebSocketGateway } from '@nestjs/websockets';
import { TimetableService } from './timetable.service';

@WebSocketGateway()
export class TimetableGateway {
  constructor(private readonly timetableService: TimetableService) { }
}
