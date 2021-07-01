import { HubConnectionBuilder } from "@microsoft/signalr";

const connection = new HubConnectionBuilder()
  .withUrl("/MessagesHub", {
    accessTokenFactory: () => localStorage.getItem("token"),
  })
  .withAutomaticReconnect()
  .build();

async function start() {
  try {
    await connection.start();
  } catch (err) {
    setTimeout(start, 5000);
  }
}

connection.onclose(start);

export const startMessageHubConnection = async () => {
  await start();
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
