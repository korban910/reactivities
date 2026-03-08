import { Card, CardContent, CardMedia, Grid, Typography } from "@mui/material";
import { Link } from "react-router";
import { format } from "date-fns";

type UserActivityProps = {
  userActivity: UserActivity;
}

const UserActivity = ({ userActivity } : UserActivityProps) => {
  return (
    <Grid size={2} key={userActivity.id}>
      <Link to={`/activities/${userActivity.id}`} style={{ textDecoration: "none" }}>
        <Card elevation={4}>
          <CardMedia
            component="img"
            height="100"
            image={
              `/images/categoryImages/${userActivity.category}.jpg`
            }
            alt={userActivity.title}
            sx={{objectFit: "cover"}}
            />
          <CardContent>
            <Typography variant="h6" textAlign={'center'} mb={1}>
              {userActivity.title}
            </Typography>
            <Typography
              variant={'body2'}
              textAlign={'center'}
              display={'flex'}
              flexDirection={'column'}
            >
              <span>
                {format(userActivity.date, 'do LLL yyyy')}
              </span>
              <span>
                {format(userActivity.date, 'h:mm a')}
              </span>
            </Typography>
          </CardContent>
        </Card>
      </Link>
    </Grid>
  )
}

export default UserActivity;