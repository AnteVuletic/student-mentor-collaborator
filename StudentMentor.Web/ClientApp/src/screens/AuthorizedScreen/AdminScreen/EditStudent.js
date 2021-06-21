import {
  Box,
  Button,
  LinearProgress,
  Container,
  Grid,
  TextField,
  Typography,
  Select,
  MenuItem,
  InputLabel,
} from "@material-ui/core";
import { Person } from "@material-ui/icons";
import { Alert } from "@material-ui/lab";
import axios from "axios";
import React, { useEffect, useState } from "react";
import { Controller, useForm } from "react-hook-form";
import { Link, useHistory, useParams } from "react-router-dom";
import ErrorPrinter from "../../../components/ErrorPrinter";
import FormControlSpacing from "../../../components/styled/FormControlSpacing";

const EditStudent = () => {
  const { studentId } = useParams();
  const [isLoading, setIsLoading] = useState(true);
  const [mentors, setMentors] = useState([]);

  const history = useHistory();
  const { handleSubmit, reset, control } = useForm({
    shouldFocusError: true,
    mode: "onChange",
    defaultValues: {
      firstName: "",
      email: "",
      lastName: "",
      mentorId: 0,
    },
  });

  const [backendMessage, setBackendMessage] = useState(null);
  const [successMessage, setSuccessMessage] = useState(null);

  useEffect(() => {
    setIsLoading(true);
    Promise.all([
      axios.get(`api/Student/${studentId}`),
      axios.get("api/Mentor"),
    ])
      .then((resArray) => {
        const [student, mentors] = resArray;
        reset({
          firstName: student.data.firstName,
          lastName: student.data.lastName,
          email: student.data.email,
          mentorId: student.data.mentor?.id ?? 0,
        });

        setMentors(mentors.data);
      })
      .finally(() => setIsLoading(false));
  }, [studentId, reset]);

  const handleSetMentor = (value) => {
    setIsLoading(true);
    axios
      .put(`api/Student/${studentId}`, { mentorId: value.mentorId })
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
      <form onSubmit={handleSubmit(handleSetMentor)}>
        <FormControlSpacing fullWidth variant="outlined">
          <Controller
            name="email"
            render={({ field: { onChange, onBlur, value } }) => (
              <TextField
                onChange={onChange}
                onBlur={onBlur}
                value={value}
                disabled={true}
                id="email"
                label="Email"
                variant="outlined"
              />
            )}
            control={control}
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
                disabled={true}
                variant="outlined"
                label="First name"
                id="firstName"
              />
            )}
            control={control}
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
                disabled={true}
                variant="outlined"
                label="Last name"
                id="lastName"
              />
            )}
            control={control}
          />
        </FormControlSpacing>
        <Box mt={2} />
        <InputLabel htmlFor="mentorId">Mentor</InputLabel>
        <FormControlSpacing fullWidth variant="outlined">
          <Controller
            name="mentorId"
            render={({ field: { onChange, onBlur, value } }) => (
              <Select
                onChange={onChange}
                onBlur={onBlur}
                displayEmpty
                value={value}
                variant="outlined"
                id="mentorId"
              >
                {[
                  <MenuItem key={-1} value={0}>
                    None
                  </MenuItem>,
                  ...mentors.map((mentor) => (
                    <MenuItem key={mentor.id} value={mentor.id}>
                      {mentor.firstName} {mentor.lastName}
                    </MenuItem>
                  )),
                ]}
              </Select>
            )}
            control={control}
          />
        </FormControlSpacing>
        <Box mt={2}></Box>
        {backendMessage && <ErrorPrinter error={backendMessage} />}
        {successMessage && <Alert severity="success">{successMessage}</Alert>}
        {isLoading && <LinearProgress />}
        {!isLoading && (
          <Grid container justify="space-between">
            <Grid item>
              <Link to="/home/admin/students">
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
                startIcon={<Person />}
              >
                Set mentor
              </Button>
            </Grid>
          </Grid>
        )}
      </form>
    </Container>
  );
};

export default EditStudent;
