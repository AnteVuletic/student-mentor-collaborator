import React, { useState, createContext, useEffect, useCallback } from "react";
import { useLocation } from "react-router";
import { navigation } from "../../utils/Navigation";
import { parseJwt } from "./UserProvider";

const initialState = {
  navigationLinks: [],
  activeNavigationLink: "",
};

export const MenuContext = createContext({
  state: { ...initialState },
  setNavigationLinks: () => {},
  setActiveNavigationLink: () => {},
});

const MenuProvider = ({ children }) => {
  const parsedToken = parseJwt(localStorage.getItem("token"));
  const { pathname } = useLocation();
  const [navigationLinks, setNavigationLinks] = useState(
    parsedToken ? navigation[parsedToken.role.toLowerCase()] : []
  );
  const [activeNavigationLink, setActiveNavigationInternal] = useState("");

  const getActiveNavigation = useCallback(() => {
    const newActiveNavigationLink = navigationLinks.find((value) =>
      pathname.includes(value.path)
    )?.path;

    return newActiveNavigationLink;
  }, [navigationLinks, pathname]);

  useEffect(() => {
    setActiveNavigationInternal(getActiveNavigation(pathname));
  }, [getActiveNavigation, setActiveNavigationInternal, pathname]);

  const setActiveNavigationLink = (value) => {
    setActiveNavigationInternal(getActiveNavigation(value));
  };

  const value = {
    state: { navigationLinks, activeNavigationLink },
    setNavigationLinks,
    setActiveNavigationLink,
  };

  return <MenuContext.Provider value={value}>{children}</MenuContext.Provider>;
};

export default MenuProvider;
