import { createMuiTheme, makeStyles } from "@material-ui/core";

const main = createMuiTheme({
  palette: {
    primary: {
      main: "#6666FF",
    },
    error: {
      main: "#FF375C",
    },
    success: {
      main: "#2AA8E0",
    },
  },
});

export const useNavigationStyles = makeStyles((theme) => ({
  menu: {
    backgroundColor: "#262A36",
  },
}));

export const useFullHeightStyles = makeStyles((theme) => ({
  fullHeight: {
    height: "100vh",
  },
}));

export default main;
