import { Component, OnInit } from '@angular/core';
import { cilCheckAlt, cilX } from '@coreui/icons';
import { CommonService } from '../../../services/CommonService';
import { ProductService } from 'src/app/services/product.service';
import { Product } from 'src/app/interfaces/product';
import { cilBarcode, cilPencil, cilPaint, cilAlignCenter, cilDollar, cilLibrary, cilLoop1, cilTask, cilShortText } from '@coreui/icons';
import { AbstractControl, FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { Pharmacy } from '../../../interfaces/pharmacy';
import { UnitMeasureService } from '../../../services/unitmeasure.service';
import { UnitMeasure } from '../../../interfaces/unitmeasure';
import { PresentationService } from '../../../services/presentation.service';
import { Presentation } from '../../../interfaces/presentation';
import { DrugService } from '../../../services/drug.service';
import { ProductRequest } from '../../../interfaces/product';

@Component({
  selector: 'app-modify-product',
  templateUrl: './modify-product.component.html',
  styleUrls: ['./modify-product.component.css'],
})
export class ModifyProductComponent implements OnInit {
  products: Product[] = [];
  icons = { cilCheckAlt, cilX, cilBarcode, cilPencil, cilAlignCenter, cilLibrary,
    cilDollar, cilLoop1, cilTask, cilShortText, cilPaint};
  targetItem: any = undefined;
  showForm: boolean = false;
  visible = false;
  modalTitle = '';
  modalMessage = '';
  form: FormGroup | any;
  productToModify: Product | any;
  unitMeasure: UnitMeasure[] = [];
  presentation: Presentation[] = [];



  constructor(
    private commonService: CommonService,
    private productService: ProductService,
    private unitMeasureService: UnitMeasureService,
    private presentationService: PresentationService,
    private fb: FormBuilder,
    
  ) {
    this.form = this.fb.group({
        name: new FormControl(),
        description: new FormControl(), 
        price: new FormControl(),
      });


      
  }

  ngOnInit(): void {
    this.getProductByUser();

  }

  getUnitMeasures(): void {
    this.unitMeasureService
    .getUnitMeasure()
    .subscribe((unit_measure) => {
      this.unitMeasure = unit_measure;
      this.setDefaultUnitMeasure();
    });
  }

  setDefaultUnitMeasure(): void {
    let unit = this.unitMeasure.length > 0 ? this.unitMeasure[0].id : 0;
    this.form.controls.unitMeasureControl.setValue(unit);
  }

  getPresentations(): void {
    this.presentationService
    .getPresentations()
    .subscribe((presentation) => {
      this.presentation = presentation;
      this.setDefaultPresentation();
    });
  }

  setDefaultPresentation(): void {
    let p = this.presentation.length > 0 ? this.presentation[0].id : 0;
    this.form.controls.presentationControl.setValue(p);

  }

  get name(): AbstractControl {
    return this.form.controls.name;
  }

  get description(): AbstractControl {
    return this.form.controls.description;
  }
  
  get price(): AbstractControl {
    return this.form.controls.price;
  }

  getProductByUser() {
    this.productService.getProductByUser().subscribe((d: any) => (this.products = d));
  }

  modifyProduct(): void {
    this.targetItem = this.productToModify;
    if (this.targetItem) {
        let name = this.name.value ? this.name.value : "";
        let description = this.description.value ? this.description.value : "";
        let price = this.price.value ? this.price.value : 0;

    let productRequest = new ProductRequest(name, description, price);
        this.productService.modifyProduct(this.targetItem,productRequest).subscribe((prod) => {
        this.form.reset();
        this.showForm = false;
        if(prod){
          this.commonService.updateToastData(
            `Success modifying product "${this.productToModify.name}"`,
            'success',
            'Product modified.'
          );
        }

      });

    }
  }


  showProductForm(product: Product) {
    this.productToModify = product;
    this.showForm = true;
  }
  
  hideProductForm() {
    this.showForm = false;
  }

  preventClick(event: Event) {
    if (this.showForm) {
      event.preventDefault();
      event.stopPropagation();
    }
  }

}
