import { useParams } from "react-router";
import { useProfile } from "../../lib/hooks/useProfile.ts";
import { Box, Divider, Typography } from "@mui/material";
import ProfileCard from "./ProfileCard.tsx";

type ProfileFollowingsProps = {
  predicate: string
}

const ProfileFollowings = ({ predicate } : ProfileFollowingsProps) => {
  const { userId } = useParams();
  const {profile, followings, loadingFollowings} = useProfile(userId, predicate);

  return (
    <Box>
      <Box display="flex">
        <Typography variant="h5">
          {
            predicate === "followings"
              ? `People ${profile?.displayName} is following`
              : `People following ${profile?.displayName}`
          }
        </Typography>
      </Box>
      <Divider sx={{my: 2}}/>
      {
        loadingFollowings ? <Typography>Loading...</Typography>
          : (
            <Box display={'flex'} marginTop={3} gap={3}>
              {
                followings &&
                followings.length > 0
                  ? (followings.map(profile => (
                    <ProfileCard profile={profile} key={profile.id} />
                  )))
                  : <Typography>No one is following {profile?.displayName}</Typography>
              }
            </Box>
          )
      }
    </Box>
  )
}

export default ProfileFollowings;