import { Component, OnInit } from '@angular/core';
import { AccountService } from './account/account.service';
import { BasketService } from './basket/basket.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  title = 'SkiNet';
  constructor(private basketService:BasketService, private accService:AccountService) {}

  ngOnInit(): void {
    this.loadBasket();
    this.loadCurrentUser();
  }

  loadCurrentUser()
  {
    const token = localStorage.getItem('token');
    this.accService.loadCurrentUser(token).subscribe(() => {
    }, error => {
      console.log(error);
    });
  }

  loadBasket()
  {
    const basketId = localStorage.getItem('basket_id');
    if(basketId)
    {
      this.basketService.getBasket(basketId).subscribe(() => {
        console.log('initialized');
      }, error => {
        console.log(error);
      });
    }
  }

}
