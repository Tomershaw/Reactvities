// Import necessary modules and components from React Router and the application
import { createBrowserRouter, Navigate, RouteObject } from "react-router-dom";
import App from "../layout/App";
import HomePage from "../../features/home/HomePage";
import ActivityDashboard from "../../features/activities/dashboard/ActivityDashboard";
import ActicityFrom from "../../features/activities/form/ActicityFrom";
import ActivityDetalis from "../../features/activities/details/ActivityDetalis";
import TestErrors from "../../features/errors/TestError";
import NotFound from "../../features/errors/NotFound";
import ServerError from "../../features/errors/ServerError";
import ProfilePage from "../../features/profiles/ProfilePage";
import RequireAuth from "./RequireAuth";

// Define the application's route configuration
export const routes: RouteObject[] = [
  {
    path: "/", // Root path of the application
    element: <App />, // Main application layout
    children: [
      {
        element: <RequireAuth />, // Protect routes that require authentication
        children: [
          { path: "", element: <HomePage /> }, // Home page
          { path: "activities", element: <ActivityDashboard /> }, // Activities dashboard
          { path: "activities/:id", element: <ActivityDetalis /> }, // Activity details page
          { path: "createActivity", element: <ActicityFrom key="create" /> }, // Create activity form
          { path: "manage/:id", element: <ActicityFrom key="manage" /> }, // Manage activity form
          { path: "profiles/:username", element: <ProfilePage /> }, // User profile page
        ],
      },
      { path: "errors", element: <TestErrors /> }, // Test error page
      { path: "not-found", element: <NotFound /> }, // 404 Not Found page
      { path: "server-error", element: <ServerError /> }, // Server error page
      { path: "*", element: <Navigate replace to="/not-found" /> }, // Redirect unknown routes to 404
    ],
  },
];

// Create and export the router instance
export const router = createBrowserRouter(routes);
