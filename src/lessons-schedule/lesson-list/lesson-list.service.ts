import { Injectable, NotFoundException } from "@nestjs/common";
import { Lesson } from "./schemas/lesson-list.schema";
import mongoose, { Model } from "mongoose";
import { InjectModel } from "@nestjs/mongoose";
import { CreateLessonDto } from "./dto/create-lesson.dto";
import { UpdateLessonDto } from "./dto/update-lesson.dto";
import { DeleteLessonDto } from "./dto/delete-lesson.dto";

@Injectable()
export class LessonListService
{
    constructor(
      @InjectModel(Lesson.name) private readonly lessonModel: Model<Lesson>
    )
    {
    }

    async findAll(): Promise<Lesson[]>
    {
        return this.lessonModel.find().exec();
    }

    async create(createLessonDto: CreateLessonDto): Promise<Lesson>
    {
        const lesson = new this.lessonModel(createLessonDto);
        return lesson.save();
    }

    async update(updateLessonDto: UpdateLessonDto): Promise<Lesson>
    {
        const isValidId = mongoose.isValidObjectId(updateLessonDto.id);

        if (isValidId == false)
        {
            throw new NotFoundException(
              `The lesson with id ${updateLessonDto.id} wasn't found.`
            );
        }

        return await this.lessonModel
          .findByIdAndUpdate(
            { _id: updateLessonDto.id },
            { name: updateLessonDto.name }
          )
          .exec();
    }

    async delete(deleteLessonDto: DeleteLessonDto): Promise<Lesson>
    {
        return this.lessonModel.findByIdAndDelete({
            _id: deleteLessonDto.id
        });
    }
}
