import type { ReactNode } from "react";
import { MenuItem } from "@mui/material";
import { NavLink } from "react-router";

type MenuItemLinkProps = {
  children: ReactNode
  to: string
};

const MenuItemLink = (
  {
    children,
    to
  } : MenuItemLinkProps
) => {
  return (
    <MenuItem
      component={NavLink}
      to={to}
      sx={{
        fontSize: '1.2rem',
        textTransform: 'uppercase',
        fontWeight: 'bold',
        color: 'inherit',
        '&.active': {
          color: 'yellow'
        }
      }}
    >
      {children}
    </MenuItem>
  )
}

export default MenuItemLink;