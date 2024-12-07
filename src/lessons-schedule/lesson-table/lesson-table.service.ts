import { Injectable } from "@nestjs/common";
import { InjectModel } from "@nestjs/mongoose";
import { LessonTable } from "./schemas/lesson-table.schema";
import { Model } from "mongoose";
import { DayOfWeek } from "./types/DayOfWeek.type";
import { UpdateLessonTableDto } from "./dto/update-lesson-table.dto";
import { Lesson } from "../lesson-list/schemas/lesson-list.schema";

@Injectable()
export class LessonTableService
{
    constructor(
      @InjectModel(LessonTable.name)
      private readonly lessonTableModel: Model<LessonTable>,
      @InjectModel(Lesson.name) private readonly lessonModel: Model<Lesson>
    )
    {
    }

    async findByDayOfWeek(dayOfWeek: DayOfWeek): Promise<LessonTable>
    {
        return this.lessonTableModel
          .findOneAndUpdate({ dayOfWeek: dayOfWeek }, {}, { upsert: true })
          .populate("lessons", {}, this.lessonModel)
          .exec();
    }

    async update(updateLessonTableDto: UpdateLessonTableDto)
    {
        return this.lessonTableModel
          .findOneAndUpdate(
            { dayOfWeek: updateLessonTableDto.dayOfWeek },
            updateLessonTableDto,
            { upsert: true }
          )
          .populate("lessons", {}, this.lessonModel)
          .exec();
    }
}
