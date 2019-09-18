import { Module } from '@nestjs/common';
import { ProductsService } from './products.service';
import { TypeOrmModule } from '@nestjs/typeorm';
import { Product } from './product.entity';
import { ProductsController } from './products.controller';
import { ProductsResolver } from './products.resolver'

@Module({
  imports: [
    TypeOrmModule.forFeature([Product])
  ],
  providers: [ProductsService, ProductsResolver],
  controllers: [ProductsController],
  exports: []//[TypeOrmModule]
})
export class ProductsModule {}
