import { useSearchParams } from "react-router";
import useAccount from "../../lib/hooks/useAccount.ts";
import { useEffect, useRef, useState } from "react";
import { Box, CircularProgress, Paper, Typography } from "@mui/material";
import { GitHub } from "@mui/icons-material";

const AuthCallback = () => {
  const [params] = useSearchParams();
  const {fetchGithubToken} = useAccount();
  const code = params.get('code');
  const [loading, setLoading] = useState(true);
  const fetched = useRef(false);
  const [response, setResponse] = useState<object | null>(null);

  useEffect(() => {
    if (!code || fetched.current) return;
    fetched.current = true;

    fetchGithubToken.mutateAsync(code)
      .then(data => {
        setResponse(data);
        setLoading(false);
      })
      .catch(error => {
        console.error(error);
        setLoading(false);
      })
  }, [code, fetchGithubToken]);

  if (!code){
    return <Typography>Problem authenticating with GitHub</Typography>
  }

  return (
    <Paper
      sx={{
        height: 400,
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        justifyContent: "center",
        p: 3,
        gap: 3,
        maxWidth: 'md',
        mx: 'auto',
        borderRadius: 3
      }}
    >
      <Box sx={{ display: "flex", alignItems: "center", justifyContent: "center", gap: 3 }}>
        <GitHub fontSize={"large"} />
        <Typography variant={"h4"}>Logging in with GitHub</Typography>
      </Box>
      {
        loading ? <CircularProgress /> : <pre>{JSON.stringify(response, null, 2)}</pre>
      }
    </Paper>
  )
}

export default AuthCallback;