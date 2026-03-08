import { z } from "zod";
import { requiredString } from "../util/util.ts";

const registerSchema = z.object({
  email: z.string(),
  displayName: requiredString('DisplayName'),
  password: requiredString('Password'),
});

type RegisterSchema = z.infer<typeof registerSchema>;

export {
  registerSchema,
  type RegisterSchema
};