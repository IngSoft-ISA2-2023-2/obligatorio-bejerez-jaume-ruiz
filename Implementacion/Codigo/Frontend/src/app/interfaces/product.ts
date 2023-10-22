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
    code: string;
    name: string;
    description: string;
    price: number;
    quantity: number;

    constructor(code: string, name: string, description: string, price: number, quantity: number){
      this.code= code;
      this.name = name;
      this.description = description;
      this.price = price;
      this.quantity= quantity;
    }
  }
