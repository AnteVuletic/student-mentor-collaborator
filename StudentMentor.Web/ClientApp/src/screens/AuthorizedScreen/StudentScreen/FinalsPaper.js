import React, { useEffect, useState, useContext } from "react";
import axios from "axios";
import {
  Box,
  Grid,
  LinearProgress,
  Typography,
  Button,
  Tabs,
  Tab,
  Card,
  Tooltip,
  Avatar,
  CardContent,
} from "@material-ui/core";

import DocViewer, { DocViewerRenderers } from "react-doc-viewer";
import { useAvatarStyles } from "../../../theme/main";
import { UserContext } from "../../../services/providers/UserProvider";

const FinalsPaper = () => {
  const {
    state: { userId },
  } = useContext(UserContext);
  const avatarStyle = useAvatarStyles();
  const [isLoading, setIsLoading] = useState(false);
  const [papers, setPapers] = useState(null);
  const [paperActiveIndex, setPaperActiveIndex] = useState(null);

  const loadAndSetPaper = () => {
    return axios.get("api/Student/GetFinalsPapers").then((res) => {
      setPapers(res.data);
      if (res.data.length > 0) {
        setPaperActiveIndex(0);
      }
    });
  };

  useEffect(() => {
    setIsLoading(true);

    loadAndSetPaper().finally(() => {
      setIsLoading(false);
    });
  }, []);

  const handleFileUpload = (event) => {
    setIsLoading(true);
    const file = event.target.files[0];
    const data = new FormData();
    data.append("file", file);
    axios
      .post("api/File", data, {
        headers: { "content-type": "multipart/form-data" },
      })
      .then((res) => {
        return axios
          .patch("api/Student/PatchFinalesPaper", res.data)
          .then(loadAndSetPaper);
      })
      .finally(() => {
        setIsLoading(false);
      });
  };

  const handlePaperActiveIndex = (event) => {
    setPaperActiveIndex(+event.target.id);
  };

  if (isLoading) return <LinearProgress />;

  return (
    <Box mt={2}>
      <Grid container spacing={2} justify="center">
        <Grid xs={12} item>
          <Typography variant="h6">Finals paper</Typography>
        </Grid>
        <Grid xs={12} container item justify="center">
          <Grid xs={6} item>
            <Button
              fullWidth
              color="primary"
              variant="contained"
              component="label"
            >
              Upload new version
              <input type="file" hidden onChange={handleFileUpload} />
            </Button>
          </Grid>
        </Grid>
        <Grid container item xs={12}>
          <Grid item xs={2}>
            {papers && (
              <Tabs
                orientation="vertical"
                variant="scrollable"
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
          <Grid item xs={10}>
            {papers?.map((paper, index) => (
              <Card key={index}>
                {paperActiveIndex === index && (
                  <CardContent>
                    {!paper.comments.length && <div>No comments</div>}
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
      </Grid>
    </Box>
  );
};

export default FinalsPaper;
