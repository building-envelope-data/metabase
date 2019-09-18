import { BadRequestException, PipeTransform, ArgumentMetadata, Injectable } from '@nestjs/common';

@Injectable()
export class RenderIdentifierPipe implements PipeTransform<number> {
  async transform(value: number, metadata: ArgumentMetadata): Promise<string> {
    if (!Number.isInteger(value)) {
      throw new BadRequestException(
        `The number '${value}' is not an integer`,
      );
    }
    if (value < -(2**63) || value > 2**63 - 1) {
      throw new BadRequestException(
        `The number '${value}' is not a 'bigint', that is, in the range [-2^63, 2^63 - 1]`,
      );
    }
    return (value + 2**63).toString(16)
  }
}
