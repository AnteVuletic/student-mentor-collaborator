import React, { useContext, useEffect } from "react";
import { Box } from "@material-ui/core";
import Students from "./Students";
import Mentors from "./Mentors";
import { Route, Switch } from "react-router-dom";
import { MenuContext } from "../../../services/providers/MenuProvider";
import InviteMentor from "./InviteMentor";

const baseUrl = "/home/admin";
const adminScreenTabs = {
  students: "Students",
  mentors: "Mentors",
  studentRoute: `${baseUrl}/students`,
  mentorsRoute: `${baseUrl}/mentors`,
};

const AdminScreen = () => {
  const { setNavigationLinks } = useContext(MenuContext);

  useEffect(() => {
    setNavigationLinks([
      {
        label: adminScreenTabs.students,
        path: adminScreenTabs.studentRoute,
      },
      {
        label: adminScreenTabs.mentors,
        path: adminScreenTabs.mentorsRoute,
      },
    ]);
  }, [setNavigationLinks]);

  return (
    <Box marginTop={2}>
      <Switch>
        <Route path={adminScreenTabs.studentRoute}>
          <Switch>
            <Route exact path={adminScreenTabs.studentRoute}>
              <Students />
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
      </Switch>
    </Box>
  );
};

export default AdminScreen;
