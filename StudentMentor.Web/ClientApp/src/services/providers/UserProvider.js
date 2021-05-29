import React, { useState } from "react";
import { useHistory } from "react-router";

export const parseJwt = (token) => {
  if (!token) return null;

  var base64Url = token.split(".")[1];
  var base64 = base64Url.replace(/-/g, "+").replace(/_/g, "/");
  var jsonPayload = decodeURIComponent(
    atob(base64)
      .split("")
      .map(function (c) {
        return "%" + ("00" + c.charCodeAt(0).toString(16)).slice(-2);
      })
      .join("")
  );

  return JSON.parse(jsonPayload);
};

const initialState = {
  role: null,
  userId: null,
};

export const UserContext = React.createContext({
  state: { ...initialState },
  logOut: () => {},
});

const UserProvider = ({ children }) => {
  const token = localStorage.getItem("token");
  const tokenParsed = parseJwt(token);
  const history = useHistory();
  const [role, setRole] = useState(tokenParsed?.role);
  const [userId, setUserId] = useState(tokenParsed?.userId);

  const logOut = () => {
    localStorage.removeItem("token");
    setRole(null);
    setUserId(null);
    history.push("/login");
  };

  if (token == null) {
    this.logOut();
  }

  const value = {
    state: { role, userId },
    logOut,
  };

  return <UserContext.Provider value={value}>{children}</UserContext.Provider>;
};

export default UserProvider;
