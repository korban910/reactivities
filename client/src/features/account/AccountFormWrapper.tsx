import { Box, Button, Paper, Typography } from "@mui/material";
import type { ReactNode } from "react";
import { type FieldValues, FormProvider, type Resolver, useForm } from "react-hook-form";

type AccountFormWrapperProps<TFormData extends FieldValues> = {
  title: string;
  icon: ReactNode;
  onSubmit: (data: TFormData) => Promise<void>;
  children: ReactNode;
  submitButtonText: string;
  resolver?: Resolver<TFormData>;
  reset?: boolean;
}

const AccountFormWrapper = <TFormData extends FieldValues>(
  {
    title,
    icon,
    onSubmit,
    children,
    submitButtonText,
    resolver,
    reset
  }: AccountFormWrapperProps<TFormData>) => {

  const methods = useForm<TFormData>({
    resolver,
    mode: 'onTouched'
  });

  const formSubmit = async (data: TFormData) => {
    await onSubmit(data);
    if (reset) methods.reset();
  }

  return (
    <FormProvider {...methods}>
      <Paper
        component={'form'}
        onSubmit={methods.handleSubmit(formSubmit)}
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
          {icon}
          <Typography variant={'h4'}>{title}</Typography>
        </Box>
        {children}
        <Button
          type={'submit'}
          disabled={!methods.formState.isValid || methods.formState.isLoading}
          variant={'contained'}
          size={'large'}
        >
          {submitButtonText}
        </Button>
      </Paper>
    </FormProvider>
  )
}

export default AccountFormWrapper;