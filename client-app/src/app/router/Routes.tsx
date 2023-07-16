import { createBrowserRouter,RouteObject } from "react-router-dom";
import App from "../layout/App";
import { Children } from "react";
import HomePage from "../../features/home/HomePage";
import ActivityDashboard from "../../features/activities/dashboard/ActivityDashboard";
import ActicityFrom from "../../features/activities/form/ActicityFrom";
import ActivityDetalis from "../../features/activities/details/ActivityDetalis";

export const routes: RouteObject[]=[
    {
        path: '/',
        element: <App />,
        children: [
            {path: '', element: <HomePage />},
            {path: 'activities', element: <ActivityDashboard />},
            {path: 'activities/:id', element: <ActivityDetalis />},
            {path: 'createActivity', element: <ActicityFrom key='create' />},
            {path: 'manage/:id', element: <ActicityFrom key="manage" />},
        ]   
    }
]

export const router = createBrowserRouter(routes);


