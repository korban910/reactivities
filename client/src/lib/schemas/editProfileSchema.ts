import { z } from "zod";
import { requiredString } from "../util/util.ts";

const editProfileSchema = z.object({
  displayName: requiredString("DisplayName"),
  bio: z.string().optional(),
})

type EditProfileSchema = z.infer<typeof editProfileSchema>;

export {
  type EditProfileSchema,
  editProfileSchema,
}