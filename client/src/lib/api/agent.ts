import axios from "axios";
import { store } from "../stores/store";
import { toast } from "react-toastify";
import router from "../../router/Routes";

type ApiErrorResponse = {
  errors?: Record<string, string[]>;
};

const sleep = (delay: number) =>
  new Promise(resolve => setTimeout(resolve, delay));

const agent = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
  withCredentials: true,
});

const errorHandlers: Record<
  number,
  (data: ApiErrorResponse) => Promise<void> | void
> = {
  400: (data) => {
    if (data?.errors) {
      throw Object.values(data.errors).flat();
    }
    toast.error("Bad request");
  },
  401: () => {
    toast.error("Unauthorized");
  },
  404: () => {
    router.navigate("/not-found");
  },
  500: (data) => {
    router.navigate("/server-error", { state: { error: data } });
  },
};

agent.interceptors.request.use(config => {
  store.uiStore.isBusy();
  return config;
});

agent.interceptors.response.use(
  async response => {
    await sleep(1000);
    store.uiStore.isIdle();
    return response;
  },
  async error => {
    await sleep(1000);
    store.uiStore.isIdle();

    const { status, data } = error.response ?? {};
    const handler = status ? errorHandlers[status] : undefined;

    if (handler) {
      await handler(data);
    }

    return Promise.reject(error);
  }
);

export default agent;
