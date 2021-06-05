import React, { useEffect, useState } from "react";
import {
  Box,
  Button,
  Container,
  Grid,
  LinearProgress,
  TextField,
  Typography,
} from "@material-ui/core";
import { Alert } from "@material-ui/lab";
import { Controller, useForm } from "react-hook-form";
import FormControlSpacing from "../components/styled/FormControlSpacing";
import { Link, useHistory, useParams } from "react-router-dom";
import axios from "axios";
import { parseJwt } from "../services/providers/UserProvider";
import { useFullHeightStyles } from "../theme/main";

const RegisterScreen = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [mentorId, setMentorId] = useState(null);
  const { token } = useParams();
  const fullHeightStyles = useFullHeightStyles();
  const history = useHistory();
  const {
    register,
    handleSubmit,
    watch,
    reset,
    control,
    formState: { errors },
  } = useForm({
    shouldFocusError: true,
    mode: "onChange",
    defaultValues: {
      email: "",
      firstName: "",
      lastName: "",
      password: "",
      confirmPassword: "",
    },
  });
  const [backendMessage, setBackendMessage] = useState(null);

  useEffect(() => {
    if (token) {
      setIsLoading(true);
      axios
        .get("api/Mentor/GetInfoFromToken", { params: { token } })
        .then((response) => {
          setMentorId(response.data.id);
          reset({
            email: response.data.email,
            firstName: response.data.firstName,
            lastName: response.data.lastName,
            password: "",
            confirmPassword: "",
          });
        })
        .catch((error) => setBackendMessage(error.data))
        .finally(() => {
          setIsLoading(false);
        });
    }
  }, [token, setMentorId, reset]);

  const submitRegistration = (value) => {
    const request = token
      ? axios.put(`api/Mentor/${mentorId}`, value)
      : axios.post("api/Account/RegisterStudent", value);

    request.then(
      (res) => {
        localStorage.setItem("token", res.data);
        const parsedToken = parseJwt(res.data);
        history.push(`/home/${parsedToken.role.toLowerCase()}`);
      },
      ({ response }) => {
        setBackendMessage(response.data);
      }
    );
  };

  const { ref: refRepeatPassword, ...rest } = register("confirmPassword", {
    validate: (value) =>
      value === watch("password") || "The passwords do not match",
  });

  if (isLoading) return <LinearProgress />;

  return (
    <Grid container className={fullHeightStyles.fullHeight} alignItems="center">
      <Container component="main" maxWidth="xs">
        <Typography component="h1" variant="h5">
          Register
        </Typography>
        <form onSubmit={handleSubmit(submitRegistration)}>
          <FormControlSpacing fullWidth variant="outlined">
            <Controller
              name="email"
              render={({ field: { onChange, onBlur, value } }) => (
                <TextField
                  onChange={onChange}
                  onBlur={onBlur}
                  value={value}
                  helperText={errors.email ? errors.email.message : null}
                  error={!!errors.email}
                  disabled={!!token}
                  id="email"
                  label="Email"
                  variant="outlined"
                />
              )}
              control={control}
              rules={{
                required: "Required",
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
                  disabled={!!token}
                  helperText={
                    errors.firstName ? errors.firstName.message : null
                  }
                  error={!!errors.firstName}
                  id="firstName"
                  label="First name"
                  variant="outlined"
                />
              )}
              control={control}
              rules={{
                required: "Required",
                minLength: 3,
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
                  disabled={!!token}
                  helperText={errors.lastName ? errors.lastName.message : null}
                  error={!!errors.lastName}
                  id="lastName"
                  label="Last name"
                  variant="outlined"
                />
              )}
              control={control}
              rules={{
                required: "Required",
                minLength: 3,
              }}
            />
          </FormControlSpacing>
          <FormControlSpacing fullWidth variant="outlined">
            <Controller
              name="password"
              render={({ field: { onChange, onBlur, value } }) => (
                <TextField
                  onChange={onChange}
                  onBlur={onBlur}
                  value={value}
                  helperText={errors?.password ? errors.password.message : null}
                  error={!!errors.password}
                  variant="outlined"
                  label="Password"
                  type="password"
                  id="password"
                  autoComplete="current-password"
                />
              )}
              control={control}
              rules={{
                required: "Required",
                minLength: {
                  value: 6,
                  message: "Password must be at least 6 characters long",
                },
              }}
            />
          </FormControlSpacing>
          <FormControlSpacing fullWidth variant="outlined">
            <TextField
              {...rest}
              inputRef={refRepeatPassword}
              helperText={
                errors?.confirmPassword ? errors.confirmPassword.message : null
              }
              error={!!errors.confirmPassword}
              variant="outlined"
              label="Repeat password"
              type="password"
              id="confirmPassword"
              autoComplete="current-password"
            />
          </FormControlSpacing>
          <Box mt={2} />
          {backendMessage && <Alert severity="error">{backendMessage}</Alert>}
          <Button type="submit" fullWidth variant="contained" color="primary">
            Register
          </Button>
          <Box mt={2} />
          <Grid container>
            <Grid item>
              <Link to="/login" variant="body2">
                Already have a account? Log in
              </Link>
            </Grid>
          </Grid>
        </form>
      </Container>
    </Grid>
  );
};

export default RegisterScreen;
