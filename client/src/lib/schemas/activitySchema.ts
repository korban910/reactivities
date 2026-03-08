import { z } from "zod"
import { requiredString } from "../util/util.ts";

const activitySchema = z.object({
  title: requiredString('Title'),
  description: requiredString('Description'),
  category: requiredString('Category'),
  date: z.date({ message: 'Date is required' }),
  location: z.object({
    venue: requiredString('Venue'),
    city: requiredString('City'),
    latitude: z.number(),
    longitude: z.number(),
  })
});

type ActivitySchema = z.infer<typeof activitySchema>;

export {
  activitySchema,
  type ActivitySchema
}