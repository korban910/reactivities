import { useLocation } from "react-router";
import { Divider, Paper, Typography } from "@mui/material";

const ServerError = () => {
  const location = useLocation();
  const error = location.state?.error;

  return (
    <Paper>
      {error ? (
        <>
          <Typography
            gutterBottom
            variant="h3"
            sx={{ px: 4, pt: 2 }}
            color="secondary"
          >
            {error.message || "There has been an error"}
          </Typography>
          <Divider />
          <Typography variant="body1" sx={{ p: 4 }}>
            {error.details || "Internal server error"}
          </Typography>
        </>
      ) : (
        <Typography variant="h5" sx={{ p: 4 }}>
          Server error
        </Typography>
      )}
    </Paper>
  );
};

export default ServerError;
