import React, { useContext, useEffect } from "react";
import { Box } from "@material-ui/core";
import Students from "./Students";
import Mentors from "./Mentors";
import { Route, Switch, Redirect } from "react-router-dom";
import { MenuContext } from "../../../services/providers/MenuProvider";
import InviteMentor from "./InviteMentor";
import EditStudent from "./EditStudent";
import { adminScreenTabs, navigation } from "../../../utils/Navigation";

const AdminScreen = () => {
  const { setNavigationLinks } = useContext(MenuContext);

  useEffect(() => {
    setNavigationLinks(navigation.admin);
  }, [setNavigationLinks]);

  return (
    <Box marginTop={2}>
      <Switch>
        <Route path={adminScreenTabs.studentRoute}>
          <Switch>
            <Route exact path={adminScreenTabs.studentRoute}>
              <Students />
            </Route>
            <Route
              path={`${adminScreenTabs.studentRoute}/edit-student/:studentId`}
            >
              <EditStudent />
            </Route>
          </Switch>
        </Route>
        <Route path={adminScreenTabs.mentorsRoute}>
          <Switch>
            <Route exact path={adminScreenTabs.mentorsRoute}>
              <Mentors />
            </Route>
            <Route path={`${adminScreenTabs.mentorsRoute}/invite`}>
              <InviteMentor />
            </Route>
          </Switch>
        </Route>
        <Route path="">
          <Redirect to={adminScreenTabs.studentRoute} />
        </Route>
      </Switch>
    </Box>
  );
};

export default AdminScreen;
