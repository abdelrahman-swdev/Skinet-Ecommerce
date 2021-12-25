import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { IOrder } from '../shared/models/order';

@Injectable({
  providedIn: 'root'
})
export class OrdersService {

  baseUrl = environment.baseUrl;

  constructor(private http: HttpClient) { }

  getOrdersForCurrentUser()
  {
    return this.http.get<IOrder[]>(this.baseUrl + 'order');
  }

  getOrderByIdForCurrentUser(orderId: number)
  {
    return this.http.get<IOrder>(this.baseUrl + 'order/' + orderId);
  }
}
