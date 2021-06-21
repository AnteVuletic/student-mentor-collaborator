import { Box } from "@material-ui/core";
import React, { useContext, useEffect } from "react";
import { MenuContext } from "../../../services/providers/MenuProvider";
import { Switch, Route, Redirect } from "react-router-dom";
import MessageWall from "./MessageWall";
import MessageProvider from "../../../services/providers/MessageProvider";
import Students from "./Students";

const baseUrl = "/home/mentor";
const mentorMenuTabs = {
  messages: "Messages",
  students: "Students",
  messagesRoute: `${baseUrl}/messages`,
  studentsRoute: `${baseUrl}/students`,
};

const MentorScreen = () => {
  const { setNavigationLinks } = useContext(MenuContext);

  useEffect(() => {
    setNavigationLinks([
      {
        label: mentorMenuTabs.messages,
        path: mentorMenuTabs.messagesRoute,
      },
      {
        label: mentorMenuTabs.students,
        path: mentorMenuTabs.studentsRoute,
      },
    ]);
  }, [setNavigationLinks]);

  return (
    <Box marginTop={2}>
      <Switch>
        <Route path={mentorMenuTabs.messagesRoute}>
          <MessageProvider>
            <MessageWall />
          </MessageProvider>
        </Route>
        <Route path={mentorMenuTabs.studentsRoute}>
          <Students />
        </Route>
        <Route path="">
          <Redirect to={mentorMenuTabs.messagesRoute} />
        </Route>
      </Switch>
    </Box>
  );
};

export default MentorScreen;
