import useAccount from "../../lib/hooks/useAccount.ts";
import type { FieldValues } from "react-hook-form";
import { useNavigate } from "react-router";
import AccountFormWrapper from "./AccountFormWrapper.tsx";
import { LockOpen } from "@mui/icons-material";
import TextInput from "../../app/shared/components/TextInput.tsx";

const ForgotPasswordForm = () => {
  const { forgotPassword } = useAccount();
  const navigate = useNavigate();

  const onSubmit = async (data: FieldValues) => {
    try {
      await forgotPassword.mutateAsync(data.email);
      navigate("/login");
    } catch (error){
      console.error(error);
    }
  }

  return (
    <AccountFormWrapper
      title="Please enter your email address"
      icon={<LockOpen fontSize={'large'} />}
      submitButtonText={'Request password reset link'}
      onSubmit={onSubmit}
    >
      <TextInput name={'email'} label={'Email address'} rules={{required: true}} />
    </AccountFormWrapper>
  )
}

export default ForgotPasswordForm;