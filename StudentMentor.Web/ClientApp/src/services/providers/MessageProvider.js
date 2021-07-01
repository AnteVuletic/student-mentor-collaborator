import axios from "axios";
import React, { useState, createContext, useEffect } from "react";
import {
  subscribeToMessageHub,
  startMessageHubConnection,
  subscribeToCommentHub,
} from "../../utils/hubs";

const initialState = {
  messages: [],
  isEndOfMessages: false,
  isMessageLoading: false,
  studentFilterId: 0,
};

export const MessageContext = createContext({
  state: { ...initialState },
  handleStudentFilterId: () => {},
  setPage: () => {},
});
const pageSize = 10;

const MessageProvider = ({ children }) => {
  const [messages, setMessages] = useState([]);
  const [studentFilterId, setStudentFilterId] = useState(0);
  const [isEndOfMessages, setIsEndOfMessages] = useState(false);
  const [isMessageLoading, setIsMessageLoading] = useState(false);
  const [page, _setPage] = useState(0);

  useEffect(() => {
    startMessageHubConnection().then(() => {
      subscribeToMessageHub((response) => {
        if (!studentFilterId) {
          setMessages((prev) => [response.data, ...prev]);
          return;
        }

        if (
          studentFilterId &&
          (response.data.userFrom.id === studentFilterId ||
            response.data.userTo.id === studentFilterId)
        ) {
          setMessages((prev) => [response.data, ...prev]);
        }
      });
      subscribeToCommentHub((response) => {
        setMessages((prev) => {
          const prevCopied = [...prev];
          const messageIndex = prevCopied.findIndex(
            (m) => m.id == response.messageId
          );
          prevCopied[messageIndex].comments = [
            ...prevCopied[messageIndex].comments,
            response,
          ];

          return prevCopied;
        });
      });
    });
  }, [setMessages, studentFilterId]);

  useEffect(() => {
    setIsMessageLoading(true);
    let params = { page, pageSize };

    if (studentFilterId) {
      params = { ...params, studentId: studentFilterId };
    }

    axios
      .get(`api/Message`, {
        params: params,
      })
      .then((response) => {
        setIsEndOfMessages(response.data.length < pageSize);
        setMessages((m) => [...m, ...response.data]);
      })
      .finally(() => {
        setIsMessageLoading(false);
      });
  }, [studentFilterId, page]);

  const handleStudentFilterId = (event) => {
    const newFilterStudentId = event.target.value;
    setMessages([]);
    _setPage(0);
    setStudentFilterId(newFilterStudentId);
  };

  const value = {
    state: { messages, isEndOfMessages, isMessageLoading, studentFilterId },
    handleStudentFilterId,
    setPage: _setPage,
  };

  return (
    <MessageContext.Provider value={value}>{children}</MessageContext.Provider>
  );
};

export default MessageProvider;
