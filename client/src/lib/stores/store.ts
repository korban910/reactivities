import CounterStore from "./counterStore.ts";
import { createContext } from "react";
import UiStore from "./uiStore.ts";
import ActivityStore from "./activityStore.ts";

type Store = {
  counterStore: CounterStore,
  uiStore: UiStore,
  activityStore: ActivityStore,
}

const store: Store = {
  counterStore: new CounterStore(),
  uiStore: new UiStore(),
  activityStore: new ActivityStore(),
}

const StoreContext = createContext(store);

export {
  StoreContext,
  UiStore,
  store
}