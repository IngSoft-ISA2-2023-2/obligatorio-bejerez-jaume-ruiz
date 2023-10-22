import { Component, OnInit} from '@angular/core';
import { cilCart, cilPlus, cilCompass } from '@coreui/icons';
import { IconSetService } from '@coreui/icons-angular';
import { ActivatedRoute } from '@angular/router';
import { Drug } from 'src/app/interfaces/drug';
import { DrugService } from '../../../services/drug.service';
import { StorageManager } from '../../../utils/storage-manager';
import { Router } from '@angular/router'; 
import { CommonService } from '../../../services/CommonService';
import { Product } from 'src/app/interfaces/product';
import { ProductService } from 'src/app/services/product.service';

@Component({
  selector: 'app-detail',
  templateUrl: './detail.component.html',
  styleUrls: ['./detail.component.css'],
})
export class DetailComponent implements OnInit {
  prod: Product | undefined;
  quantity: number = 1;
  cartP: Product[] = [];

  constructor(
    private route: ActivatedRoute,
    public iconSet: IconSetService,
    private productService: ProductService,
    private storageManager: StorageManager,
    private router: Router,
    private commonService: CommonService
  ) {
    iconSet.icons = { cilCart, cilPlus, cilCompass };
  }

  ngOnInit(): void {
    this.getProduct();
    this.storageManager.saveData('total', JSON.stringify(0));
  }

  getProduct(): void {
    const id = parseInt(this.route.snapshot.paramMap.get('id')!, 10);
    this.productService.getProduct(id).subscribe((prod) => (this.prod = prod));
  }

  addToCart(prod: Product) {
    if (prod) {
      this.cartP = JSON.parse(this.storageManager.getData('cartP'));
      if (!this.cartP) {
        this.cartP = [];
        this.storageManager.saveData('cartP', JSON.stringify(this.cartP));
      }
      
      let exist: boolean = false;
      for (let item of this.cartP) {
        if (item.id === prod.id){
          item.quantity += this.quantity;
          exist = true;
          break;
        }
      }
      if (!exist){
        prod.quantity = this.quantity;
        this.cartP.push(prod);
      }
      this.storageManager.saveData('cartP', JSON.stringify(this.cartP));
    }
    this.updateHeader(this.cartP.length);
    this.router.navigate(['/home/cart']);
  }

  updateHeader(value: number){
    this.commonService.updateHeaderData(value);
   }

}
