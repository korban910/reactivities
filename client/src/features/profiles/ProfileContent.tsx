import { useState } from "react";
import { Box, Paper, Tab, Tabs } from "@mui/material";
import ProfilePhotos from "./ProfilePhotos.tsx";
import ProfileAbout from "./ProfileAbout.tsx";
import ProfileFollowings from "./ProfileFollowings.tsx";
import ProfileActivities from "./ProfileActivities.tsx";

const ProfileHeader = () => {
  const [value, setValue] = useState(0);

  const tabContent = [
    {label: 'About', content: <ProfileAbout />},
    {label: 'Photos', content: <ProfilePhotos />},
    {label: 'Events', content: <ProfileActivities />},
    {label: 'Followers', content: <ProfileFollowings predicate={'followers'} />},
    {label: 'Following', content: <ProfileFollowings predicate={'followings'} />},
  ]

  return (
    <Box
      component={Paper}
      mt={2}
      p={3}
      elevation={3}
      height={500}
      sx={{display: 'flex', alignItems: 'flex-start', borderRadius: 3}}
    >
      <Tabs
        orientation={'vertical'}
        value={value}
        onChange={(_, value) => setValue(value)}
        sx={{borderRight: 1, height: 450, flex: '0 0 200px'}}
        >
        {
          tabContent.map((tab, index) => (
            <Tab key={index} label={tab.label} sx={{mr: 3}} />
          ))
        }
      </Tabs>
      <Box sx={{flexGrow: 1, p:3, pt: 0}}>
        {tabContent[value].content}
      </Box>
    </Box>
  )
}

export default ProfileHeader