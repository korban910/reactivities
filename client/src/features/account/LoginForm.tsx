import useAccount from "../../lib/hooks/useAccount.ts";
import { useForm } from "react-hook-form";
import { loginSchema, type LoginSchema } from "../../lib/schemas/loginSchema.ts";
import { zodResolver } from "@hookform/resolvers/zod";
import { Box, Button, Paper, Typography } from "@mui/material";
import { LockOpen } from "@mui/icons-material";
import TextInput from "../../app/shared/components/TextInput.tsx";
import { Link, useLocation, useNavigate } from "react-router";
import { useState } from "react";
import { toast } from "react-toastify";

const LoginForm = () => {
  const [notVerified, setNotVerified] = useState<boolean>(false);
  const { loginUser, resendConfirmationEmail } = useAccount();
  const navigate = useNavigate();
  const location = useLocation();

  const { control, handleSubmit, watch, formState: { isValid, isLoading } } = useForm<LoginSchema>({
    mode: 'onTouched',
    resolver: zodResolver(loginSchema)
  });

  // eslint-disable-next-line react-hooks/incompatible-library
  const email = watch('email');

  const handleResendEmail = async () => {
    try {
      await resendConfirmationEmail.mutateAsync({
        email,
        userId: null
      });
      setNotVerified(false);
    } catch (error) {
      console.log(error);
      toast.error('Problem sending email - please check your email');
    }
  }

  const onSubmit = async (data: LoginSchema) => {
    await loginUser.mutateAsync(data, {
      onSuccess: () => {
        navigate(location.state?.from || '/activities');
      },
      onError: (error) => {
        if (error.message === 'NotAllowed'){
          setNotVerified(true);
        }
      }
    });
  }

  return (
    <Paper
      component={'form'}
      onSubmit={handleSubmit(onSubmit)}
      sx={{
        display: "flex",
        flexDirection: "column",
        p: 3,
        gap: 3,
        maxWidth: 'md',
        mx: 'auto',
        borderRadius: 3
      }}
    >
      <Box
        display={"flex"}
        alignItems={'center'}
        justifyContent={'center'}
        gap={3}
        color={'secondary.main'}
      >
        <LockOpen fontSize={'large'} />
        <Typography variant={'h4'}>Sign in</Typography>
      </Box>
      <TextInput label={'Email'} control={control} name={'email'} />
      <TextInput label={'Password'} type={'password'} control={control} name={'password'} />
      <Button
        type={'submit'}
        disabled={!isValid || isLoading}
        variant={'contained'}
        size={'large'}
      >
        Login
      </Button>
      {
        notVerified ? (
          <Box display={'flex'} flexDirection={'column'} justifyContent={'center'}>
            <Typography textAlign={'center'} color={'error'}>
              Your email has not been verified. You can click the button to re-send the verification email.
            </Typography>
            <Button
              disabled={resendConfirmationEmail.isPending}
              onClick={handleResendEmail}
            >
              Re-send email link
            </Button>
          </Box>
        ) : (
          <Box display={'flex'} alignItems={'center'} justifyContent={'center'} gap={3}>
            <Typography>
              Forgot password? Click <Link to={'/forgot-password'}>here</Link>
            </Typography>
            <Typography sx={{textAlign: 'center'}}>
              Don't have an account?
              <Typography sx={{ml: 2}} component={Link} to={'/register'} color={'primary'}>
                Sign up
              </Typography>
            </Typography>
          </Box>
        )
      }
    </Paper>
  )
}

export default LoginForm;