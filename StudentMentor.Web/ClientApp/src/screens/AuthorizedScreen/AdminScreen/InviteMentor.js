import {
  Box,
  Button,
  LinearProgress,
  Container,
  Grid,
  TextField,
  Typography,
} from "@material-ui/core";
import { MailOutline } from "@material-ui/icons";
import { Alert } from "@material-ui/lab";
import axios from "axios";
import React, { useState } from "react";
import { Controller, useForm } from "react-hook-form";
import { Link, useHistory } from "react-router-dom";
import ErrorPrinter from "../../../components/ErrorPrinter";
import FormControlSpacing from "../../../components/styled/FormControlSpacing";

const InviteMentor = () => {
  const [isLoading, setIsLoading] = useState(false);
  const history = useHistory();
  const {
    handleSubmit,
    control,
    formState: { errors },
  } = useForm({
    shouldFocusError: true,
    mode: "onChange",
    defaultValues: {
      firstName: "",
      email: "",
      lastName: "",
    },
  });

  const [backendMessage, setBackendMessage] = useState(null);
  const [successMessage, setSuccessMessage] = useState(null);

  const handleInvite = (value) => {
    setIsLoading(true);
    axios
      .post("api/Mentor", value)
      .then(
        (res) => {
          setSuccessMessage("Successfully invited mentor");
          setTimeout(() => {
            history.push("/home/admin/mentors");
          }, 2000);
        },
        ({ response }) => setBackendMessage(response.data)
      )
      .finally(() => {
        setIsLoading(false);
      });
  };

  return (
    <Container component="main" maxWidth="xs">
      <Typography component="h1" variant="h5">
        Mentor invite
      </Typography>
      <form onSubmit={handleSubmit(handleInvite)}>
        <FormControlSpacing fullWidth variant="outlined">
          <Controller
            name="email"
            render={({ field: { onChange, onBlur, value } }) => (
              <TextField
                onChange={onChange}
                onBlur={onBlur}
                value={value}
                helperText={errors?.email ? errors.email.message : null}
                error={!!errors.email}
                id="email"
                label="Email"
                variant="outlined"
              />
            )}
            control={control}
            rules={{
              required: "Email is required",
              pattern: {
                value: /^\S+@\S+\.\S+$/,
                message: "Invalid email format",
              },
            }}
          />
        </FormControlSpacing>
        <FormControlSpacing fullWidth variant="outlined">
          <Controller
            name="firstName"
            render={({ field: { onChange, onBlur, value } }) => (
              <TextField
                onChange={onChange}
                onBlur={onBlur}
                value={value}
                helperText={errors?.firstName ? errors.firstName.message : null}
                error={!!errors.firstName}
                variant="outlined"
                label="First name"
                type="text"
                id="firstName"
              />
            )}
            control={control}
            rules={{
              required: "First name is required",
            }}
          />
        </FormControlSpacing>
        <FormControlSpacing fullWidth variant="outlined">
          <Controller
            name="lastName"
            render={({ field: { onChange, onBlur, value } }) => (
              <TextField
                onChange={onChange}
                onBlur={onBlur}
                value={value}
                helperText={errors?.lastName ? errors.lastName.message : null}
                error={!!errors.lastName}
                variant="outlined"
                label="Last name"
                type="text"
                id="lastName"
              />
            )}
            control={control}
            rules={{
              required: "Last name is required",
            }}
          />
        </FormControlSpacing>
        <Box mt={2}></Box>
        {backendMessage && <ErrorPrinter error={backendMessage} />}
        {successMessage && <Alert severity="success">{successMessage}</Alert>}
        {isLoading && <LinearProgress />}
        {!isLoading && (
          <Grid container justify="space-between">
            <Grid item>
              <Link to="/home/admin/mentors" variant="body2">
                <Button variant="contained" color="secondary">
                  Cancel
                </Button>
              </Link>
            </Grid>
            <Grid item>
              <Button
                type="submit"
                variant="contained"
                color="primary"
                startIcon={<MailOutline />}
              >
                Send invite
              </Button>
            </Grid>
          </Grid>
        )}
      </form>
    </Container>
  );
};

export default InviteMentor;
