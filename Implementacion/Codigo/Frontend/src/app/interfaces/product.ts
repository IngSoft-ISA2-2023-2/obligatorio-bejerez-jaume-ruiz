export interface Product {
    id: number;
    name: string;
    description: string;
    price: number;
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
