import type { RegisteredComponent } from "@builder.io/sdk-angular";
import { Counter } from "./components/counter.component";
import { FigmaImportsPage } from "./components/figma-imports.component";

export const CUSTOM_COMPONENTS: RegisteredComponent[] = [
  {
    component: Counter,
    name: "Counter",
    inputs: [
      {
        name: "initialCount",
        type: "number",
      },
    ],
  },
  {
    component: FigmaImportsPage,
    name: "FigmaImportsPage",
  },
];
