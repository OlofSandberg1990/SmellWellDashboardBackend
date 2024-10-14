import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { KeywordSalesComponent } from './components/keyword-sales/keyword-sales.component';
import { BuilderPage } from './components/builder-page.component';
import { OrganicRankingComponent } from './components/organic-ranking/organic-ranking.component';
import { SalesOverviewComponent } from './components/sales-overview/sales-overview.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, KeywordSalesComponent, OrganicRankingComponent, BuilderPage, SalesOverviewComponent],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'SMFrontEndAng';
}
