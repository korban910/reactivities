import { Grid, Typography } from "@mui/material";
import ProfileHeader from "./ProfileHeader.tsx";
import ProfileContent from "./ProfileContent.tsx";
import { useParams } from "react-router";
import { useProfile } from "../../lib/hooks/useProfile.ts";

const ProfilePage = () => {
  const { userId } = useParams();
  const { profile, loadingProfile } = useProfile(userId);

  if (loadingProfile) {
    return <Typography>Loading...</Typography>;
  }

  if (!profile) {
    return <Typography>No profile found.</Typography>;
  }

  return (
    <Grid container>
      <Grid size={12}>
        <ProfileHeader />
        <ProfileContent />
      </Grid>
    </Grid>
  )
}

export default ProfilePage;