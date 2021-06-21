import {
  AppBar,
  Button,
  Container,
  Fab,
  Grid,
  LinearProgress,
  Toolbar,
  Tooltip,
  Box,
  Avatar,
} from "@material-ui/core";
import { GridArrowUpwardIcon } from "@material-ui/data-grid";
import { Person, PowerSettingsNew } from "@material-ui/icons";
import axios from "axios";
import React, { useContext, useEffect, useState } from "react";
import { Route, Switch, useHistory, Link } from "react-router-dom";

import AdminScreen from "./AuthorizedScreen/AdminScreen";
import MentorScreen from "./AuthorizedScreen/MentorScreen";
import StudentScreen from "./AuthorizedScreen/StudentScreen";
import ProfileScreen from "./AuthorizedScreen/ProfileScreen";
import SubscreenNavigation from "../components/SubscreenNavigation";
import ScrollTop from "../components/ScrollTop";

import { UserContext } from "../services/providers/UserProvider";
import { MenuContext } from "../services/providers/MenuProvider";
import { useAvatarStyles, useNavigationStyles } from "../theme/main";

const HomeScreen = (props) => {
  const avatarStyle = useAvatarStyles();
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
              <Tooltip title={`${userInfo.firstName} ${userInfo.lastName}`}>
                <Avatar className={avatarStyle.avatarPrimary}>
                  {userInfo.firstName?.charAt(0)}
                  {userInfo.lastName?.charAt(0)}
                </Avatar>
              </Tooltip>
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
                  <Link to="/home/profile">
                    <Button
                      color="primary"
                      variant="contained"
                      startIcon={<Person />}
                    >
                      Profile
                    </Button>
                  </Link>
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
          <Route path="/home/profile/:githubToken">
            <ProfileScreen />
          </Route>
          <Route path="/home/profile">
            <ProfileScreen />
          </Route>
        </Switch>
      </Container>
      <ScrollTop {...props}>
        <Box m={2}>
          <Grid container justify="flex-end">
            <Grid item>
              <Fab color="primary" size="small">
                <GridArrowUpwardIcon />
              </Fab>
            </Grid>
          </Grid>
        </Box>
      </ScrollTop>
    </>
  );
};

export default HomeScreen;
