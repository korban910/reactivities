import { useParams } from "react-router";
import { useProfile } from "../../lib/hooks/useProfile.ts";
import { Box, Button, Divider, Typography } from "@mui/material";
import ProfileEditForm from "./ProfileEditForm.tsx";
import { useState } from "react";

const ProfileAbout = () => {
  const { userId } = useParams();
  const { profile } = useProfile(userId);
  const [editMode, setEditMode] = useState(false);
  return (
    <Box>
      <Box display={'flex'} justifyContent={'space-between'}>
        <Typography variant={'h5'}>About {profile?.displayName}</Typography>
        <Button onClick={() => setEditMode(!editMode)}>
          {!editMode ? "Edit profile" : "Cancel"}
        </Button>
      </Box>
      <Divider sx={{my: 2}}/>
      {
        !editMode ? (
          <Box sx={{overflow: 'auto', maxHeight: 350}}>
            <Typography variant={'body1'} sx={{whiteSpace: 'pre-wrap'}}>
              {profile?.bio || 'No description added yet'}
            </Typography>
          </Box>
        ) : (
          <ProfileEditForm onSuccess={setEditMode}/>
        )
      }
    </Box>
  )
}

export default ProfileAbout;