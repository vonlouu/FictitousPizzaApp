import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AnalyticsComponent } from './analytics.component';
import { AnalyticsRoutingModule } from './analytics.routing.module';
import { HttpClientModule } from '@angular/common/http';
import { AnalyticsService } from './analytics.service';
import { BaseChartDirective } from 'ng2-charts';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    AnalyticsComponent
  ],
  imports: [
    CommonModule,
    AnalyticsRoutingModule,
    BaseChartDirective,
    FormsModule
  ],
  providers: [AnalyticsService]
})
export class AnalyticsModule { }
