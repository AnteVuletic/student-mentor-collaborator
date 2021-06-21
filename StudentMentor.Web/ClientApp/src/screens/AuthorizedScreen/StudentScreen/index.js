import React, { useContext, useEffect } from "react";
import { Box } from "@material-ui/core";
import { MenuContext } from "../../../services/providers/MenuProvider";
import { Switch, Route, Redirect } from "react-router-dom";
import MessageWall from "./MessageWall";
import FinalsPaper from "./FinalsPaper";
import MessageProvider from "../../../services/providers/MessageProvider";

const baseUrl = "/home/student";
const studentMenuTabs = {
  messages: "Messages",
  messagesRoute: `${baseUrl}/messages`,
  finalsPaper: "Finals paper",
  finalsPaperRoute: `${baseUrl}/finals-paper`,
};

const StudentScreen = () => {
  const { setNavigationLinks } = useContext(MenuContext);

  useEffect(() => {
    setNavigationLinks([
      {
        label: studentMenuTabs.messages,
        path: studentMenuTabs.messagesRoute,
      },
      {
        label: studentMenuTabs.finalsPaper,
        path: studentMenuTabs.finalsPaperRoute,
      },
    ]);
  }, [setNavigationLinks]);

  return (
    <Box marginTop={2}>
      <Switch>
        <Route path={studentMenuTabs.messagesRoute}>
          <MessageProvider>
            <MessageWall />
          </MessageProvider>
        </Route>
        <Route path={studentMenuTabs.finalsPaperRoute}>
          <FinalsPaper />
        </Route>
        <Route path="">
          <Redirect to={studentMenuTabs.messagesRoute} />
        </Route>
      </Switch>
    </Box>
  );
};
export default StudentScreen;
