import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment.development';

@Injectable()
export class AnalyticsService {
  baseUrl = environment.apiUrl;
  http = inject(HttpClient);

  getTopPizzas(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}insights/top-pizzas`);
  }

  getSalesByCategory(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}insights/sales-by-category`);
  }

  getSalesSummary(params: HttpParams): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}insights/sales-summary`, { params });
  }
}
