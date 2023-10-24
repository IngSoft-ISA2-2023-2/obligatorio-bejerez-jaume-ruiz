import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { environment } from '../../environments/environment';
import { CommonService } from './CommonService';
import { StorageManager } from '../utils/storage-manager';
import { Product, ProductRequest } from '../interfaces/product';


@Injectable({ providedIn: 'root' })
export class ProductService {
    
  private url = environment.apiUrl + '/api/product';

  httpOptions = {
    headers: new HttpHeaders()
     .set('Content-Type', 'application/json')
     .set('Authorization', 'f4522298-a723-4c47-ad43-594f09eeae66')
  };

  constructor(
    private http: HttpClient,
    private commonService: CommonService,
    private storageManager: StorageManager) { }

  getHttpHeaders(): HttpHeaders {
    let login = JSON.parse(this.storageManager.getLogin());
    let token = login ? login.token : "";
    
    return new HttpHeaders()
      .set('Content-Type', 'application/json')
      .set('Authorization', token);
  }

    /** GET product by id. Will 404 if id not found */
    getProduct(id: number): Observable<Product> {
      const url = `${this.url}/${id}`;
      return this.http.get<Product>(url, {headers: this.getHttpHeaders() })
      .pipe(
        tap(),
        catchError(this.handleError<Product>(`Get Product id=${id}`))
      );
    }

    /** GET product from the server */
    getProducts(): Observable<Product[]> {
        return this.http.get<Product[]>(this.url, {headers: this.getHttpHeaders() })
          .pipe(
            tap(),
            catchError(this.handleError<Product[]>('Get Products', []))
          );
      }

      /** POST Create product */
      createProduct(prod: ProductRequest): Observable<Product> {
        return this.http.post<Product>(this.url, prod, {headers: this.getHttpHeaders() })
        .pipe(
          tap(),
          catchError(this.handleError<Product>('Create Product'))
        );
      }

    /** DELETE Delete deleteProduct */
  deleteProduct(id: number): Observable<any> {
    const url = `${this.url}/${id}`;
    return this.http.delete<any>(url, {headers: this.getHttpHeaders() })
    .pipe(
      tap(),
      catchError(this.handleError<any>('Delete Product'))
    );
  }

  modifyProduct(targetItem: Product, productRequest: ProductRequest) {
    const url = `${this.url}/${targetItem.id}`;
    return this.http.put<ProductRequest>(url, productRequest, {headers: this.getHttpHeaders() })
    .pipe(
      tap(),
      catchError(this.handleError<Product>('Modify Product'))
    );
   }

      getProductByUser(): Observable<Product[]> {
        const url = `${this.url}/user`;
        return this.http.get<Product[]>(url, {headers: this.getHttpHeaders() })
          .pipe(
            tap(),
            catchError(this.handleError<Product[]>('Get Products By User', []))
          );
      }

      private handleError<T>(operation = 'operation', result?: T) {
        return (error: any): Observable<T> => {
    
          // TODO: send the error to remote logging infrastructure
          //console.error(error); // log to console instead
    
          // TODO: better job of transforming error for user consumption
          this.log(`${operation} failed: ${error.error.message}`);
    
          // Let the app keep running by returning an empty result.
          return of(result as T);
        };
      }
    
        /** Log a DrugService error with the MessageService */
        private log(message: string) {
            this.commonService.updateToastData(message, "danger", "Error");
       }

}