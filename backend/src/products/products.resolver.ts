import { Args, Mutation, Query, Resolver } from '@nestjs/graphql';
import { CreateProductDto, UpdateProductDto, ListProducts } from './dto';
import { Product } from './product.entity'
import { ProductsService } from './products.service'
// import { RenderIdentifierPipe } from './render-identifier.pipe' // Can be used like `@Args('id', ParseIntPipe)`

@Resolver(of => Product)
export class ProductsResolver {
  constructor(private readonly productsService: ProductsService) {}

  // @Query(returns => [Product], { name: 'products' })
  // async list(@Args('query') query: ListProducts): Promise<Product[]> {
  //   return this.productsService.list(query);
  // }

  @Query(returns => Product, { name: 'product' })
  async find(@Args('identifier') identifier: string): Promise<Product> {
    return this.productsService.find(identifier);
  }

  // @Mutation(returns => String, { name: 'create-product' })
  // async create(@Args('product') dto: CreateProductDto): Promise<string> {
  //   return this.productsService.create(dto);
  // }

  // @Mutation()
  // async update(@Args('identifier') identifier: string, @Args('product') dto: UpdateProductDto): Promise<any> { // TODO Is `any` what we want here? Or rather the unit type `[]`
  //   return this.productsService.update(identifier, dto);
  // }

  // @Mutation()
  // async delete(@Args('identifier') identifier: string): Promise<any> { // TODO Is `any` what we want here?
  //   return this.productsService.delete(identifier);
  // }
}
