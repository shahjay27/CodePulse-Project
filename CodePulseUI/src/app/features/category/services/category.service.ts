import { Injectable } from '@angular/core';
import { AddCategoryRequest } from '../models/add-category-request.model';
import { Observable } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Category } from '../models/category.model';
import { environment } from 'src/environments/environment';
import { UpdateCategoryRequest } from '../models/update-category-request.model';
import { CookieService } from 'ngx-cookie-service';

@Injectable({
  providedIn: 'root',
})
export class CategoryService {
  constructor(private http: HttpClient, private cookieService: CookieService) { }

  getAllCategories(query?: string, sortBy?: string, sortDirection?: string, pageNumer?: number, pageSize?: number): Observable<Category[]> {
    let params = new HttpParams();

    if (query) {
      params = params.set('query', query);
    }

    if (sortBy && sortDirection) {
      params = params.set('sortBy', sortBy);
    }

    if (sortDirection) {
      params = params.set('sortDirection', sortDirection);
    }

    if (pageNumer) {
      params = params.set('pageNumber', pageNumer);
    }

    if (pageSize) {
      params = params.set('pageSize', pageSize);
    }

    return this.http.get<Category[]>(
      `${environment.apiBaseUrl}/api/Categories`, {
      params: params
    }
    );
  }

  getCategoryById(id: string): Observable<Category> {
    return this.http.get<Category>(
      `${environment.apiBaseUrl}/api/Categories/${id}`
    );
  }

  getCategoryCount(): Observable<number> {
    return this.http.get<number>(
      `${environment.apiBaseUrl}/api/Categories/count`
    );
  }

  addCategory(model: AddCategoryRequest): Observable<void> {
    return this.http.post<void>(
      `${environment.apiBaseUrl}/api/Categories?addAuth=true`,
      model
    );
  }

  updateCategory(
    id: string,
    updateCategoryReq: UpdateCategoryRequest
  ): Observable<Category> {
    return this.http.put<Category>(
      `${environment.apiBaseUrl}/api/Categories/${id}?addAuth=true`,
      updateCategoryReq
    );
  }

  deleteCategory(id: string): Observable<Category> {
    return this.http.delete<Category>(
      `${environment.apiBaseUrl}/api/Categories/${id}?addAuth=true`
    );
  }
}
