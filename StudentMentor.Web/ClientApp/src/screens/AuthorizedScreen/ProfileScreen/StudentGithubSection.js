import axios from "axios";
import React, { useEffect, useState } from "react";
import {
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Link,
  Avatar,
  Button,
} from "@material-ui/core";

const StudentGithubSection = () => {
  const [repositories, setRepositories] = useState([]);
  const [repositoryId, setRepositoryId] = useState(null);
  useEffect(() => {
    axios.get("/api/Github/GetAllRepositories").then((response) => {
      setRepositories(response.data.repositories);
      setRepositoryId(response.data.repositoryId);
    });
  }, []);

  const assignRepository = (repositoryId) => {
    axios.patch("/api/Student/PatchRepositoryId", { repositoryId }).then(() => {
      setRepositoryId(repositoryId);
    });
  };

  return (
    <TableContainer>
      <Table>
        <TableHead>
          <TableRow>
            <TableCell>Id</TableCell>
            <TableCell>Name</TableCell>
            <TableCell>Owner</TableCell>
            <TableCell>Actions</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {repositories.map((repository) => (
            <TableRow
              selected={repository.id === repositoryId}
              key={repository.id}
            >
              <TableCell>{repository.id}</TableCell>
              <TableCell>
                <Link href={repository.url} target="_blank">
                  {repository.name}
                </Link>
              </TableCell>
              <TableCell>
                <Avatar url={repository.owner.avatarUrl} />
              </TableCell>
              <TableCell>
                {repositoryId !== repository.id && (
                  <Button
                    variant="outlined"
                    color="primary"
                    onClick={() => assignRepository(repository.id)}
                  >
                    Assign repository
                  </Button>
                )}
              </TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  );
};

export default StudentGithubSection;
