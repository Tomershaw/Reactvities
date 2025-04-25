import {
  HubConnection,
  HubConnectionBuilder,
  LogLevel,
} from "@microsoft/signalr";
import { ChatComment } from "../models/comment";
import { makeAutoObservable, runInAction } from "mobx";
import { store } from "./store";

// Manages comments and SignalR hub connection for real-time chat
export default class commentStore {
  // List of chat comments
  comments: ChatComment[] = [];
  // SignalR hub connection instance
  hubConnection: HubConnection | null = null;

  constructor() {
    makeAutoObservable(this);
  }

  // Initializes the SignalR hub connection for a specific activity
  createHubConnection = (activityId: string) => {
    if (store.activityStore.selectedActivity) {
      this.hubConnection = new HubConnectionBuilder()
        .withUrl(import.meta.env.VITE_CHAT_URL + "?activityId=" + activityId, {
          accessTokenFactory: () => store.userStore.user?.token as string,
        })
        .withAutomaticReconnect()
        .configureLogging(LogLevel.Information)
        .build();

      this.hubConnection
        .start()
        .catch(error =>
          console.log("Error establishing the connection:", error)
        );

      // Loads initial comments from the server
      this.hubConnection.on("LoadComments", (comments: ChatComment[]) => {
        runInAction(() => {
          comments.forEach(comment => {
            comment.createAt = new Date(comment.createAt);
          });
          this.comments = comments;
        });
      });

      // Receives new comments in real-time
      this.hubConnection.on("ReceiveComment", (comment: ChatComment) => {
        runInAction(() => {
          comment.createAt = new Date(comment.createAt);
          this.comments.unshift(comment);
        });
      });
    }
  };

  // Stops the SignalR hub connection
  stopHubConnection = () => {
    this.hubConnection
      ?.stop()
      .catch(error => console.log("Error stopping connection:", error));
  };

  // Clears the comments and stops the hub connection
  clearComments = () => {
    this.comments = [];
    this.stopHubConnection();
  };

  // Sends a new comment to the server
  addComment = async (values: { body: string; activityId?: string }) => {
    values.activityId = store.activityStore.selectedActivity?.id;
    try {
      await this.hubConnection?.invoke("SendComment", values);
    } catch (error) {
      console.log(error);
    }
  };
}
