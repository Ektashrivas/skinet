import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Basket, IBasket, IBasketitem, IbasketTotals } from '../shared/models/basket';
import { IProduct } from '../shared/models/products';



@Injectable({
  providedIn: 'root'
})
export class BasketService {
  baseURL = environment.apiUrl;
  private basketSource = new BehaviorSubject<IBasket>(null);
  basket$ = this.basketSource.asObservable();
  private basketTotalSource = new BehaviorSubject<IbasketTotals>(null);
  baskettotals$ = this.basketTotalSource.asObservable();

  constructor(private http: HttpClient) { }
  getBasket(id: string) {
    return this.http.get(this.baseURL + 'basket?id=' + id)
      .pipe(
        map((basket: IBasket) => {
          this.basketSource.next(basket);
          this.calculateTotals();
        })
      );
  }

  setBasket(basket: IBasket) {
    this.http.post(this.baseURL + 'basket', basket).subscribe((response: IBasket) => {
      this.basketSource.next(response);
      this.calculateTotals();
    }, error => {
      console.log(error);
    })
  }

  getCurrentBasketValue() {
    return this.basketSource.value;
  }

  addItemToBasket(item: IProduct, quantity = 1) {
    const itemToAdd: IBasketitem = this.mapProductItemToBasketItem(item, quantity);
    const basket = this.getCurrentBasketValue() ?? this.createBasket();
    basket.items = this.addOrUpdateItem(basket.items, itemToAdd, quantity);
    this.setBasket(basket);
  }
  incrementItemQuantity(item: IBasketitem) {
    const basket = this.getCurrentBasketValue();
    const foundItemIndex = basket.items.findIndex(x => x.id === item.id);
    basket.items[foundItemIndex].quantity++;
    this.setBasket(basket);
  }
  decrementItemQuantity(item: IBasketitem) {
    const basket = this.getCurrentBasketValue();
    const foundItemIndex = basket.items.findIndex(x => x.id === item.id);
    if (basket.items[foundItemIndex].quantity > 1)
      basket.items[foundItemIndex].quantity--;
    else
      this.removeItemFromBasket(item);
  }
  removeItemFromBasket(item: IBasketitem) {
    const basket = this.getCurrentBasketValue();
    if (basket.items.some(x=> x.id === item.id))
        basket.items= basket.items.filter(i=>i.id !== item.id);
    if(basket.items.length >0)
     this.setBasket(basket);
     else{
       this.deleteBasket(basket);
     }
  }
  deleteBasket(basket: IBasket) {
    return this.http.delete(this.baseURL +'basket?id=' +basket.id).subscribe(()=>{
      this.basketSource.next(null);
      this.basketTotalSource.next(null);
      localStorage.removeItem('basket_id');
    }, error=>{
      console.log(error);
    });
  }
  private calculateTotals() {
    const basket = this.getCurrentBasketValue();
    const shipping = 0;
    const subtotal = basket.items.reduce((a, b) => (b.price * b.quantity) + a, 0);
    const total = subtotal + shipping;
    this.basketTotalSource.next({ shipping, total, subtotal });
  }
  private addOrUpdateItem(items: IBasketitem[], itemToAdd: IBasketitem, quantity: number): IBasketitem[] {
    const index = items.findIndex(i => i.id === itemToAdd.id);
    if (index === -1) {
      itemToAdd.quantity = quantity;
      items.push(itemToAdd);
    }
    else {
      items[index].quantity += quantity;
    }
    return items;
  }
  private createBasket(): IBasket {
    const basket = new Basket();
    localStorage.setItem('basket_id', basket.id);
    return basket;
  }

  mapProductItemToBasketItem(item: IProduct, quantity: number): IBasketitem {
    return {
      id: item.id,
      productName: item.name,
      price: item.price,
      pictureURL: item.pictureURL,
      quantity: quantity,
      brand: item.productBrand,
      type: item.productType
    }
  }
}
