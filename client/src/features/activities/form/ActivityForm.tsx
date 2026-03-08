import { Box, Button, Paper, Typography } from "@mui/material";
import useActivities from "../../../lib/hooks/useActivities.ts";
import { Link, useNavigate, useParams } from "react-router";
import { useForm } from "react-hook-form";
import { useEffect } from "react";
import { activitySchema, type ActivitySchema } from "../../../lib/schemas/activitySchema.ts";
import { zodResolver } from "@hookform/resolvers/zod";
import TextInput from "../../../app/shared/components/TextInput.tsx";
import SelectInput from "../../../app/shared/components/SelectInput.tsx";
import { categoryOptions } from "../../../constants/categoryOptions.ts";
import DateTimeInput from "../../../app/shared/components/DateTimeInput.tsx";
import LocationInput from "../../../app/shared/components/LocationInput.tsx";

const ActivityForm = () => {
  const { control, reset, handleSubmit } = useForm<ActivitySchema>({
    mode: 'onTouched',
    resolver: zodResolver(activitySchema),
  });
  const navigate = useNavigate();
  const { activityId } = useParams();
  const { updateActivity, createActivity, activity, isLoadingActivity } = useActivities(activityId);

  const onSubmit = async (data: ActivitySchema) => {
    const { location, ...rest } = data;
    const flattenedData = {
      ...rest,
      ...location ,
    };
    try {
      if (activity){
        updateActivity.mutate({...activity, ...flattenedData}, {
          onSuccess: () => navigate(`/activities/${activity.id}`),
        });
      } else {
        createActivity.mutate(flattenedData, {
          onSuccess: (id) => navigate(`/activities/${id}`),
        })
      }
    } catch (error){
      console.log(error);
    }
  }

  useEffect(() => {
    if (activity) {
      reset({
        ...activity,
        date: new Date(activity.date),
        location: {
          city: activity.city,
          venue: activity.venue,
          latitude: activity.latitude,
          longitude: activity.longitude,
        }
      })
    }
  }, [activity, reset]);

  if (isLoadingActivity) {
    return <Typography>Loading Activity...</Typography>
  }

  return (
    <Paper sx={{borderRadius: 3, padding: 3}}>
      <Typography variant="h5" gutterBottom color="primary">
        {activity ? 'Edit' : 'Create'} activity
      </Typography>
      <Box component='form' onSubmit={handleSubmit(onSubmit)} display='flex' flexDirection='column' gap={3}>
        <TextInput
          label={'Title'}
          control={control}
          name={'title'}
        />
        <TextInput
          label={'Description'}
          control={control}
          name={'description'}
          multiline
          rows={3}
        />
        <Box display='flex' gap={3}>
          <SelectInput
            items={categoryOptions}
            label={'Category'}
            control={control}
            name={'category'}
          />
          <DateTimeInput
            label={'Date'}
            control={control}
            name={'date'}
          />
        </Box>
        <LocationInput
          control={control}
          label={'Enter the location'}
          name={'location'}
        />
        <Box display='flex' justifyContent='end' gap={3}>
          <Button component={Link} to={'/activities'} color='inherit'>Cancel</Button>
          <Button
            color='success'
            variant="contained"
            type="submit"
            disabled={updateActivity.isPending || createActivity.isPending}
          >Submit</Button>
        </Box>
      </Box>
    </Paper>
  )
}

export default ActivityForm;