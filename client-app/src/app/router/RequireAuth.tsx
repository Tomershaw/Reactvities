// This component enforces authentication for routes.
// If the user is not logged in, they are redirected to the home page.
// Otherwise, it renders the child routes using <Outlet />.
import { Navigate, Outlet, useLocation } from "react-router-dom";
import { useStore } from "../stores/store";

export default function RequireAuth() {
  // Destructure the isLoggedIn property from the userStore.
  const {
    userStore: { isLoggedIn },
  } = useStore();
  // Get the current location to redirect back after login if needed.
  const location = useLocation();

  // Redirect to home page if the user is not logged in.
  if (!isLoggedIn) {
    return <Navigate to="/" state={{ from: location }} />;
  }

  // Render child routes if the user is authenticated.
  return <Outlet />;
}
