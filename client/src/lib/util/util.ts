import { type DateArg, format, formatDistanceToNow } from "date-fns";
import { z } from "zod";

const formatDate = (date: DateArg<Date>): string => {
  return format(date, 'dd MMM yyyy h:mm a');
}

const requiredString = (fieldName: string) =>
  z
    .string({ error : `${fieldName} is required`})
    .min(1, { message: `${fieldName} is required` });

const timeAgo = (date: DateArg<Date>) => {
  return formatDistanceToNow(date) + ' ago';
}

export {
  formatDate,
  requiredString,
  timeAgo,
}