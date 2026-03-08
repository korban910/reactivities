import useAccount from "../lib/hooks/useAccount.ts";
import { Navigate, Outlet, useLocation } from "react-router";
import { Typography } from "@mui/material";

const RequireAuth = () => {
  const { user, loadingUserInfo } = useAccount();
  const location = useLocation();

  if (loadingUserInfo){
    return <Typography>Loading ...</Typography>
  }

  if (!user){
    return <Navigate replace to={'/login'} state={{ from: location }} />;
  }

  return (
    <Outlet />
  )
}

export default RequireAuth;