import React, { useContext, useEffect } from "react";
import { Box } from "@material-ui/core";
import { MenuContext } from "../../../services/providers/MenuProvider";
import { Switch, Route, Redirect } from "react-router-dom";
import MessageWall from "./MessageWall";
import FinalsPaper from "./FinalsPaper";
import MessageProvider from "../../../services/providers/MessageProvider";
import { navigation, studentMenuTabs } from "../../../utils/Navigation";

const StudentScreen = () => {
  const { setNavigationLinks } = useContext(MenuContext);

  useEffect(() => {
    setNavigationLinks(navigation.student);
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
