import { useNavigate, useSearchParams } from "react-router";
import useAccount from "../../lib/hooks/useAccount.ts";
import { Typography } from "@mui/material";
import { resetPasswordSchema, type ResetPasswordSchema } from "../../lib/schemas/resetPasswordSchema.ts";
import AccountFormWrapper from "./AccountFormWrapper.tsx";
import { zodResolver } from "@hookform/resolvers/zod";
import { LockOpen } from "@mui/icons-material";
import TextInput from "../../app/shared/components/TextInput.tsx";

const ResetPasswordForm = () => {
  const [params] = useSearchParams();
  const {resetPassword} = useAccount();
  const navigate = useNavigate();

  const email = params.get('email');
  const code = params.get('code');

  if (!email || !code) {
    return <Typography>Invalid reset password code</Typography>
  }

  const onSubmit = async (data: ResetPasswordSchema) => {
    try {
      await resetPassword.mutateAsync({
        email: email,
        resetCode: code,
        newPassword: data.newPassword,
      })
      navigate('/login');
    }catch (error) {
      console.error(error);
    }
  }

  return (
    <AccountFormWrapper<ResetPasswordSchema>
      title={"Reset your password"}
      submitButtonText={'Reset password'}
      onSubmit={onSubmit}
      resolver={zodResolver(resetPasswordSchema)}
      icon={<LockOpen fontSize={'large'} />}
    >
      <TextInput name={'newPassword'} type={'password'} label={'New Password'} />
      <TextInput name={'confirmPassword'} label={'Confirm password'} type={'password'} />
    </AccountFormWrapper>
  )
}

export default ResetPasswordForm;