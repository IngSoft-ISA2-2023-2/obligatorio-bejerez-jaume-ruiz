import { Component, OnInit } from '@angular/core';
import { cilCheckAlt, cilX } from '@coreui/icons';
import { DrugService } from '../../../services/drug.service';
import { Drug } from '../../../interfaces/drug';
import { CommonService } from '../../../services/CommonService';
import { ProductService } from 'src/app/services/product.service';

@Component({
  selector: 'app-delete-product',
  templateUrl: './delete-product.component.html',
  styleUrls: ['./delete-product.component.css'],
})
export class DeleteProductComponent implements OnInit {
  products: Drug[] = [];
  icons = { cilCheckAlt, cilX };
  targetItem: any = undefined;
  visible = false;
  modalTitle = '';
  modalMessage = '';

  constructor(
    private commonService: CommonService,
    private productService: ProductService
  ) {}

  ngOnInit(): void {
    this.getProductByUser();
  }

  getProductByUser() {
    this.productService.getProductByUser().subscribe((d: any) => (this.products = d));
  }

  deleteProduct(index: number): void {
    for (let prod of this.products) {
      if (prod.id === index) {
        this.targetItem = prod;
        break;
      }
    }
    if (this.targetItem) {
      this.modalTitle = 'Delete Product';
      this.modalMessage = `Deleting '${this.targetItem.code} - ${this.targetItem.name}'. Are you sure ?`;
      this.visible = true;
    }
  }

  closeModal(): void {
    this.visible = false;
  }

  saveModal(event: any): void {
    if (event) {
      this.productService.deleteProduct(this.targetItem.id).subscribe((p: any) => {
        if (p) {
          this.visible = false;
          this.getProductByUser();
          this.commonService.updateToastData(
            `Success deleting products "${this.targetItem.code} - ${this.targetItem.name}"`,
            'success',
            'Product deleted.'
          );
        }
      });
    }
  }
}
