import { Component, inject } from '@angular/core';
import { AnalyticsService } from './analytics.service';
import { ChartConfiguration, ChartType } from 'chart.js';
import { HttpParams } from '@angular/common/http';


@Component({
  selector: 'app-analytics',
  templateUrl: './analytics.component.html',
  styleUrls: ['./analytics.component.scss']
  
})
export class AnalyticsComponent {
  startDate: string | null = '01/01/2015';
  endDate: string | null = '12/30/2015';

  summary: any;
  monthlySalesChartData: ChartConfiguration<'line'>['data'] = { labels: [], datasets: [] };
  topPizzasChartData: ChartConfiguration<'bar'>['data'] = { labels: [], datasets: [] };
  categoryChartData: ChartConfiguration<'pie'>['data'] = { labels: [], datasets: [] };

  lineChartType: ChartType = 'line';
  barChartType: ChartType = 'bar';
  pieChartType: ChartType = 'pie';

  chartOptions: ChartConfiguration['options'] = {
    responsive: true,
    plugins: {
      legend: { display: true },
      title: { display: false }
    }
  };

  analyticsService = inject(AnalyticsService)
  topPizzas: any;

  ngOnInit(): void {
    this.loadSalesSummary();
    this.loadSalesByCategory();
    this.loadTopFivePizza()
  }

  loadTopFivePizza() {
    this.analyticsService.getTopPizzas()
    .subscribe((data) => {
       this.topPizzasChartData = {
        labels: data.map((x: any) => x.name),
        datasets: [
          {
            data: data.map((x: any) => x.totalSold),
            label: 'Top Pizzas',
            backgroundColor: '#66BB6A'
          }
        ]
      };

      this.topPizzas = data[0]
    })
  }

  loadSalesSummary() {
    let params = new HttpParams();
    if (this.startDate) params = params.set('startDate', this.startDate);
    if (this.endDate) params = params.set('endDate', this.endDate);

    this.analyticsService.getSalesSummary(params).subscribe((data: any) => {
      this.summary = data;

      this.monthlySalesChartData = {
        labels: data.monthlySalesTrend.map((x: any) => new Date(x.date).toLocaleDateString()),
        datasets: [
          {
            data: data.monthlySalesTrend.map((x: any) => x.totalSales),
            label: 'Daily Sales',
            fill: true
          }
        ]
      };
    });
  }

  loadSalesByCategory() {
   this.analyticsService.getSalesByCategory().subscribe(data => {
      this.categoryChartData = {
        labels: data.map(x => x.category),
        datasets: [
          {
            data: data.map(x => x.totalRevenue),
            backgroundColor: ['#42A5F5', '#FFA726', '#66BB6A', '#EF5350', '#AB47BC']
          }
        ]
      };
    });
  }
}
