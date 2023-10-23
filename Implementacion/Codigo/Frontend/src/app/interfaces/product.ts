export interface Product {
    id: number;
    code: string;
    name: string;
    description: string;
    price: number;
    quantity: number;
    pharmacy: {
      id: number;
      name: string;  
    };
  }

  export class ProductRequest {
    name: string;
    description: string;
    price: number;

    constructor(name: string, description: string, price: number){
      this.name = name;
      this.description = description;
      this.price = price;
    }
  }
