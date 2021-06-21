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
} from "@material-ui/core";

import { Close } from "@material-ui/icons";
import DocViewer, { DocViewerRenderers } from "react-doc-viewer";
import { useNavigationStyles } from "../../../theme/main";

const Students = () => {
  const [isLoading, setIsLoading] = useState(true);
  const [students, setStudents] = useState([]);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const styles = useNavigationStyles();
  const fetchStudents = () => {
    setIsLoading(true);
    axios.get("api/Student/GetMentoringStudents").then(({ data }) => {
      setStudents(data);
      setIsLoading(false);
    });
  };

  useEffect(fetchStudents, []);

  const handleOpenModal = () => setIsModalOpen(true);
  const handleCloseModal = () => setIsModalOpen(false);

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
                {student.mentor
                  ? `${student.mentor.firstName} ${student.mentor.lastName}`
                  : "No mentor"}
              </TableCell>
              <TableCell>
                {student.finalsPaper && (
                  <Button
                    fullWidth
                    color="primary"
                    variant="outlined"
                    onClick={handleOpenModal}
                  >
                    Open preview
                  </Button>
                )}
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
                        {student.finalsPaper.fileName}
                      </Typography>
                    </Toolbar>
                  </AppBar>
                  <DocViewer
                    pluginRenderers={DocViewerRenderers}
                    documents={[{ uri: student.finalsPaper.url }]}
                    style={{ width: "100%", height: "100%" }}
                  />
                </Dialog>
              </TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  );
};

export default Students;
