import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { FormControl } from '@angular/forms';
import { debounceTime, delay, distinctUntilChanged } from 'rxjs/operators';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
})
export class DashboardComponent implements OnInit {
  orders: any[] = [];
  loading: boolean = true;
  searchTerm = new FormControl('');
  currentPage: number = 1;
  pageSize: number = 5;
  pageSizeOptions: number[] = [5, 10, 25, 50, 100];
  totalItems: number = 0;
  totalPages: number = 1;
  sortBy: string = 'OrderDate';
  ascending: boolean = true;
  Math = Math;
  showDropdown = false;

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.loadOrders();

    this.searchTerm.valueChanges
      .pipe(debounceTime(500), distinctUntilChanged())
      .subscribe(() => {
        this.currentPage = 1;
        this.loadOrders();
      });
  }

  loadOrders(): void {
    this.loading = true;

    let params = new HttpParams()
      .set('page', this.currentPage.toString())
      .set('pageSize', this.pageSize.toString())
      .set('sortBy', this.sortBy)
      .set('ascending', this.ascending.toString());

    if (this.searchTerm.value) {
      params = params.set('q', this.searchTerm.value);
    }

    this.http
      .get<any>('https://localhost:7045/api/orders', { params })
      .pipe(delay(500))
      .subscribe({
        next: (response) => {
          this.orders = response.items;
          this.totalItems = response.totalItems;
          this.totalPages = Math.ceil(this.totalItems / this.pageSize);
          this.loading = false;
        },
        error: (error) => {
          console.error('Error loading orders:', error);
          this.loading = false;
        },
      });
  }

  changePage(page: number): void {
    if (page >= 1 && page <= this.totalPages && page !== this.currentPage) {
      this.currentPage = page;
      this.loadOrders();
    }
  }

  changePageSize(size: number): void {
    this.pageSize = size;
    this.currentPage = 1;
    this.showDropdown = !this.showDropdown;
    this.loadOrders();
  }

  getPages(): number[] {
    const pages: number[] = [];
    const maxVisiblePages = 5;
    let startPage = Math.max(
      1,
      this.currentPage - Math.floor(maxVisiblePages / 2)
    );
    const endPage = Math.min(this.totalPages, startPage + maxVisiblePages - 1);

    startPage = Math.max(1, endPage - maxVisiblePages + 1);

    for (let i = startPage; i <= endPage; i++) {
      pages.push(i);
    }
    return pages;
  }

  toggleDropdown(): void {
    this.showDropdown = !this.showDropdown;
  }

  getOrderDateTime(order: any): Date {
    const time = order?.orderTime?.split(":")

    const orderDate = new Date(order?.orderDate)
    if (time && time.length > 0) {
      orderDate.setHours(time[0], time[1], time[2])
    }

    return orderDate
  }
}
