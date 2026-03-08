import { type FieldValues, useController, type UseControllerProps } from "react-hook-form";
import { TextField, type TextFieldProps } from "@mui/material";

type TextInputProps<T extends FieldValues> = {} & UseControllerProps<T> & TextFieldProps;

const TextInput = <T extends FieldValues>(props: TextInputProps<T>) => {
  const { field, fieldState } = useController({ ...props });

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