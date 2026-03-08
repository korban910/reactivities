import { useParams } from "react-router";
import { useProfile } from "../../lib/hooks/useProfile.ts";
import { useState } from "react";
import { Box, Grid, Tab, Tabs, Typography } from "@mui/material";
import UserActivity from "../../app/shared/components/UserActivity.tsx";
const ProfileActivities = () => {
  const { userId } = useParams();
  const [filter, setFilter] = useState("future");
  const [value, setValue] = useState(0);
  const { userActivities, loadingUserActivities } = useProfile(userId, undefined, filter);

  const tabContent = [
    {label: 'FUTURE EVENTS', filter: "future" },
    {label: 'PAST EVENTS', filter: "past" },
    {label: 'HOSTING', filter: "hosting" },
  ]

  return (
    <Box>
      <Grid container spacing={2}>
        <Grid size={12}>
          <Tabs
            orientation={'horizontal'}
            value={value}
            onChange={(_, value) => {
              setValue(value);
              setFilter(tabContent[value].filter);
            }}
          >
          {
            tabContent.map((tab, index) => (
              <Tab key={index} label={tab.label}/>
            ))
          }
          </Tabs>
        </Grid>
      </Grid>
      <Grid
        container
        spacing={2}
        sx={{
          marginTop: 2,
          height: 400,
          overflow: 'auto',
          flexWrap: 'wrap' }}
      >
        {
          userActivities?.length ? (
            userActivities?.map((activity) => (
              <UserActivity key={activity.id} userActivity={activity} />
            ))
          ) : (
            <Typography>
              {loadingUserActivities ? "Loading..." : "No activities to show"}
            </Typography>
          )
        }
      </Grid>
    </Box>
  )
}

export default ProfileActivities