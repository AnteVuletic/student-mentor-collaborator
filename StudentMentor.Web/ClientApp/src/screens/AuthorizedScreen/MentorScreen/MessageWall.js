import { Box, Grid, LinearProgress, Typography } from "@material-ui/core";
import axios from "axios";
import React, { useContext, useEffect, useState } from "react";

import MessageSender from "../../../components/MessageSender";
import Message from "../../../components/Message";
import { MessageContext } from "../../../services/providers/MessageProvider";
import { FormLabel, Select, MenuItem } from "@material-ui/core";
import { SentimentDissatisfied } from "@material-ui/icons";
import useIntersect from "../../../utils/useInteresect";

const MessageWall = () => {
  const [setNode, entry] = useIntersect({
    threshold: 1,
    rootMargin: "0px",
  });
  const [isLoading, setIsLoading] = useState(true);
  const [students, setStudents] = useState([]);
  const {
    state: { isEndOfMessages, isMessageLoading, messages, studentFilterId },
    setPage,
    handleStudentFilterId,
  } = useContext(MessageContext);

  useEffect(() => {
    axios
      .get("api/Student/GetMentoringStudents")
      .then((res) => {
        setStudents(res.data);
      })
      .finally(() => {
        setIsLoading(false);
      });
  }, []);

  useEffect(() => {
    if (entry.isIntersecting && !isEndOfMessages) {
      setPage((prev) => prev + 1);
    }
  }, [setPage, entry, isEndOfMessages]);

  if (isLoading) return <LinearProgress />;

  return (
    <Grid container justify="center" spacing={2}>
      <MessageSender students={students} />
      <Grid item justify="center" container xs={12}>
        <Grid item xs={6}>
          <FormLabel>Filter by student</FormLabel>
          <Select
            onChange={handleStudentFilterId}
            value={studentFilterId}
            fullWidth
            variant="outlined"
          >
            {[
              <MenuItem key={-1} value={0}>
                Filter by student
              </MenuItem>,
              ...students.map((s) => (
                <MenuItem key={s.id} value={s.id}>
                  {s.firstName} {s.lastName}
                </MenuItem>
              )),
            ]}
          </Select>
        </Grid>
      </Grid>
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
