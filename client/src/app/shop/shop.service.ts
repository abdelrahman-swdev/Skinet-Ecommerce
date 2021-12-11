import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IBrand } from '../shared/models/brand';
import { IPagination } from '../shared/models/pagination';
import { IProductType } from '../shared/models/productType';
import { map } from 'rxjs/operators'
import { ShopParams } from '../shared/models/shopParams';
import { IProduct } from '../shared/models/product';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  baseUrl = environment.baseUrl;
  constructor(private http: HttpClient) {}

  getProducts(shopParams:ShopParams) : Observable<IPagination>{
    let params = new HttpParams();
    if(shopParams.brandId !== 0) params = params.append('brandId', shopParams.brandId.toString());
    if(shopParams.typeId !== 0) params = params.append('typeId', shopParams.typeId.toString());
    params = params.append('sort', shopParams.sort);
    params = params.append('pageIndex', shopParams.pageNumber.toString());
    params = params.append('pageSize', shopParams.pageSize.toString());
    if(shopParams.search) params = params.append('search', shopParams.search);
    return this.http.get<IPagination>(this.baseUrl.concat('products'), {observe: 'response', params})
    .pipe(
      map(response => {
        return response.body;
      })
    );
  }

  getProduct(id:number) : Observable<IProduct>
  {
    return this.http.get<IProduct>(this.baseUrl + 'products/' + id);
  }

  getBrands() : Observable<IBrand[]>
  {
    return this.http.get<IBrand[]>(this.baseUrl.concat('products/brands'));
  }

  getProductTypes() : Observable<IProductType[]>
  {
    return this.http.get<IProductType[]>(this.baseUrl.concat('products/types'));
  }

}
