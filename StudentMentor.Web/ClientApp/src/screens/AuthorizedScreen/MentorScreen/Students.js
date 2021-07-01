import React, { useEffect, useState } from "react";
import axios from "axios";
import {
  LinearProgress,
  Grid,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Typography,
  Dialog,
  AppBar,
  Toolbar,
  IconButton,
  Button,
  Tooltip,
  Avatar,
  Tabs,
  Tab,
  Card,
  CardContent,
} from "@material-ui/core";

import { Close } from "@material-ui/icons";
import DocViewer, { DocViewerRenderers } from "react-doc-viewer";
import { useAvatarStyles, useNavigationStyles } from "../../../theme/main";

const Students = () => {
  const avatarStyle = useAvatarStyles();
  const [isLoading, setIsLoading] = useState(true);
  const [students, setStudents] = useState([]);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [papers, setPapers] = useState(null);
  const [paperActiveIndex, setPaperActiveIndex] = useState(null);
  const styles = useNavigationStyles();
  const fetchStudents = () => {
    setIsLoading(true);
    axios.get("api/Student/GetMentoringStudents").then(({ data }) => {
      setStudents(data);
      setIsLoading(false);
    });
  };

  const loadAndSetPaper = (studentId) => {
    return axios
      .get(`api/Student/GetFinalsPapersForMentor/${studentId}`)
      .then((res) => {
        setPapers(res.data);
        if (res.data.length > 0) {
          setPaperActiveIndex(0);
        }
      });
  };

  useEffect(fetchStudents, []);

  const handleOpenModal = (studentId) => {
    loadAndSetPaper(studentId).then(() => {
      setIsModalOpen(true);
    });
  };

  const handleCloseModal = () => {
    setPaperActiveIndex(null);
    setPapers(null);
    setIsModalOpen(false);
  };

  const handleClaimStudent = (studentId) => {
    setIsLoading(true);
    axios.patch(`api/Mentor/ClaimStudent/${studentId}`).then(fetchStudents);
  };

  const handleUnClaimStudent = (studentId) => {
    setIsLoading(true);
    axios.patch(`api/Mentor/UnClaimStudent/${studentId}`).then(fetchStudents);
  };

  const handlePaperActiveIndex = (event) => {
    setPaperActiveIndex(+event.target.id);
  };

  if (isLoading) return <LinearProgress />;

  if (students.length === 0) {
    return (
      <Grid container justify="center">
        <Grid item>
          <Typography variant="h5">No students registered yet</Typography>
        </Grid>
      </Grid>
    );
  }

  return (
    <TableContainer>
      <Table>
        <TableHead>
          <TableRow>
            <TableCell>Id</TableCell>
            <TableCell>First name</TableCell>
            <TableCell>Last name</TableCell>
            <TableCell>Email</TableCell>
            <TableCell>Mentor</TableCell>
            <TableCell>Paper</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {students.map((student) => (
            <TableRow key={student.id}>
              <TableCell>{student.id}</TableCell>
              <TableCell>{student.firstName}</TableCell>
              <TableCell>{student.lastName}</TableCell>
              <TableCell>{student.email}</TableCell>
              <TableCell>
                {student.mentor && (
                  <Tooltip title="Unclaim student">
                    <Button
                      fullWidth
                      color="secondary"
                      variant="outlined"
                      onClick={() => handleUnClaimStudent(student.id)}
                    >
                      {student.mentor.firstName} {student.mentor.lastName}
                    </Button>
                  </Tooltip>
                )}
                {!student.mentor && (
                  <Button
                    fullWidth
                    color="primary"
                    variant="outlined"
                    onClick={() => handleClaimStudent(student.id)}
                  >
                    Claim
                  </Button>
                )}
              </TableCell>
              <TableCell>
                {student.hasPaper && (
                  <>
                    <Button
                      fullWidth
                      color="primary"
                      variant="outlined"
                      onClick={() => handleOpenModal(student.id)}
                    >
                      View papers
                    </Button>
                    <Dialog
                      open={isModalOpen}
                      onClose={handleCloseModal}
                      fullWidth
                    >
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
                            {student.firstName} {student.lastName}
                          </Typography>
                        </Toolbar>
                      </AppBar>
                      <Grid container item xs={12}>
                        <Grid item xs={12}>
                          {papers && (
                            <Tabs
                              value={paperActiveIndex}
                              onChange={handlePaperActiveIndex}
                            >
                              {papers?.map((p, index) => (
                                <Tab
                                  label={`Version - ${papers.length - index}`}
                                  key={p.id}
                                  id={index}
                                />
                              ))}
                            </Tabs>
                          )}
                        </Grid>
                        <Grid item xs={12}>
                          {papers?.map((paper, index) => (
                            <Card key={index}>
                              {paperActiveIndex === index && (
                                <CardContent>
                                  {!paper.comments.length && (
                                    <div>No comments</div>
                                  )}
                                  {paper.comments.length !== 0 &&
                                    paper.comments.map((comment) => (
                                      <Grid
                                        key={comment.id}
                                        container
                                        spacing={2}
                                        item
                                        xs={12}
                                        style={{
                                          padding: "2px 8px",
                                          margin: "0 12px 4px 12px",
                                        }}
                                        alignItems="center"
                                      >
                                        <Grid item xs={3}>
                                          <Tooltip
                                            title={`${comment.userFrom.firstName} ${comment.userFrom.lastName}`}
                                          >
                                            <Avatar
                                              className={
                                                comment.userFrom.id ===
                                                comment.userFrom.id
                                                  ? avatarStyle.avatarPrimary
                                                  : avatarStyle.avatarSecondary
                                              }
                                            >
                                              {comment.userFrom.firstName?.charAt(
                                                0
                                              )}
                                              {comment.userFrom.lastName?.charAt(
                                                0
                                              )}
                                            </Avatar>
                                          </Tooltip>
                                        </Grid>
                                        <Grid item xs={9}>
                                          <Typography
                                            variant="body2"
                                            component="p"
                                          >
                                            {comment.content}
                                          </Typography>
                                        </Grid>
                                      </Grid>
                                    ))}
                                  <div>
                                    <a
                                      href={paper.url}
                                      target="_blank"
                                      rel="noopener noreferrer"
                                    >
                                      {paper.fileName}
                                    </a>
                                    <DocViewer
                                      pluginRenderers={DocViewerRenderers}
                                      documents={[{ uri: paper.url }]}
                                      style={{ width: "100%", height: "70vh" }}
                                    />
                                  </div>
                                </CardContent>
                              )}
                            </Card>
                          ))}
                        </Grid>
                      </Grid>
                    </Dialog>
                  </>
                )}
              </TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  );
};

export default Students;
