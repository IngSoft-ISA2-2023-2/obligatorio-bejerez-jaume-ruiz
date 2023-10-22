import { Component, OnInit } from '@angular/core';
import { cilCart, cilPlus, cilCompass } from '@coreui/icons';
import { IconSetService } from '@coreui/icons-angular';
import { Drug } from '../../../interfaces/drug';
import { DrugService } from '../../../services/drug.service';
import { CommonService } from '../../../services/CommonService';
import { StorageManager } from 'src/app/utils/storage-manager';
import { ProductService } from 'src/app/services/product.service';
import { Product } from 'src/app/interfaces/product';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  drugs: Drug[] = [];
  products: Product[] = [];
  cart: any[] = [];
  
  constructor(
    public iconSet: IconSetService,
    private drugService: DrugService,
    private productService: ProductService,
    private commonService: CommonService,
    private storageManager: StorageManager) {
    iconSet.icons = { cilCart, cilPlus, cilCompass };

    this.commonService.onSearchDataUpdate.subscribe((data: any) => {
      this.drugs = data;
      this.products = data;
    });
  }

  ngOnInit(): void {
    this.updateCart();
    this.getDrugs();
    this.getProducts();
    this.storageManager.saveData('total', JSON.stringify(0));
  }

  getDrugs(): void {
    this.drugService.getDrugs()
    .subscribe(drugs => this.drugs = drugs);
  }

  getProducts(): void {
    this.productService.getProducts()
    .subscribe(products => this.products = products);
  }

  updateCart(): void {
    this.cart = JSON.parse(this.storageManager.getData('cart'));
    if (!this.cart) {
      this.cart = [];
      this.storageManager.saveData('cart', JSON.stringify(this.cart));
    }
    this.commonService.updateHeaderData(this.cart.length);
  }
}
