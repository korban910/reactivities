import { useLocalObservable } from "mobx-react-lite";
import { HubConnection, HubConnectionBuilder, HubConnectionState } from "@microsoft/signalr";
import { useEffect, useRef } from "react";
import { runInAction } from "mobx";

const useComments = (activityId?: string) => {
  const created = useRef(false);
  const commentStore = useLocalObservable(() => ({
    hubConnection: null as HubConnection | null,
    comments: [] as ChatComment[],

    createHubConnection(activityId: string){
      if (!activityId) {
        return;
      }

      this.hubConnection = new HubConnectionBuilder()
        .withUrl(`${import.meta.env.VITE_COMMENT_URL}?activityId=${activityId}`, {
          withCredentials: true,
        })
        .withAutomaticReconnect()
        .build();

      this.hubConnection.start().catch(error => console.log('Error establishing connection', error));

      this.hubConnection.on(import.meta.env.VITE_SIGNALR_CONNECTION_METHOD_NAME, (comments) => {
        runInAction(() => {
          this.comments = comments;
        })
      })

      this.hubConnection.on(import.meta.env.VITE_SIGNALR_RECEIVE_METHOD_NAME, (comment) => {
        runInAction(() => {
          this.comments.unshift(comment); // put top of the list
        })
      })
    },

    stopHubConnection(){
      if (this.hubConnection?.state === HubConnectionState.Connected) {
        this.hubConnection.stop().catch(error => console.log('Error establishing connection', error));
      }
    }
  }));

  useEffect(() => {
    if (activityId && !created.current) {
      commentStore.createHubConnection(activityId);
      created.current = true;
    }
    return () => {
      commentStore.stopHubConnection();
      commentStore.comments = [];
    }
  }, [activityId, commentStore]);

  return {
    commentStore
  }
}

export {
  useComments,
}