import useAccount from "../../lib/hooks/useAccount.ts";
import { useEffect, useRef, useState } from "react";
import { Link, useSearchParams } from "react-router";
import { Box, Button, Divider, Paper, Typography } from "@mui/material";
import { EmailRounded } from "@mui/icons-material";

const VerifyEmail = () => {
  const { verifyEmail, resendConfirmationEmail } = useAccount();
  const [status, setStatus] = useState('verifying');
  const [searchParams] = useSearchParams();
  const userId = searchParams.get("userId");
  const code = searchParams.get("code");
  const hasRun = useRef(false);

  useEffect(() => {
    if (code && userId && !hasRun.current) {
      hasRun.current = true;
      verifyEmail.mutateAsync({userId, code})
        .then(() => setStatus("verified"))
        .catch(() => setStatus("failed"));
    }
  }, [code, userId, verifyEmail]);

  const getBody = () => {
    switch (status) {
      case 'verifying':
        return <Typography>Verifying...</Typography>;
      case 'failed':
        return (
          <Box display={'flex'} flexDirection={'column'} gap={2} justifyContent={'center'}>
            <Typography>
              Verification failed. You can try resending the verify link to your email
            </Typography>
            <Button
              onClick={() => resendConfirmationEmail.mutateAsync({
                userId,
                email: null
              })}
              disabled={resendConfirmationEmail.isPending}
            >
              Resend verification email
            </Button>
          </Box>
        );
      case 'verified':
        return (
          <Box display={'flex'} flexDirection={'column'} gap={2} justifyContent={'center'}>
            <Typography>
              Email has been verified - you can now login
            </Typography>
            <Button component={Link} to={'/login'}>
              Go to login
            </Button>
          </Box>
        );
    }
  }

  return (
    <Paper
      sx={{
        height: 400,
        display: 'flex',
        flexDirection: 'column',
        justifyContent: 'center',
        alignItems: 'center',
        p: 6
      }}
    >
      <EmailRounded sx={{fontSize: 100}} color={'primary'}/>
      <Typography variant={'h3'} gutterBottom>
        Email verification
      </Typography>
      <Divider sx={{ mt: 2 }} />
      {getBody()}
    </Paper>
  )
}

export default VerifyEmail;