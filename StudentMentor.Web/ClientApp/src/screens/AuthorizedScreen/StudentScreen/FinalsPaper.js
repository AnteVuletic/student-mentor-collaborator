import React, { useEffect, useState } from "react";
import axios from "axios";
import {
  Box,
  Grid,
  LinearProgress,
  Typography,
  Button,
} from "@material-ui/core";

import DocViewer, { DocViewerRenderers } from "react-doc-viewer";

const FinalsPaper = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [paper, setPaper] = useState(null);

  const loadAndSetPaper = () => {
    return axios.get("api/Student/GetFinalsPaper").then((res) => {
      setPaper(res.data);
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

  if (isLoading) return <LinearProgress />;

  return (
    <Box mt={2}>
      <Grid container wrap="wrap" spacing={2} justify="center">
        <Grid xs={12} item>
          <Typography variant="h6">Finals paper</Typography>
        </Grid>
        {paper && (
          <Grid xs={12} item>
            <Typography variant="p">
              Current paper:{" "}
              <a href={paper.url} target="_blank" rel="noopener noreferrer">
                {paper.fileName}
              </a>
              <DocViewer
                pluginRenderers={DocViewerRenderers}
                documents={[{ uri: paper.url }]}
                style={{ width: "100%", height: "100%" }}
              />
            </Typography>
          </Grid>
        )}
        <Grid xs={6} item>
          <Button
            fullWidth
            color="primary"
            variant="contained"
            component="label"
          >
            Upload File
            <input type="file" hidden onChange={handleFileUpload} />
          </Button>
        </Grid>
      </Grid>
    </Box>
  );
};

export default FinalsPaper;
