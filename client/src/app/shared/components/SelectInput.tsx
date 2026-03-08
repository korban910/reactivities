import { useController, type FieldValues, type UseControllerProps } from "react-hook-form";
import {
  FormControl,
  FormHelperText,
  InputLabel,
  MenuItem,
  Select,
  type SelectProps,
} from "@mui/material";

type SelectInputProps<T extends FieldValues> = {
  items: { text: string; value: string }[];
  label: string;
} & UseControllerProps<T> &
  Omit<SelectProps, "name" | "value" | "onChange">;

const SelectInput = <T extends FieldValues>(
  props: SelectInputProps<T>
) => {
  const { field, fieldState } = useController(props);

  return (
    <FormControl fullWidth error={!!fieldState.error}>
      <InputLabel>{props.label}</InputLabel>

      <Select
        {...field}
        value={field.value ?? ""}
        label={props.label}
      >
        {props.items.map((item) => (
          <MenuItem key={item.value} value={item.value}>
            {item.text}
          </MenuItem>
        ))}
      </Select>

      <FormHelperText>{fieldState.error?.message}</FormHelperText>
    </FormControl>
  );
};

export default SelectInput;
