import {
  AppBar,
  Button,
  Container,
  Fab,
  Grid,
  LinearProgress,
  Toolbar,
  Typography,
} from "@material-ui/core";
import { Person, PowerSettingsNew } from "@material-ui/icons";
import axios from "axios";
import React, { useContext, useEffect, useState } from "react";
import { Route, Switch, useHistory } from "react-router";

import AdminScreen from "./AuthorizedScreen/AdminScreen";
import MentorScreen from "./AuthorizedScreen/MentorScreen";
import StudentScreen from "./AuthorizedScreen/StudentScreen";

import { useNavigationStyles } from "../theme/main";
import { UserContext } from "../services/providers/UserProvider";
import SubscreenNavigation from "../components/SubscreenNavigation";
import { MenuContext } from "../services/providers/MenuProvider";
import ScrollTop from "../components/ScrollTop";
import { GridArrowUpwardIcon } from "@material-ui/data-grid";

const HomeScreen = (props) => {
  const {
    state: { navigationLinks, activeNavigationLink },
    setActiveNavigationLink,
  } = useContext(MenuContext);
  const history = useHistory();
  const styles = useNavigationStyles();
  const {
    state: { role },
    logOut,
  } = useContext(UserContext);
  const [userInfo, setUserInfo] = useState({});

  useEffect(() => {
    if (window.location.pathname === "/home" && role) {
      history.push(`/home/${role.toLowerCase()}`);
    }
    axios.get("api/Account").then(({ data }) => setUserInfo(data));
  }, [role, history]);

  if (!role) {
    return <LinearProgress />;
  }

  return (
    <>
      <AppBar position="sticky" className={styles.menu}>
        <Toolbar>
          <Grid container justify="space-between" alignItems="center">
            <Grid container item xs={2}>
              <Typography variant="h5">
                {userInfo.firstName} {userInfo.lastName}
              </Typography>
            </Grid>
            <Grid container item xs={7}>
              <SubscreenNavigation
                navigationLinks={navigationLinks}
                activeNavigationLink={activeNavigationLink}
                setActiveNavigationLink={setActiveNavigationLink}
              />
            </Grid>
            <Grid container item xs={3}>
              <Grid container justify="flex-end" spacing={1}>
                <Grid item>
                  <Button
                    color="primary"
                    variant="contained"
                    startIcon={<Person />}
                  >
                    Profile
                  </Button>
                </Grid>
                <Grid item>
                  <Button
                    color="secondary"
                    variant="contained"
                    startIcon={<PowerSettingsNew />}
                    onClick={logOut}
                  >
                    Log out
                  </Button>
                </Grid>
              </Grid>
            </Grid>
          </Grid>
        </Toolbar>
      </AppBar>
      <Container>
        <Toolbar id="back-to-top-anchor" />
        <Switch>
          <Route path="/home/student">
            <StudentScreen />
          </Route>
          <Route path="/home/mentor">
            <MentorScreen />
          </Route>
          <Route path="/home/admin">
            <AdminScreen />
          </Route>
        </Switch>
        <ScrollTop {...props}>
          <Fab color="primary" size="small">
            <GridArrowUpwardIcon />
          </Fab>
        </ScrollTop>
      </Container>
    </>
  );
};

export default HomeScreen;
