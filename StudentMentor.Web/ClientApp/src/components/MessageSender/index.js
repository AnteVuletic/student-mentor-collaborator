import React, { useState } from "react";
import {
  TextField,
  Select,
  MenuItem,
  Grid,
  Button,
  LinearProgress,
  FormControl,
  FormHelperText,
  FormLabel,
} from "@material-ui/core";
import { Controller, useForm } from "react-hook-form";
import { Send } from "@material-ui/icons";
import { Alert } from "@material-ui/lab";
import ErrorPrinter from "../ErrorPrinter";
import { sendMessageToHub } from "../../utils/hubs";

const MessageSender = ({ students, mentor }) => {
  const [rows, setRows] = useState(3);
  const [isSending, setIsSending] = useState(false);
  const [successMessage, setSuccessMessage] = useState(null);
  const [backendError, setBackendError] = useState(null);

  const {
    handleSubmit,
    control,
    formState: { errors },
    reset,
  } = useForm({
    shouldFocusError: true,
    mode: "onChange",
    defaultValues: {
      userToId: mentor?.id || "",
      content: "",
    },
  });

  const sendMessage = (value) => {
    setIsSending(true);
    sendMessageToHub(value)
      .then(() => {
        reset();
        setSuccessMessage("Message successfully sent");
        setTimeout(() => {
          setSuccessMessage(null);
        }, 2000);
      })
      .catch((errors) => {
        setBackendError(errors.data);
      })
      .finally(() => {
        setIsSending(false);
      });
  };

  return (
    <Grid container>
      <Grid item xs={12}>
        <form onSubmit={handleSubmit(sendMessage)}>
          <Grid container spacing={2}>
            <Grid item xs={3}>
              {students && (
                <Controller
                  name="userToId"
                  control={control}
                  rules={{
                    required: "Messaging user is required",
                  }}
                  render={({ field: { onChange, onBlur, value } }) => (
                    <FormControl error={!!errors.userToId} fullWidth>
                      <FormLabel>Choose student</FormLabel>
                      <Select
                        onChange={onChange}
                        onBlur={onBlur}
                        value={value}
                        fullWidth
                        variant="outlined"
                        required
                      >
                        {[
                          <MenuItem disabled key={-1} value="">
                            Choose student
                          </MenuItem>,
                          ...students.map((s) => (
                            <MenuItem key={s.id} value={s.id}>
                              {s.firstName} {s.lastName}
                            </MenuItem>
                          )),
                        ]}
                      </Select>
                      <FormHelperText>
                        {errors?.userToId ? errors.userToId.message : null}
                      </FormHelperText>
                    </FormControl>
                  )}
                />
              )}
              {mentor && (
                <FormControl error={!!errors.userToId} fullWidth>
                  <Select
                    value={mentor.id}
                    fullWidth
                    disabled
                    variant="outlined"
                    required
                  >
                    <MenuItem disabled key={mentor.id} value={mentor.id}>
                      {mentor.firstName} {mentor.lastName}
                    </MenuItem>
                  </Select>
                </FormControl>
              )}
            </Grid>
            <Grid item xs={12}>
              <Controller
                name="content"
                control={control}
                rules={{
                  required: "Message content is required",
                }}
                render={({ field: { onChange, onBlur, value } }) => (
                  <TextField
                    value={value}
                    onChange={onChange}
                    onBlur={(e) => {
                      setRows(3);
                      onBlur(e);
                    }}
                    onFocus={() => setRows(5)}
                    variant="outlined"
                    placeholder="Type in your message here"
                    multiline
                    fullWidth
                    helperText={errors?.content ? errors.content.message : null}
                    error={!!errors.content}
                    rows={rows}
                  />
                )}
              />
            </Grid>
            <Grid item xs={12}>
              {successMessage && (
                <Alert severity="success">{successMessage}</Alert>
              )}
              {backendError && <ErrorPrinter error={backendError} />}
            </Grid>
            <Grid container justify="flex-end">
              {!isSending && (
                <Grid item xs={2}>
                  <Button
                    type="submit"
                    fullWidth
                    variant="contained"
                    color="primary"
                    startIcon={<Send />}
                  >
                    Send
                  </Button>
                </Grid>
              )}
              {isSending && (
                <Grid item xs={2}>
                  <LinearProgress />
                </Grid>
              )}
            </Grid>
          </Grid>
        </form>
      </Grid>
    </Grid>
  );
};

export default MessageSender;
