import { Tab } from "@material-ui/core";
import { TabContext, TabList } from "@material-ui/lab";
import React from "react";
import { useHistory } from "react-router";

const SubscreenNavigation = ({
  navigationLinks,
  activeNavigationLink,
  setActiveNavigationLink,
}) => {
  const history = useHistory();
  const onNavigationChange = (value) => {
    history.push(value);
    setActiveNavigationLink(value);
  };

  return (
    <TabContext value={activeNavigationLink ?? ""}>
      <TabList variant="scrollable" scrollButtons="auto">
        {[
          <Tab key={-1} value={""} style={{ display: 'none' }} />,
          ...navigationLinks.map((navigationLink, index) => (
            <Tab
              key={index}
              label={navigationLink.label}
              value={navigationLink.path}
              onClick={() => onNavigationChange(navigationLink.path)}
            />
          )),
        ]}
      </TabList>
    </TabContext>
  );
};

export default SubscreenNavigation;
