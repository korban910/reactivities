import { z } from "zod";

const loginSchema = z.object({
  email: z.string().email(),
  password: z.string({ error : "password is required"}).min(6, { message: "password is required" } ),
});

type LoginSchema = z.infer<typeof loginSchema>;

export {
  loginSchema,
  type LoginSchema
}