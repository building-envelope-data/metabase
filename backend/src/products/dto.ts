import { IsString, IsInt } from 'class-validator';

export class ListProducts {
  @IsInt()
  readonly limit?: number;
}

export class CreateProductDto {
  @IsString()
  readonly name: string;

  @IsString()
  readonly description?: string;
}

export class UpdateProductDto {
  @IsString()
  readonly name: string;

  @IsString()
  readonly description?: string;
}
