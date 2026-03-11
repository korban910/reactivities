import useAccount from "../../lib/hooks/useAccount.ts";
import { Button, Paper, Typography } from "@mui/material";
import { Check } from "@mui/icons-material";

type RegisterSuccessProps = {
  email?: string;
}

const RegisterSuccess = (
  {
    email,
  }: RegisterSuccessProps
) => {
  const { resendConfirmationEmail } = useAccount();

  if (!email) return null;

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
      <Check sx={{fontSize: 100}} color={'primary'} />
      <Typography variant="h3" gutterBottom>
        You have successfully registered!
      </Typography>
      <Typography variant="h3" gutterBottom>
        Please check your email to confirm your account
      </Typography>
      <Button fullWidth onClick={() => resendConfirmationEmail.mutate(email)}>
        Re-send confirmation email
      </Button>
    </Paper>
  )
}

export default RegisterSuccess;