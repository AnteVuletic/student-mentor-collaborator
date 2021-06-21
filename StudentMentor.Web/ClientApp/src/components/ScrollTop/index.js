import { useScrollTrigger, Zoom } from "@material-ui/core";
import React from "react";

const ScrollTop = (props) => {
  const { children } = props;
  const trigger = useScrollTrigger({
    disableHysteresis: true,
    threshold: 1,
  });

  const handleClick = (event) => {
    const anchor = (event.target.ownerDocument || document).querySelector(
      "#back-to-top-anchor"
    );

    if (anchor) {
      anchor.scrollIntoView({ behavior: "smooth", block: "center" });
    }
  };

  return (
    <Zoom
      in={trigger}
      style={{ position: "fixed", bottom: "8px", right: "8px" }}
    >
      <div onClick={handleClick}>{children}</div>
    </Zoom>
  );
};

export default ScrollTop;
