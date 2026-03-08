import { type FieldValues, useController, type UseControllerProps } from "react-hook-form";
import { DateTimePicker, type DateTimePickerProps } from "@mui/x-date-pickers";

type DateTimeInputProps<T extends FieldValues> = {} & UseControllerProps<T> & DateTimePickerProps;


const DateTimeInput = <T extends FieldValues>(props: DateTimeInputProps<T>) => {
  const { field, fieldState } = useController({...props});

  return (
    <DateTimePicker
      {...props}
      value={field.value ?? null}
      onChange={value => {
        field.onChange(new Date(value!));
      }}
      sx={{width: '100%'}}
      slotProps={{
        textField: {
          onBlur: field.onBlur,
          error: !!fieldState.error,
          helperText: fieldState.error?.message
        }
      }}
    />
  )
}

export default DateTimeInput;