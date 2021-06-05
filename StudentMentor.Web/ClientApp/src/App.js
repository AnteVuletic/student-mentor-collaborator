import React from "react";
import { ThemeProvider } from "@material-ui/core";
import { Redirect, Route, Switch } from "react-router";
import HomeScreen from "./screens/HomeScreen";
import LoginScreen from "./screens/LoginScreen";
import RegisterScreen from "./screens/RegisterScreen";
import UserProvider from "./services/providers/UserProvider";
import "./App.css";

import main from "./theme/main";
import MenuProvider from "./services/providers/MenuProvider";

const App = () => {
  return (
    <MenuProvider>
      <ThemeProvider theme={main}>
        <Switch>
          <Route exact path="/login">
            <LoginScreen />
          </Route>
          <Route exact path="/register">
            <RegisterScreen />
          </Route>
          <Route path="/register/:token">
            <RegisterScreen />
          </Route>
          <Route path="/home">
            <UserProvider>
              <HomeScreen />
            </UserProvider>
          </Route>
          <Redirect to="/home" />
        </Switch>
      </ThemeProvider>
    </MenuProvider>
  );
};

export default App;
