import { Component, OnInit } from '@angular/core';
import { IOrder } from 'src/app/shared/models/order';
import { OrdersService } from '../orders.service';

@Component({
  selector: 'app-orders-list',
  templateUrl: './orders-list.component.html',
  styleUrls: ['./orders-list.component.scss']
})
export class OrdersListComponent implements OnInit {

  orders:IOrder[];

  constructor(private ordersService:OrdersService) { }

  ngOnInit(): void {
    this.getOrders();
  }

  getOrders()
  {
    this.ordersService.getOrdersForCurrentUser().subscribe((orders) => {
      this.orders = orders;
    }, error => {
      console.error(error);
    });
  }

}
