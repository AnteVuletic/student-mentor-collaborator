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
} from "@material-ui/core";
import { ArrowForwardTwoTone, GitHub, Close } from "@material-ui/icons";
import React, { useContext, useState } from "react";
import { UserContext } from "../../services/providers/UserProvider";
import { useAvatarStyles, useNavigationStyles } from "../../theme/main";
import { formatDateTime } from "../../utils/formatters";
import DocViewer, { DocViewerRenderers } from "react-doc-viewer";

const Message = ({ message }) => {
  const {
    state: { userId },
  } = useContext(UserContext);
  const [isModalOpen, setIsModalOpen] = useState(false);

  const handleCloseModal = () => setIsModalOpen(false);
  const handleOpenModal = () => setIsModalOpen(true);

  const avatarStyle = useAvatarStyles();
  const styles = useNavigationStyles();

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
        <Typography variant="body2" component="p">
          {message.content}
        </Typography>
        <Grid container>
          {message.repositoryName && (
            <Grid item container alignItems="center">
              <Box mr={2}>
                <GitHub />
              </Box>
              <Typography body="body2" component="span">
                {message.repositoryName}
              </Typography>
            </Grid>
          )}
          {message.file && (
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
          )}
          {message.commits && message.commits.length !== 0 && (
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
                        {c.fileLogs?.length !== 0 && (
                          <TableRow key={`action-${index}`}>
                            <TableCell>
                              <Typography variant="h6">Action</Typography>
                            </TableCell>
                            <TableCell>
                              <Typography variant="h6">File</Typography>
                            </TableCell>
                          </TableRow>
                        )}
                        {c.fileLogs?.length !== 0 &&
                          c.fileLogs.map((fl) => (
                            <TableRow key={`file-${fl.id}`}>
                              <TableCell>{fl.changeType}</TableCell>
                              <TableCell>{fl.file}</TableCell>
                            </TableRow>
                          ))}
                      </>
                    ))}
                  </TableBody>
                </Table>
              </Box>
            </Grid>
          )}
        </Grid>
      </CardContent>
    </Card>
  );
};

export default Message;
