import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
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

  constructor(private route:ActivatedRoute, 
    private shopService:ShopService, 
    private breadcrumbService:BreadcrumbService)
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

}
