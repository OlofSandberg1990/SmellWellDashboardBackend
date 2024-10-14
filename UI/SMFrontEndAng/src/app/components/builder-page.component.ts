import { Component, Input } from "@angular/core";
import { fetchOneEntry, type BuilderContent } from "@builder.io/sdk-angular";
import { Content } from "@builder.io/sdk-angular";
import { CommonModule } from "@angular/common";
import { environment } from "../../environments/environment";
import { CUSTOM_COMPONENTS } from "../builder-registry";

@Component({
  selector: "app-builder-page",
  standalone: true,
  imports: [Content, CommonModule],
  template: `
    <ng-container *ngIf="content; else notFound">
      <builder-content
        [model]="model"
        [content]="content"
        [apiKey]="apiKey"
        [customComponents]="customComponents"
      ></builder-content>
    </ng-container>

    <ng-template #notFound>
      <div>404 - Content not found</div>
    </ng-template>
  `,
})
export class BuilderPage {
  title = "builder page";

  @Input() model: string = "page"; // Default model is 'page'

  apiKey = environment.builderApiKey;

  content: BuilderContent | null = null;

  customComponents = CUSTOM_COMPONENTS;

  async ngOnInit() {
    const urlPath = window.location.pathname || "/";

    const builderContent = await fetchOneEntry({
      model: this.model,
      apiKey: this.apiKey,
      userAttributes: {
        urlPath,
      },
    });

    if (!builderContent) {
      return;
    }

    this.content = builderContent;
  }
}
