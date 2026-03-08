import { useContext } from "react";
import { StoreContext } from "../stores/store.ts";

const useStore = () => {
  return useContext(StoreContext);
}

export default useStore;