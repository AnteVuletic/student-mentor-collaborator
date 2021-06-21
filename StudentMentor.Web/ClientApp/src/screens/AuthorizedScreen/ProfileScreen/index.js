import { Grid } from "@material-ui/core";
import axios from "axios";
import React, { useContext, useEffect, useState } from "react";
import { Typography, Avatar, LinearProgress, Button } from "@material-ui/core";
import { GitHub } from "@material-ui/icons";
import { useAvatarStyles } from "../../../theme/main";
import { UserContext } from "../../../services/providers/UserProvider";
import { useHistory, useParams } from "react-router-dom";
import StudentGithubSection from "./StudentGithubSection";

const ProfileScreen = () => {
  const history = useHistory();
  const { githubToken } = useParams();
  const {
    state: { role },
  } = useContext(UserContext);
  const avatarStyle = useAvatarStyles();
  const [profile, setProfile] = useState({});
  const [isLoading, setIsLoading] = useState(true);

  const getGithubAuthorizeLink = () => {
    axios.get("/api/Account/GetGithubAuthLink").then((response) => {
      window.location.replace(response.data);
    });
  };

  useEffect(() => {
    if (githubToken) {
      axios
        .patch("/api/Student/PatchGithubAccessKey", { githubToken })
        .then(() => {
          history.push("/home/profile");
        });
    } else {
      axios
        .get("/api/Account")
        .then((response) => {
          setProfile({ ...response.data });
        })
        .finally(() => {
          setIsLoading(false);
        });
    }
  }, [history, githubToken]);

  if (isLoading) {
    return <LinearProgress />;
  }

  const student = role === "Student";

  return (
    <Grid container justify="center" spacing={4}>
      <Grid container item spacing={2} alignItems="center">
        <Grid item>
          <Avatar className={avatarStyle.avatarPrimary}>
            {profile.firstName?.charAt(0)}
            {profile.lastName?.charAt(0)}
          </Avatar>
        </Grid>
        <Grid item>
          <Typography variant="body1">
            {profile.firstName}&nbsp;{profile.lastName}
          </Typography>
          <Typography variant="body1">{profile.email}</Typography>
        </Grid>
      </Grid>
      {student && (
        <Grid container item spacing={2}>
          <Grid item xs={12}>
            <Typography variant="h5">Mentor</Typography>
          </Grid>
          {profile.mentor && (
            <Grid container spacing={2} item xs={12}>
              <Grid item>
                <Avatar className={avatarStyle.avatarSecondary}>
                  {profile.mentor.firstName?.charAt(0)}
                  {profile.mentor.lastName?.charAt(0)}
                </Avatar>
              </Grid>
              <Grid item>
                <Typography variant="body1">
                  {profile.mentor.firstName}&nbsp;{profile.mentor.lastName}
                </Typography>
                <Typography variant="body1">{profile.mentor.email}</Typography>
              </Grid>
            </Grid>
          )}
          {!profile.mentor && (
            <Grid item xs={12}>
              <Typography variant="body1">No mentor assigned yet</Typography>
            </Grid>
          )}
        </Grid>
      )}
      {student && (
        <Grid container item spacing={2}>
          <Grid item xs={12}>
            <Typography variant="h5">Github</Typography>
          </Grid>
          <Grid item xs={12}>
            {!githubToken && !profile.githubAccessKey && (
              <Button
                variant="contained"
                color="primary"
                startIcon={<GitHub />}
                onClick={getGithubAuthorizeLink}
              >
                Authorize github
              </Button>
            )}
            {!githubToken && profile.githubAccessKey && (
              <StudentGithubSection />
            )}
          </Grid>
        </Grid>
      )}
    </Grid>
  );
};

export default ProfileScreen;
