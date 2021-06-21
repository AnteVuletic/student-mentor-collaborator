import {
  Box,
  debounce,
  Grid,
  LinearProgress,
  Typography,
  useScrollTrigger,
} from "@material-ui/core";
import axios from "axios";
import React, { useContext, useEffect, useState } from "react";

import MessageSender from "../../../components/MessageSender";
import Message from "../../../components/Message";
import { MessageContext } from "../../../services/providers/MessageProvider";
import { SentimentDissatisfied } from "@material-ui/icons";

const MessageWall = () => {
  const scrolledToEndTrigger = useScrollTrigger();
  const [isLoading, setIsLoading] = useState(true);
  const [mentor, setMentor] = useState({});
  const {
    state: { isEndOfMessages, isMessageLoading, messages },
    loadMessagePage,
  } = useContext(MessageContext);

  useEffect(() => {
    axios
      .get("api/Mentor/GetMentor")
      .then((res) => {
        setMentor(res.data);
      })
      .finally(() => {
        setIsLoading(false);
      });
  }, []);

  useEffect(() => {
    if (scrolledToEndTrigger && !isEndOfMessages) {
      debounce(loadMessagePage, 1000)();
    }
  }, [scrolledToEndTrigger, loadMessagePage, isEndOfMessages]);

  useEffect(() => {
    loadMessagePage();
  }, []);

  if (isLoading) return <LinearProgress />;

  return (
    <Grid container justify="center" spacing={2}>
      <MessageSender mentor={mentor} />
      <Grid item xs={6}>
        {messages.map((m) => (
          <Box key={m.id} mt={2}>
            <Message message={m} />
          </Box>
        ))}
      </Grid>
      {isMessageLoading && <LinearProgress />}

      {isEndOfMessages && (
        <Grid container item xs={12} alignContent="center" justify="center">
          <Typography variant="subtitle2">No more messages</Typography>
          <SentimentDissatisfied />
        </Grid>
      )}
    </Grid>
  );
};

export default MessageWall;
