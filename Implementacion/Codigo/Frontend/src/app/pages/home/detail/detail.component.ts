import { Component, OnInit} from '@angular/core';
import { cilCart, cilPlus, cilCompass } from '@coreui/icons';
import { IconSetService } from '@coreui/icons-angular';
import { ActivatedRoute } from '@angular/router';
import { Drug } from 'src/app/interfaces/drug';
import { DrugService } from '../../../services/drug.service';
import { StorageManager } from '../../../utils/storage-manager';
import { Router } from '@angular/router'; 
import { CommonService } from '../../../services/CommonService';

@Component({
  selector: 'app-detail',
  templateUrl: './detail.component.html',
  styleUrls: ['./detail.component.css'],
})
export class DetailComponent implements OnInit {
  drug: Drug | undefined;
  quantity: number = 1;
  cartD: Drug[] = [];

  constructor(
    private route: ActivatedRoute,
    public iconSet: IconSetService,
    private drugService: DrugService,
    private storageManager: StorageManager,
    private router: Router,
    private commonService: CommonService
  ) {
    iconSet.icons = { cilCart, cilPlus, cilCompass };
  }

  ngOnInit(): void {
    this.getDrug();
    this.storageManager.saveData('total', JSON.stringify(0));
  }

  getDrug(): void {
    const id = parseInt(this.route.snapshot.paramMap.get('id')!, 10);
    this.drugService.getDrug(id).subscribe((drug) => (this.drug = drug));
  }

  addToCart(drug: Drug) {
    if (drug) {
      this.cartD = JSON.parse(this.storageManager.getData('cartD'));
      if (!this.cartD) {
        this.cartD = [];
        this.storageManager.saveData('cartD', JSON.stringify(this.cartD));
      }
      
      let exist: boolean = false;
      for (let item of this.cartD) {
        if (item.code === drug.code && item.id === drug.id){
          item.quantity += this.quantity;
          exist = true;
          break;
        }
      }
      if (!exist){
        drug.quantity = this.quantity;
        this.cartD.push(drug);
      }
      this.storageManager.saveData('cartD', JSON.stringify(this.cartD));
    }
    this.updateHeader(this.cartD.length);
    this.router.navigate(['/home/cart']);
  }

  updateHeader(value: number){
    this.commonService.updateHeaderData(value);
   }

}
