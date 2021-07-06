import {
  Avatar,
  Card,
  CardContent,
  CardHeader,
  Grid,
  Tooltip,
  Typography,
  Table,
  TableHead,
  TableBody,
  TableCell,
  TableRow,
  Dialog,
  Button,
  Toolbar,
  AppBar,
  IconButton,
  Box,
  TextField,
  LinearProgress,
} from "@material-ui/core";
import { ArrowForwardTwoTone, GitHub, Close, Send } from "@material-ui/icons";
import React, { useContext, useState } from "react";
import { UserContext } from "../../services/providers/UserProvider";
import {
  useAvatarStyles,
  useCardMessageStyles,
  useNavigationStyles,
} from "../../theme/main";
import { formatDateTime } from "../../utils/formatters";
import DocViewer, { DocViewerRenderers } from "react-doc-viewer";
import { useForm, Controller } from "react-hook-form";

import { Alert } from "@material-ui/lab";
import ErrorPrinter from "../ErrorPrinter";
import { sendCommentToHub } from "../../utils/hubs";

const Message = ({ message }) => {
  const {
    state: { userId },
  } = useContext(UserContext);
  const [isModalOpen, setIsModalOpen] = useState(false);

  const [isSending, setIsSending] = useState(false);
  const [successMessage, setSuccessMessage] = useState(null);
  const [backendError, setBackendError] = useState(null);

  const handleCloseModal = () => setIsModalOpen(false);
  const handleOpenModal = () => setIsModalOpen(true);

  const avatarStyle = useAvatarStyles();
  const cardStyle = useCardMessageStyles();
  const styles = useNavigationStyles();

  const {
    handleSubmit,
    control,
    formState: { errors },
    reset,
  } = useForm({
    shouldFocusError: true,
    mode: "onChange",
    defaultValues: {
      content: "",
    },
  });

  const handleSendComment = (value) => {
    setIsSending(true);
    sendCommentToHub({ ...value, messageId: message.id })
      .then(() => {
        reset();
        setSuccessMessage("Comment successfully sent");
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

  const messageTittle = (
    <Grid container alignItems="center" spacing={1}>
      <Grid item>
        <Tooltip title={message.userFrom.email}>
          <Avatar
            className={
              message.userFrom.id === +userId
                ? avatarStyle.avatarPrimary
                : avatarStyle.avatarSecondary
            }
          >
            {message.userFrom.firstName.charAt(0)}
            {message.userFrom.lastName.charAt(0)}
          </Avatar>
        </Tooltip>
      </Grid>
      <ArrowForwardTwoTone color="primary" display="block" />
      <Grid item>
        <Tooltip title={message.userTo.email}>
          <Avatar
            className={
              message.userTo.id === +userId
                ? avatarStyle.avatarPrimary
                : avatarStyle.avatarSecondary
            }
          >
            {message.userTo.firstName.charAt(0)}
            {message.userTo.lastName.charAt(0)}
          </Avatar>
        </Tooltip>
      </Grid>
    </Grid>
  );

  const subHeader = (
    <Box mt={1}>
      <Typography variant="body2" color="textSecondary">
        Poslano: {formatDateTime(message.messageCreatedAt)}
      </Typography>
    </Box>
  );

  return (
    <Card>
      <CardHeader title={messageTittle} subheader={subHeader} />
      <CardContent>
        {message.content && (
          <Box mt={2}>
            <Typography variant="body2" component="p">
              {message.content}
            </Typography>
          </Box>
        )}
        {message.repositoryName && (
          <Box mt={2}>
            <Grid container>
              <Grid item container alignItems="center">
                <Box mr={2}>
                  <GitHub />
                </Box>
                <Typography body="body2" component="span">
                  {message.repositoryName}
                </Typography>
              </Grid>
            </Grid>
          </Box>
        )}
        {message.file && (
          <Box mt={2}>
            <Grid item xs={12}>
              <Box mt={1}>
                <Typography variant="body2">
                  File named{" "}
                  <a
                    href={message.file.url}
                    target="_blank"
                    rel="noopener noreferrer"
                  >
                    {message.file.fileName}
                  </a>{" "}
                  was uploaded
                </Typography>
                <Button
                  fullWidth
                  color="primary"
                  variant="outlined"
                  onClick={handleOpenModal}
                >
                  Open preview
                </Button>
                <Dialog open={isModalOpen} onClose={handleCloseModal} fullWidth>
                  <AppBar className={styles.menu}>
                    <Toolbar>
                      <IconButton
                        edge="start"
                        color="inherit"
                        onClick={handleCloseModal}
                        aria-label="close"
                      >
                        <Close />
                      </IconButton>
                      <Typography variant="h6">
                        {message.file.fileName}
                      </Typography>
                    </Toolbar>
                  </AppBar>
                  <DocViewer
                    pluginRenderers={DocViewerRenderers}
                    documents={[{ uri: message.file.url }]}
                    style={{ width: "100%", height: "100%" }}
                  />
                </Dialog>
              </Box>
            </Grid>
          </Box>
        )}
        {message.commits && message.commits.length !== 0 && (
          <Box mt={2}>
            <Grid item xs={12}>
              <Box mt={1}>
                <Table size="small">
                  <TableHead>
                    <TableRow>
                      <TableCell>Message</TableCell>
                      <TableCell>Time stamp</TableCell>
                    </TableRow>
                  </TableHead>
                  <TableBody>
                    {message.commits.map((c, index) => (
                      <>
                        <TableRow key={index}>
                          <Tooltip title={c.id}>
                            <TableCell>
                              <a
                                href={c.url}
                                target="_blank"
                                rel="noopener noreferrer"
                              >
                                {c.message}
                              </a>
                            </TableCell>
                          </Tooltip>
                          <TableCell>{formatDateTime(c.timeStamp)}</TableCell>
                        </TableRow>
                        <TableRow key={c.id}>
                          <TableCell colSpan={2}>
                            {c.fileLogs?.length !== 0 && (
                              <Table size="small">
                                <TableHead>
                                  <TableRow>
                                    <TableCell>Action</TableCell>
                                    <TableCell>File</TableCell>
                                  </TableRow>
                                </TableHead>
                                <TableBody>
                                  {c.fileLogs.map((fl) => (
                                    <TableRow key={fl.id}>
                                      <TableCell>{fl.changeType}</TableCell>
                                      <TableCell>{fl.file}</TableCell>
                                    </TableRow>
                                  ))}
                                </TableBody>
                              </Table>
                            )}
                          </TableCell>
                        </TableRow>
                      </>
                    ))}
                  </TableBody>
                </Table>
              </Box>
            </Grid>
          </Box>
        )}
        {message.comments && message.comments.length !== 0 && (
          <Box mt={6} ml={2}>
            <Grid container spacing={5} item xs={12}>
              {message.comments.map((comment) => (
                <Card key={comment.id} className={cardStyle.wrapper}>
                  <Grid container spacing={2} item xs={12} alignItems="center">
                    <Grid item xs={3}>
                      <Tooltip
                        title={`${comment.userFrom.firstName} ${comment.userFrom.lastName}`}
                      >
                        <Avatar
                          className={
                            comment.userFrom.id === userId
                              ? avatarStyle.avatarPrimary
                              : avatarStyle.avatarSecondary
                          }
                        >
                          {comment.userFrom.firstName?.charAt(0)}
                          {comment.userFrom.lastName?.charAt(0)}
                        </Avatar>
                      </Tooltip>
                    </Grid>
                    <Grid item xs={9}>
                      <Typography variant="body2" component="p">
                        {comment.content}
                      </Typography>
                    </Grid>
                  </Grid>
                </Card>
              ))}
            </Grid>
          </Box>
        )}
        <Box mt={6}>
          <Grid container item xs={12}>
            <form
              onSubmit={handleSubmit(handleSendComment)}
              style={{ width: "100%" }}
            >
              <Grid container spacing={2} item xs={12} alignItems="center">
                <Grid item xs={10}>
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
                        onBlur={onBlur}
                        variant="outlined"
                        placeholder="Type in your message here"
                        multiline
                        fullWidth
                        helperText={
                          errors?.content ? errors.content.message : null
                        }
                        error={!!errors.content}
                        rows={2}
                      />
                    )}
                  />
                </Grid>

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
              <Grid item xs={12}>
                {successMessage && (
                  <Alert severity="success">{successMessage}</Alert>
                )}
                {backendError && <ErrorPrinter error={backendError} />}
              </Grid>
            </form>
          </Grid>
        </Box>
      </CardContent>
    </Card>
  );
};

export default Message;
