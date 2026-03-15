import { createBrowserRouter, Navigate } from "react-router";
import App from "../app/layout/App.tsx";
import HomePage from "../features/home/HomePage.tsx";
import ActivityDashboard from "../features/activities/dashboard/ActivityDashboard.tsx";
import ActivityForm from "../features/activities/form/ActivityForm.tsx";
import ActivityDetailPage from "../features/activities/details/ActivityDetailPage.tsx";
import Counter from "../features/counter/Counter.tsx";
import TestErrors from "../features/errors/TestErrors.tsx";
import NotFound from "../features/errors/NotFound.tsx";
import ServerError from "../features/errors/ServerError.tsx";
import LoginForm from "../features/account/LoginForm.tsx";
import RequireAuth from "./RequireAuth.tsx";
import RegisterForm from "../features/account/RegisterForm.tsx";
import ProfilePage from "../features/profiles/ProfilePage.tsx";
import VerifyEmail from "../features/account/VerifyEmail.tsx";
import ChangePasswordForm from "../features/account/ChangePasswordForm.tsx";

const router = createBrowserRouter([
  {
    path: "/",
    element: <App />,
    children: [
      { element: <RequireAuth />, children: [
          { path: 'activities', element: <ActivityDashboard /> },
          { path: 'activities/:activityId', element: <ActivityDetailPage /> },
          { path: 'createActivity', element: <ActivityForm key='create' /> },
          { path: 'edit/:activityId', element: <ActivityForm /> },
          { path: 'profiles/:userId', element: <ProfilePage />},
          { path: 'change-password', element: <ChangePasswordForm />}
      ]},
      { path: '', element: <HomePage /> },
      { path: 'counter', element: <Counter /> },
      { path: 'errors', element: <TestErrors /> },
      { path: 'not-found', element: <NotFound /> },
      { path: 'server-error', element: <ServerError /> },
      { path: 'login', element: <LoginForm /> },
      { path: 'register', element: <RegisterForm /> },
      { path: 'confirm-email', element: <VerifyEmail /> },
      { path: '*', element: <Navigate replace to={'/not-found'} /> },
    ]
  }
]);

export default router;