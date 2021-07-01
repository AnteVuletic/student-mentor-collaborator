import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";

const connection = new HubConnectionBuilder()
  .withUrl("/MessagesHub", {
    accessTokenFactory: () => localStorage.getItem("token"),
  })
  .configureLogging(LogLevel.Debug)
  .withAutomaticReconnect()
  .build();

export const startMessageHubConnection = async () => {
  await connection.stop();
  await connection.start();
};

export const subscribeToMessageHub = (callback) => {
  connection.on("MessageRecieved", callback);
};

export const subscribeToCommentHub = (callback) => {
  connection.on("CommentRecieved", callback);
};

export const sendMessageToHub = async (message) => {
  const result = await connection.invoke("SendMessage", message);
  return result;
};

export const sendCommentToHub = async (comment) => {
  const result = await connection.invoke("SendComment", comment);
  return result;
};
