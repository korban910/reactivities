import { requiredString } from "../util/util.ts";
import { z } from "zod";

const resetPasswordSchema = z.object({
  newPassword: requiredString('New Password'),
  confirmPassword: requiredString('Confirm Password'),
}).refine((data) => data.newPassword === data.confirmPassword, {
  message: 'Password must match',
  path: ['confirmPassword'],
});

type ResetPasswordSchema = z.infer<typeof resetPasswordSchema>;

export {
  resetPasswordSchema,
  type ResetPasswordSchema
};