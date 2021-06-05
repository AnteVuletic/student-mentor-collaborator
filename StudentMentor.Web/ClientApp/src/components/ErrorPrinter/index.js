import { Alert } from "@material-ui/lab";
import React from "react";

const ErrorPrinter = ({ error }) => {
  const errorKeys = Object.keys(error.errors);
  return (
    <>
      {errorKeys.map((key) => {
        if (Array.isArray(error.errors[key]))
          return error.errors[key].map((e) => (
            <Alert severity="error">{e}</Alert>
          ));
        return <Alert severity="error">{error.errors[key]}</Alert>;
      })}
    </>
  );
};

export default ErrorPrinter;
