import * as React from 'react';
import Popover from '@mui/material/Popover';
import ProfileCard from "../../../features/profiles/ProfileCard.tsx";
import { Link } from "react-router";
import { Avatar } from "@mui/material";

type AvatarPopoverProps = {
  profile: Profile;
}

const AvatarPopover = ({ profile } : AvatarPopoverProps) => {
  const [anchorEl, setAnchorEl] = React.useState<HTMLElement | null>(null);

  const handlePopoverOpen = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const handlePopoverClose = () => {
    setAnchorEl(null);
  };

  const open = Boolean(anchorEl);

  return (
    <>
      <Avatar
        alt={profile.displayName + ' image'}
        src={profile.imageUrl}
        component={Link}
        sx={{
          border: profile.following ? 3: 0,
          borderColor: 'secondary.main'
        }}
        to={`/profiles/${profile.id}`}
        onMouseEnter={handlePopoverOpen}
        onMouseLeave={handlePopoverClose}
      />
      <Popover
        id="mouse-over-popover"
        sx={{ pointerEvents: 'none' }}
        open={open}
        anchorEl={anchorEl}
        anchorOrigin={{
          vertical: 'bottom',
          horizontal: 'left',
        }}
        transformOrigin={{
          vertical: 'top',
          horizontal: 'left',
        }}
        onClose={handlePopoverClose}
        disableRestoreFocus
      >
        <ProfileCard profile={profile} />
      </Popover>
    </>
  );
}

export default AvatarPopover;