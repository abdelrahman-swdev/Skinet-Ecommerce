import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AccountService } from '../account/account.service';
import { BasketService } from '../basket/basket.service';
import { IAddress } from '../shared/models/address';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss']
})
export class CheckoutComponent implements OnInit {

  checkoutForm:FormGroup;

  constructor(private fb:FormBuilder, 
    private accService:AccountService,
    private basketService:BasketService) { }

  ngOnInit(): void {
    this.createCheckoutForm();
    this.getAddress();
    this.getDeliveryMethodValue();
  }

  createCheckoutForm()
  {
    this.checkoutForm = this.fb.group({
      addressForm: this.fb.group({
        firstName: ['', Validators.required],
        lastName: ['', Validators.required],
        street: ['', Validators.required],
        city: ['', Validators.required],
        state: ['', Validators.required],
        zipCode: ['', Validators.required]
      }),

      deliveryForm: this.fb.group({
        deliveryMethod: ['', Validators.required]
      }),

      paymentForm: this.fb.group({
        nameOnCard: ['', Validators.required]
      })
    });
  }

  getAddress()
  {
    this.accService.getUserAddress().subscribe(address => {
      if(address){
        this.checkoutForm.get('addressForm').patchValue(address);
      }
    }, error => {
      console.error(error);
    })
  }

  getDeliveryMethodValue()
  {
    const basket = this.basketService.getCurrentBasketValue();
    if(basket.deliveryMethodId !== null)
    {
      this.checkoutForm.get('deliveryForm').get('deliveryMethod').patchValue(basket.deliveryMethodId.toString());
    }
  }
}

