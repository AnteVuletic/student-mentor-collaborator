import axios from "axios";
import React, { useState, createContext, useCallback, useEffect } from "react";
import {
  subscribeToMessageHub,
  startMessageHubConnection,
} from "../../utils/hubs";

const initialState = {
  messages: [],
  isEndOfMessages: false,
  isMessageLoading: false,
};

export const MessageContext = createContext({
  state: { ...initialState },
  loadMessagePage: () => {},
});
const pageSize = 5;

const MessageProvider = ({ children }) => {
  const [messages, setMessages] = useState([]);
  const [isEndOfMessages, setIsEndOfMessages] = useState(false);
  const [isMessageLoading, setIsMessageLoading] = useState(false);
  const [page, setPage] = useState(0);

  useEffect(() => {
    startMessageHubConnection().then(() =>
      subscribeToMessageHub((response) => {
        setMessages((prev) => [response.data, ...prev]);
      })
    );
  }, [setMessages]);

  const loadMessagePage = useCallback(() => {
    setIsMessageLoading(true);
    axios
      .get(`api/Message`, { params: { page, pageSize } })
      .then((response) => {
        setIsEndOfMessages(response.data.length < pageSize);

        setMessages((m) => [...m, ...response.data]);
        setPage(page + 1);
      })
      .finally(() => {
        setIsMessageLoading(false);
      });
  }, [page, setPage, setMessages, setIsEndOfMessages]);

  const value = {
    state: { messages, isEndOfMessages, isMessageLoading },
    loadMessagePage,
  };

  return (
    <MessageContext.Provider value={value}>{children}</MessageContext.Provider>
  );
};

export default MessageProvider;
