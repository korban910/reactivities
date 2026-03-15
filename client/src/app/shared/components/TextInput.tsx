import { type FieldValues, useController, type UseControllerProps, useFormContext } from "react-hook-form";
import { TextField, type TextFieldProps } from "@mui/material";

type TextInputProps<T extends FieldValues> = {} & UseControllerProps<T> & TextFieldProps;

const TextInput = <T extends FieldValues>(
  {
    control,
    ...props
  }: TextInputProps<T>) => {
  const formContext = useFormContext<T>();
  const effectiveControl = control || formContext?.control;

  if (!effectiveControl) {
    throw new Error('Text input must be used within a form provider or passed as props');
  }

  const { field, fieldState } = useController({ ...props, control: effectiveControl });

  return (
    <TextField
      {...props}
      {...field}
      value={field.value || ''}
      fullWidth
      variant="outlined"
      error={!!fieldState.error}
      helperText={fieldState.error?.message}
    />
  )

}

export default TextInput;