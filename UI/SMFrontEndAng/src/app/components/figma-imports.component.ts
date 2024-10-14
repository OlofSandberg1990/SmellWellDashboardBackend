import { Component } from "@angular/core";
import { CommonModule } from "@angular/common";

import { BuilderPage } from "./builder-page.component";

@Component({
  selector: "app-figma-imports",
  standalone: true,
  imports: [BuilderPage, CommonModule],
  template: ` <app-builder-page [model]="'figma-imports'"></app-builder-page> `,
})
export class FigmaImportsPage {
  title = "figma imports";
}
