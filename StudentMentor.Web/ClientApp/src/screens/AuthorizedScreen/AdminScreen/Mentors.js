import React, { useEffect, useState } from "react";
import axios from "axios";
import {
  Button,
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
import { Link } from "react-router-dom";
import { Delete, MailOutline } from "@material-ui/icons";

const Mentors = () => {
  const [isLoading, setIsLoading] = useState(true);
  const [mentors, setMentors] = useState([]);
  const fetchMentors = () => {
    setIsLoading(true);
    axios.get("api/Mentor").then(({ data }) => {
      setMentors(data);
      setIsLoading(false);
    });
  };

  useEffect(fetchMentors, []);

  const deleteMentor = (mentorId) => {
    setIsLoading(true);
    axios.delete(`api/Mentor/${mentorId}`).then(fetchMentors);
  };

  if (isLoading) return <LinearProgress />;

  if (mentors.length === 0) {
    return (
      <Grid container spacing={2}>
        <Grid container item justify="center">
          <Typography variant="h5">No mentors registered yet</Typography>
        </Grid>
        <Grid container item justify="center">
          <Link to="/home/admin/mentors/invite">
            <Button
              type="submit"
              variant="contained"
              fullWidth
              color="primary"
              startIcon={<MailOutline />}
            >
              Send invite
            </Button>
          </Link>
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
            <TableCell>Actions</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {mentors.map((mentor) => (
            <TableRow key={mentor.id}>
              <TableCell>{mentor.id}</TableCell>
              <TableCell>{mentor.firstName}</TableCell>
              <TableCell>{mentor.lastName}</TableCell>
              <TableCell>{mentor.email}</TableCell>
              <TableCell>
                <IconButton
                  onClick={() => deleteMentor(mentor.id)}
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

export default Mentors;
