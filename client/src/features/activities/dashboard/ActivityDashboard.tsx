import ActivityList from "./ActivityList.tsx";
import ActivityFilters from "./ActivityFilters.tsx";
import { Grid } from "@mui/material";

const ActivityDashboard = () => {

  return (
    <Grid container spacing={3}>
      <Grid size={8}>
        <ActivityList />
      </Grid>
      <Grid
        sx={{
          position: 'sticky',
          top: 112,
          alignSelf: 'flex-start',
        }}
        size={4}
      >
        <ActivityFilters />
      </Grid>
    </Grid>
  )
}

export default ActivityDashboard;