import { Entity, PrimaryGeneratedColumn, Column } from 'typeorm';
import { Field, Int, ObjectType } from 'type-graphql';
import { Min, Max } from "class-validator";

@Entity()
@ObjectType()
export class Product {
  @PrimaryGeneratedColumn()
  id: number;

  @Column("bigint", {unique: true})
  @Min(0)
  @Max(2**64)
  @Field(type => Int)
  identifier: number;

  @Column("text")
  @Field()
  name: string;

  @Column("text", {nullable: true})
  @Field({nullable: true})
  description?: string;
}
