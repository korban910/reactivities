import { requiredString } from "../util/util.ts";
import { z } from "zod";

const changePasswordSchema = z.object({
  currentPassword: requiredString('Current Password'),
  newPassword: requiredString('New Password'),
  confirmPassword: requiredString('Confirm Password'),
}).refine((data) => data.newPassword === data.confirmPassword, {
  message: 'Password must match',
  path: ['confirmPassword'],
});

type ChangePasswordSchema = z.infer<typeof changePasswordSchema>;

export {
  changePasswordSchema,
  type ChangePasswordSchema
};