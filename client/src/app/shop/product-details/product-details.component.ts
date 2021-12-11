import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BasketService } from 'src/app/basket/basket.service';
import { IProduct } from 'src/app/shared/models/product';
import { BreadcrumbService } from 'xng-breadcrumb';
import { ShopService } from '../shop.service';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss']
})
export class ProductDetailsComponent implements OnInit {

  product:IProduct;
  quantity = 1;

  constructor(private route:ActivatedRoute, 
    private shopService:ShopService, 
    private breadcrumbService:BreadcrumbService,
    private basketService:BasketService)
  {
    this.breadcrumbService.set('@productDetails', ' ');
  }

  ngOnInit(): void {
    this.loadProduct();
  }

  loadProduct()
  {
    this.shopService.getProduct(+this.route.snapshot.paramMap.get('id')).subscribe(prod => {
      this.product = prod;
      this.breadcrumbService.set('@productDetails', this.product.name);
    },error => {
      console.error(error);
    });
  }

  decreamentQuantity()
  {
    if(this.quantity > 1)
    {
      this.quantity--;
    }
  }

  increamentQuantity()
  {
    this.quantity++;
  }

  addToCart()
  {
    this.basketService.addItemToBasket(this.product, this.quantity);
  }

}
