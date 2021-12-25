import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { BasketService } from 'src/app/basket/basket.service';
import { IOrder } from 'src/app/shared/models/order';
import { BreadcrumbService } from 'xng-breadcrumb';
import { OrdersService } from '../orders.service';

@Component({
  selector: 'app-order-details',
  templateUrl: './order-details.component.html',
  styleUrls: ['./order-details.component.scss']
})
export class OrderDetailsComponent implements OnInit {

  order:IOrder;

  constructor(private ordersService:OrdersService, 
    private activatedRoute:ActivatedRoute, 
    private breadcrumbService:BreadcrumbService) { 
      this.breadcrumbService.set('@orderDetails', ' ');
    }

  ngOnInit(): void {
    this.getOrder();
    
  }

  getOrder()
  {
    this.ordersService.getOrderByIdForCurrentUser(+this.activatedRoute.snapshot.paramMap.get('id'))
    .subscribe((order) => {
      this.order = order;
      this.breadcrumbService.set('@orderDetails', `Order# ${this.order.id} - ${this.order.status}`);
    }, error => {
      console.error(error);
    });
  }

}
