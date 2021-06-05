import React, { useEffect, useState } from "react";
import axios from "axios";
import {
  LinearProgress,
  Grid,
  IconButton,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Typography,
} from "@material-ui/core";
import { Delete } from "@material-ui/icons";

const Students = () => {
  const [isLoading, setIsLoading] = useState(true);
  const [students, setStudents] = useState([]);
  const fetchStudents = () => {
    setIsLoading(true);
    axios.get("api/Student").then(({ data }) => {
      setStudents(data);
      setIsLoading(false);
    });
  };

  useEffect(fetchStudents, []);

  const deleteStudent = (studentId) => {
    setIsLoading(true);
    axios.delete(`api/Student/${studentId}`).then(fetchStudents);
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
            <TableCell>Actions</TableCell>
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
                <IconButton
                  onClick={() => deleteStudent(student.id)}
                  color="secondary"
                >
                  <Delete />
                </IconButton>
              </TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  );
};

export default Students;
