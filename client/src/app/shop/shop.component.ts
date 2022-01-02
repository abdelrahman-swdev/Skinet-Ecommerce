import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { IBrand } from '../shared/models/brand';
import { IPagination } from '../shared/models/pagination';
import { IProduct } from '../shared/models/product';
import { IProductType } from '../shared/models/productType';
import { ShopParams } from '../shared/models/shopParams';
import { ShopService } from './shop.service';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit {
  products: IProduct[];
  brands:IBrand[];
  productTypes:IProductType[];
  shopParams: ShopParams;
  totalCount:number;
  sortOptions = [
    {name: "Alphabetical", value:"name"},
    {name: "Price: Low to High", value:"priceAsc"},
    {name: "Price: High to Low", value:"priceDesc"}
  ];
  @ViewChild('search') searchTerm: ElementRef;

  constructor(private shopService: ShopService) { 
    this.shopParams = this.shopService.shopParams;
  }

  ngOnInit(): void {
    this.getProducts(true);
    this.getBrands();
    this.getProductTypes();
  }
  
  getProducts(useCache = false)
  {

    this.shopService.getProducts(useCache).subscribe((response) => {
      this.products = response.data;
      this.totalCount = response.count;
    }, error => {
      console.error(error);
    });
  }

  getBrands()
  {
    this.shopService.getBrands().subscribe((response) => {
      this.brands = [{id:0, name:'All'}, ...response];
    }, error => {
      console.error(error);
    });
  }

  getProductTypes()
  {
    this.shopService.getProductTypes().subscribe((response) => {
      this.productTypes = [{id:0, name:'All'}, ...response];
    }, error => {
      console.error(error);
    });
  }

  onBrandSelected(brandId:number){
    const params = this.shopService.getShopParams();
    params.brandId = brandId;
    params.pageNumber = 1;
    this.shopService.setShopParams(params);
    this.getProducts();
  }

  onTypeSelected(typeId:number){
    const params = this.shopService.getShopParams();
    params.typeId = typeId;
    params.pageNumber = 1;
    this.shopService.setShopParams(params);
    this.getProducts();
  }

  onSortSelected(sort:string)
  {
    const params = this.shopService.getShopParams();
    params.sort = sort;
    this.shopService.setShopParams(params);
    this.getProducts();
  }

  onPageChanged(event:number)
  {
    const params = this.shopService.getShopParams();
    if(params.pageNumber !== event)
    {
      params.pageNumber = event;
      this.shopService.setShopParams(params);
      this.getProducts(true);
    }
  }

  onSearch()
  {
    const params = this.shopService.getShopParams();
    params.search = this.searchTerm.nativeElement.value;
    params.pageNumber = 1;
    this.shopService.setShopParams(params);
    this.getProducts();
  }

  onReset()
  {
    this.searchTerm.nativeElement.value = '';
    this.shopParams = new ShopParams();
    this.shopService.setShopParams(this.shopParams);
    this.getProducts();
  }

}
