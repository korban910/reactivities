import useAccount from "../../lib/hooks/useAccount.ts";
import { changePasswordSchema, type ChangePasswordSchema } from "../../lib/schemas/changePasswordSchema.ts";
import AccountFormWrapper from "./AccountFormWrapper.tsx";
import { Password } from "@mui/icons-material";
import { zodResolver } from "@hookform/resolvers/zod";
import TextInput from "../../app/shared/components/TextInput.tsx";

const ChangePasswordForm = () => {
  const { changePassword } = useAccount();

  const onSubmit = async (data: ChangePasswordSchema) => {
    try{
      await changePassword.mutateAsync(data);
    } catch (error) {
      console.log(error);

    }
  }

  return (
    <AccountFormWrapper<ChangePasswordSchema>
      title="Change Password"
      icon={<Password fontSize={'large'}/>}
      onSubmit={onSubmit}
      submitButtonText={'Update password'}
      resolver={zodResolver(changePasswordSchema)}
      reset
    >
      <TextInput type={'password'} label="Current Password" name={'currentPassword'} />
      <TextInput type={'password'} label="New Password" name={'newPassword'} />
      <TextInput type={'password'} label="Confirm Password" name={'confirmPassword'} />
    </AccountFormWrapper>
  )
}

export default ChangePasswordForm;