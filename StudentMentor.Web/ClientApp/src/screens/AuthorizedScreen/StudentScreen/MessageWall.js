import { Box, Grid, LinearProgress, Typography } from "@material-ui/core";
import axios from "axios";
import React, { useContext, useEffect, useState } from "react";

import MessageSender from "../../../components/MessageSender";
import Message from "../../../components/Message";
import { MessageContext } from "../../../services/providers/MessageProvider";
import { SentimentDissatisfied } from "@material-ui/icons";
import useIntersect from "../../../utils/useInteresect";

const MessageWall = () => {
  const [setNode, entry] = useIntersect({
    threshold: 1,
    rootMargin: "0px",
  });
  const [isLoading, setIsLoading] = useState(true);
  const [mentor, setMentor] = useState({});
  const {
    state: { isEndOfMessages, isMessageLoading, messages },
    setPage,
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
    if (entry.isIntersecting && !isEndOfMessages) {
      setPage((prev) => prev + 1);
    }
  }, [entry, isEndOfMessages, setPage]);

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

      <Grid item xs={12} ref={setNode}></Grid>
    </Grid>
  );
};

export default MessageWall;
