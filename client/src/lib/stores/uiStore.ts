import { makeAutoObservable } from "mobx";

class UiStore {
  isLoading = false;

  constructor() {
    makeAutoObservable(this);
  }

  isBusy = () => {
    this.isLoading = true;
  }

  isIdle = () => {
    this.isLoading = false;
  }
}

export default UiStore;