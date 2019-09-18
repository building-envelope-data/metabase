import { Injectable } from '@nestjs/common';
import { InjectRepository } from '@nestjs/typeorm';
import { Repository } from 'typeorm';
import { Product } from './product.entity';
import { CreateProductDto, UpdateProductDto, ListProducts } from './dto';

// TODO Use the library `CRUD` instead? https://docs.nestjs.com/recipes/crud
@Injectable()
export class ProductsService {
  constructor(
    @InjectRepository(Product)
    private readonly productRepository: Repository<Product>,
  ) {}

  async list(query: ListProducts): Promise<Product[]> {
    return this.productRepository.find({take: query.limit});
  }

  async find(identifier: string): Promise<Product> {
    return this.productRepository.findOneOrFail({identifier: this.parseIdentifier(identifier)});
  }

  async create(dto: CreateProductDto): Promise<string> {
    let identifier: number = await this.getRandomIdentifier();
    return this.productRepository.insert({identifier: identifier, name: dto.name, description: dto.description}).then(insertResult => {return this.renderIdentifier(identifier);});
  }

  async update(identifier: string, dto: UpdateProductDto): Promise<any> {
    return this.productRepository.update({identifier: this.parseIdentifier(identifier)}, dto);
  }

  async delete(identifier: string): Promise<any> {
    return this.productRepository.delete({identifier: this.parseIdentifier(identifier)});
  }

  private parseIdentifier(identifier: string): number {
    return parseInt(identifier, 16) - 2**63; // TODO Do not hard-code `63`, that is, the precision of identifiers less 1
  }

  private renderIdentifier(identifier: number): string {
    return (identifier + 2**63).toString(16)
  }

  private async getRandomIdentifier(): Promise<number> {
    let identifier: number = 0;
    do {
      identifier = this.getRandomInt(2**64) - 2**63; // TODO Do not hardcode the number of bits `64` here. Use a constant instead!
    }
    while (await this.productRepository.count({identifier: identifier}) == 0);
    return identifier;
  }

  // https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Math/random
  private getRandomInt(exclusiveMax: number): number {
    return Math.floor(Math.random() * Math.floor(exclusiveMax));
  }
}
