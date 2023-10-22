import { Component, OnInit } from '@angular/core';
import { cilCart, cilPlus, cilCompass, cilCheckCircle, cilTrash } from '@coreui/icons';
import { IconSetService } from '@coreui/icons-angular';
import { Drug } from 'src/app/interfaces/drug';
import { StorageManager } from '../../../utils/storage-manager';
import { Router } from '@angular/router';
import { CommonService } from '../../../services/CommonService';
import { Product } from 'src/app/interfaces/product';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {
  cartD: Drug[] = [];
  cartP: Product[] = [];
  total: number = 0;

  constructor(
    public iconSet: IconSetService,
    private storageManager: StorageManager,
    private router: Router,
    private commonService: CommonService) {
    iconSet.icons = { cilCart, cilPlus, cilCompass, cilCheckCircle, cilTrash };
  }

  ngOnInit(): void {
    this.cartD = JSON.parse(this.storageManager.getData('cartD'));
    if (!this.cartD) {
      this.cartD = [];
      this.storageManager.saveData('cartD', JSON.stringify(this.cartD));
    }
    this.cartP = JSON.parse(this.storageManager.getData('cartP'));
    if (!this.cartP) {
      this.cartP = [];
      this.storageManager.saveData('cartP', JSON.stringify(this.cartP));
    }
    this.storageManager.saveData('total', JSON.stringify(0));
    this.updateTotal();
  }

  deleteD(index: number){
    this.cartD = JSON.parse(this.storageManager.getData('cartD'));
    this.cartD.splice(index, 1);
    this.storageManager.saveData('cartD', JSON.stringify(this.cartD));
    this.updateTotal();
    this.updateHeader(this.cartD.length);
  }

  deleteP(index: number){
    this.cartP = JSON.parse(this.storageManager.getData('cartP'));
    this.cartP.splice(index, 1);
    this.storageManager.saveData('cartP', JSON.stringify(this.cartP));
    this.updateTotal();
    this.updateHeader(this.cartP.length);
  }

  updateTotal(){
    this.total = 0;
    this.cartD = JSON.parse(this.storageManager.getData('cartD'));
    this.cartP = JSON.parse(this.storageManager.getData('cartP'));
    for(let item of this.cartD){
      this.total += (item.price * item.quantity);
    }
    for(let item of this.cartP){
      this.total += item.price;
    }
  }

  updateHeader(value: number){
    this.commonService.updateHeaderData(value);
   }

  goToCho(){
    this.storageManager.saveData('total', JSON.stringify(this.total));
    this.router.navigate(['/home/cart/cho']);
  }
  
}
