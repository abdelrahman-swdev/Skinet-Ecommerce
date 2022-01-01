import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import {
  Basket,
  IBasket,
  IBasketItem,
  IBasketTotals,
} from '../shared/models/basket';
import { IDeliveryMethod } from '../shared/models/deliveryMethod';
import { IProduct } from '../shared/models/product';

@Injectable({
  providedIn: 'root',
})
export class BasketService {
  baseUrl = environment.baseUrl;
  private basketSource = new BehaviorSubject<IBasket>(null);
  basket$ = this.basketSource.asObservable();
  private basketTotalsSource = new BehaviorSubject<IBasketTotals>(null);
  basketTotals$ = this.basketTotalsSource.asObservable();
  shipping = 0;

  constructor(private http: HttpClient) {}

  setShippingPrice(dm:IDeliveryMethod)
  {
    this.shipping = dm.price;
    const basket = this.getCurrentBasketValue();
    basket.deliveryMethodId = dm.id;
    basket.shippingPrice = dm.price;
    this.calculateTotals();
    this.setBasket(basket);
  }

  createPaymentIntent()
  {
    return this.http.post(this.baseUrl + 'payment/' + this.getCurrentBasketValue().id, {})
    .pipe(
      map((basket:IBasket) => {
        this.basketSource.next(basket);
      })
    );
  }

  // get basket by id from api
  getBasket(id: string) {
    return this.http.get(this.baseUrl + 'basket?id=' + id).pipe(
      map((basket: IBasket) => {
        this.basketSource.next(basket);
        this.shipping = basket.shippingPrice;
        this.calculateTotals();
      })
    );
  }

  // set basket in api
  setBasket(basket: IBasket) {
    this.http.post(this.baseUrl + 'basket', basket).subscribe(
      (response: IBasket) => {
        this.basketSource.next(response);
        this.calculateTotals();
      },
      (error) => {
        console.log(error);
      }
    );
  }

  // get current basket value from basketSource
  getCurrentBasketValue() {
    return this.basketSource.value;
  }

  // add item with its quantity to the current basket
  addItemToBasket(item: IProduct, quantity = 1) {
    const itemToAdd: IBasketItem = this.mapProductItemToBasketItem(
      item,
      quantity
    );
    const basket = this.getCurrentBasketValue() ?? this.createBasket();
    basket.items = this.addOrUpdateItemInBasket(
      basket.items,
      itemToAdd,
      quantity
    );
    this.setBasket(basket);
  }

  // map IProduct To IBasketItem
  mapProductItemToBasketItem(item: IProduct, quantity: number): IBasketItem {
    return {
      id: item.id,
      brand: item.productBrand,
      type: item.productType,
      pictureUrl: item.pictureUrl,
      price: item.price,
      quantity,
      productName: item.name,
    };
  }

  // increment Item Quantity
  incrementItemQuantity(item:IBasketItem)
  {
    const basket = this.getCurrentBasketValue();
    const foundItemIndex = basket.items.findIndex(i => i.id === item.id);
    basket.items[foundItemIndex].quantity++;
    this.setBasket(basket);
  }

  // decrement Item Quantity
  decrementItemQuantity(item:IBasketItem)
  {
    const basket = this.getCurrentBasketValue();
    const foundItemIndex = basket.items.findIndex(i => i.id === item.id);
    if(basket.items[foundItemIndex].quantity > 1)
    {
      basket.items[foundItemIndex].quantity--;
    }else {
      this.removeItemFromBasket(item);
    }
    this.setBasket(basket);
  }

  // remove Item From Basket
  removeItemFromBasket(item: IBasketItem) {
    const basket = this.getCurrentBasketValue();
    if(basket.items.some(x => x.id === item.id))
    {
      basket.items = basket.items.filter(x => x.id !== item.id);
      if(basket.items.length > 0)
      {
        this.setBasket(basket);
      }
      else
      {
        this.deleteBasket(basket);
      }
    }
  }

  // delete Basket from api
  deleteBasket(basket: IBasket) {
    this.http.delete(this.baseUrl + 'basket?id=' + basket.id).subscribe(() => {
      this.basketSource.next(null);
      this.basketTotalsSource.next(null);
      localStorage.removeItem('basket_id');
    }, error => {
      console.log(error);
    })
  }

  deleteBasketFromClient()
  {
    this.basketSource.next(null);
    this.basketTotalsSource.next(null);
    localStorage.removeItem('basket_id');
  }

  // to check if the item is already exist we increase the quantity only
  private addOrUpdateItemInBasket(
    items: IBasketItem[],
    itemToAdd: IBasketItem,
    quantity: number
  ): IBasketItem[] {
    const index = items.findIndex((i) => i.id === itemToAdd.id);
    if (index === -1) {
      itemToAdd.quantity = quantity;
      items.push(itemToAdd);
    } else {
      items[index].quantity += quantity;
    }
    return items;
  }

  // create basket if current basket value from basketSource is null
  private createBasket(): IBasket {
    const basket = new Basket();
    localStorage.setItem('basket_id', basket.id);
    return basket;
  }

  // calculate total price of current basket
  private calculateTotals() {
    const basket = this.getCurrentBasketValue();
    const subtotal = basket.items.reduce((prev, current) => (current.price * current.quantity) + prev, 0);
    const shipping = this.shipping;
    const total = subtotal + shipping;
    this.basketTotalsSource.next({shipping, subtotal, total});
  }
}
