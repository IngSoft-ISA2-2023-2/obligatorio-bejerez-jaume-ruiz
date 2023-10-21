import { Component, OnInit } from '@angular/core';
import { cilBarcode, cilPencil, cilPaint, cilAlignCenter, cilDollar, cilLibrary, cilLoop1, cilTask, cilShortText } from '@coreui/icons';
import { AbstractControl, FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { Pharmacy } from '../../../interfaces/pharmacy';
import { UnitMeasureService } from '../../../services/unitmeasure.service';
import { UnitMeasure } from '../../../interfaces/unitmeasure';
import { PresentationService } from '../../../services/presentation.service';
import { Presentation } from '../../../interfaces/presentation';
import { DrugService } from '../../../services/drug.service';
import { ProductRequest } from '../../../interfaces/product';
import { CommonService } from '../../../services/CommonService';
import { ProductService } from 'src/app/services/product.service';

@Component({
  selector: 'app-create-product',
  templateUrl: './create-product.component.html',
  styleUrls: ['./create-product.component.css'],
})
export class CreateProductComponent implements OnInit {
  form: FormGroup | any;
  pharmacys: Pharmacy[] = [];
  unitMeasure: UnitMeasure[] = [];
  presentation: Presentation[] = [];

  icons = { cilBarcode, cilPencil, cilAlignCenter, cilLibrary,
    cilDollar, cilLoop1, cilTask, cilShortText, cilPaint };

  constructor(
    private commonService: CommonService,
    private productService: ProductService,
    private unitMeasureService: UnitMeasureService,
    private presentationService: PresentationService,
    private fb: FormBuilder
  ) {
    
    this.form = this.fb.group({
        name: new FormControl(),
        description: new FormControl(), 
        price: new FormControl(),
      });
  }

  ngOnInit(): void {
    this.getUnitMeasures();
    this.getPresentations();
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

  createProduct(): void {
    let name = this.name.value ? this.name.value : "";
    let description = this.description.value ? this.description.value : "";
    let price = this.price.value ? this.price.value : 0;

    let productRequest = new ProductRequest(name, description, price);
        this.productService.createProduct(productRequest).subscribe((prod) => {
        this.form.reset();
        this.setDefaultPresentation();
        this.setDefaultUnitMeasure();

        if (prod){
          this.commonService.updateToastData(
            `Success creating "${prod.name}"`,
            'success',
            'Drug created.'
          );
        }
      });

  }
}

