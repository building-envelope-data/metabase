import { Controller, Get, Query, Post, Body, Put, Param, Delete } from '@nestjs/common';
import { CreateProductDto, UpdateProductDto, ListProducts } from './dto';
import { Product } from './product.entity'
import { ProductsService } from './products.service'

@Controller('products')
export class ProductsController {
  constructor(private readonly productsService: ProductsService) {}

  @Get()
  async list(@Query() query: ListProducts): Promise<Product[]> {
    return this.productsService.list(query);
  }

  @Get(':identifier')
  async find(@Param('identifier') identifier: string): Promise<Product> {
    return this.productsService.find(identifier);
  }

  @Post()
  async create(@Body() createProductDto: CreateProductDto): Promise<string> {
    return this.productsService.create(createProductDto);
  }

  @Put(':identifier')
  async update(@Param('identifier') identifier: string, @Body() updateProductDto: UpdateProductDto): Promise<any> { // TODO Is `any` what we want here? Or rather the unit type `[]`
    return this.productsService.update(identifier, updateProductDto);
  }

  @Delete(':identifier')
  async delete(@Param('identifier') identifier: string): Promise<any> { // TODO Is `any` what we want here?
    return this.productsService.delete(identifier);
  }
}
