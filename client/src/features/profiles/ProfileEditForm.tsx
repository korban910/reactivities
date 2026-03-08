import { useForm } from "react-hook-form";
import { editProfileSchema, type EditProfileSchema } from "../../lib/schemas/editProfileSchema.ts";
import { zodResolver } from "@hookform/resolvers/zod";
import { useEffect } from "react";
import { useProfile } from "../../lib/hooks/useProfile.ts";
import { useParams } from "react-router";
import { Box, Button, Paper, Typography } from "@mui/material";
import TextInput from "../../app/shared/components/TextInput.tsx";

type ProfileEditFormProps = {
  onSuccess: (editMode: boolean) => void;
}

const ProfileEditForm = ({ onSuccess } : ProfileEditFormProps) => {
  const { userId } = useParams();
  const { profile, editProfile } = useProfile(userId);
  const { control, reset, handleSubmit, formState: { isDirty, isValid }} = useForm<EditProfileSchema>({
    mode: 'onTouched',
    resolver: zodResolver(editProfileSchema),
    defaultValues: {
      displayName: profile?.displayName,
      bio: profile?.bio
    }
  })

  const onSubmit = async (values: EditProfileSchema) => {
    await editProfile.mutateAsync(values , {
      onSuccess: () => {
        onSuccess(false);
      }
    });
  }

  useEffect(() => {
    if (profile) {
      reset({
        displayName: profile?.displayName,
        bio: profile?.bio || '',
      });
    }
  }, [profile, reset])

  return (
    <Paper sx={{borderRadius: 3, padding: 3}}>
      <Typography variant={'h5'} gutterBottom color={'primary'}>
        Edit Profile
      </Typography>
      <Box component={'form'} onSubmit={handleSubmit(onSubmit)} display={'flex'} flexDirection={'column'} gap={3}>
        <TextInput
          label={'Display Name'}
          control={control}
          name={'displayName'}
          />
        <TextInput
          label={'Bio'}
          control={control}
          multiline
          rows={4}
          name={'bio'}
          />
        <Button
          color={'primary'}
          variant={'contained'}
          type={'submit'}
          disabled={editProfile.isPending || !isDirty || !isValid}
        >Update Profile</Button>
      </Box>
    </Paper>
  )
}

export default ProfileEditForm;