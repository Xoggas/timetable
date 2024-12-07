import { Prop, Schema, SchemaFactory } from "@nestjs/mongoose";

@Schema()
export class Lesson
{
    @Prop({ required: true })
    name: string;
}

export const LessonSchema = SchemaFactory.createForClass(Lesson);
