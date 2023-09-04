import { createBrowserRouter,Navigate,RouteObject } from "react-router-dom";
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

export const routes: RouteObject[]=[
    {
        path: '/',
        element: <App />,
        children: [
            {element:<RequireAuth/>, children: [
                {path: '', element: <HomePage />},
                {path: 'activities', element: <ActivityDashboard />},
                {path: 'activities/:id', element: <ActivityDetalis />},
                {path: 'createActivity', element: <ActicityFrom key='create' />},
                {path: 'manage/:id', element: <ActicityFrom key="manage" />},
                {path: 'profiles/:username', element: <ProfilePage />},

            ]},
            // {path: 'login', element: <LoginForm />},
            {path: 'errors', element: <TestErrors />},
            {path: 'not-found', element: <NotFound />},
            {path: 'server-error', element: <ServerError />},
            {path: '*', element: <Navigate replace to= '/not-found' />},
        ]   
    }
]

export const router = createBrowserRouter(routes);


