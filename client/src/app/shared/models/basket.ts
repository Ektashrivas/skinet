import { v4 as uuid } from 'uuid';

export interface IBasket {
    id: string;
    items: IBasketitem[];
}

export interface IBasketitem {
    id: number;
    productName: string;
    price: number;
    quantity: number;
    pictureURL: string;
    brand: string;
    type: string;
}

export class Basket implements IBasket {
    id = uuid();
    items: IBasketitem[] = [];

}

export interface IbasketTotals {
    shipping: number;
    subtotal: number;
    total: number;
}


