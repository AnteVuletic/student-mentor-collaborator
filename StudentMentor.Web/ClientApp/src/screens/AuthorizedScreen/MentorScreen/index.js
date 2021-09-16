import { Box } from "@material-ui/core";
import React, { useContext, useEffect } from "react";
import { MenuContext } from "../../../services/providers/MenuProvider";
import { Switch, Route, Redirect } from "react-router-dom";
import MessageWall from "./MessageWall";
import MessageProvider from "../../../services/providers/MessageProvider";
import Students from "./Students";
import { mentorMenuTabs, navigation } from "../../../utils/Navigation";

const MentorScreen = () => {
  const { setNavigationLinks } = useContext(MenuContext);

  useEffect(() => {
    setNavigationLinks(navigation.mentor);
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
