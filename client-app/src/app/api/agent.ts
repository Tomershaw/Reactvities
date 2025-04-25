// Importing necessary modules and types for API requests and responses
import axios, { AxiosError, AxiosResponse } from "axios";
import { Activity, ActivityFormValues } from "../models/activity";
import { toast } from "react-toastify";
import { router } from "../router/Routes";
import { store } from "../stores/store";
import { User, UserFormValues } from "../models/user";
import Photo, { Profile, UserActivity } from "../models/profile";
import { PaginatedResult } from "../models/pagination";

// Utility function to simulate a delay (useful for development purposes)
const sleep = (delay: number) => {
  return new Promise(resolve => {
    setTimeout(resolve, delay);
  });
};

// Setting the base URL for Axios requests from environment variables
axios.defaults.baseURL = import.meta.env.VITE_API_URL;

// Helper function to extract the response body from Axios responses
const responseBody = <T>(response: AxiosResponse<T>) => response.data;

// Adding a request interceptor to include the Authorization token in headers
axios.interceptors.request.use(config => {
  const token = store.commonStore.token;
  if (token && config.headers) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

// Adding a response interceptor to handle delays, pagination, and errors
axios.interceptors.response.use(
  async response => {
    // Simulate a delay in development mode
    if (import.meta.env.DEV) await sleep(1000);

    // Handle pagination headers if present
    const pagination = response.headers["pagination"];
    if (pagination) {
      response.data = new PaginatedResult(
        response.data,
        JSON.parse(pagination)
      );
      return response as AxiosResponse<PaginatedResult<unknown>>;
    }
    return response;
  },
  (error: AxiosError) => {
    // Extracting error details from the response
    const { data, status, config, headers } = error.response as AxiosResponse;
    switch (status) {
      case 400:
        // Handle validation errors or navigation for missing resources
        if (
          config.method === "get" &&
          Object.prototype.hasOwnProperty.call(data.errors, "id")
        ) {
          router.navigate("/not-found");
        }
        if (data.errors) {
          const modalStateErros = [];
          for (const key in data.errors) {
            modalStateErros.push(data.errors[key]);
          }
          throw modalStateErros.flat();
        } else {
          toast.error(data);
        }
        break;
      case 401:
        // Handle unauthorized errors and session expiration
        if (
          status === 401 &&
          headers["www-authenticate"]?.startsWith(
            'Bearer error="invalid_token"'
          )
        ) {
          store.userStore.logout();
          toast.error("Session expired - please login again");
        } else {
          toast.error("unauthorised");
        }
        break;
      case 403:
        // Handle forbidden errors
        toast.error("forbidden");
        break;
      case 404:
        // Navigate to not-found page for 404 errors
        router.navigate("/not-found");
        break;
      case 500:
        // Handle server errors and navigate to server-error page
        store.commonStore.setServerError(data);
        router.navigate("/server-error");
        break;
    }
    return Promise.reject(error);
  }
);

// Utility object for making HTTP requests with predefined methods
const requests = {
  get: <T>(url: string) => axios.get<T>(url).then(responseBody),
  post: <T>(url: string, body: object) =>
    axios.post<T>(url, body).then(responseBody),
  put: <T>(url: string, body: object) =>
    axios.put<T>(url, body).then(responseBody),
  del: <T>(url: string) => axios.delete<T>(url).then(responseBody),
};

// API endpoints for managing activities
const Activities = {
  list: (params: URLSearchParams) =>
    axios
      .get<PaginatedResult<Activity[]>>("/activities", { params })
      .then(responseBody),
  details: (id: string) => requests.get<Activity>(`/activities/${id}`),
  create: (activity: ActivityFormValues) =>
    requests.post<void>(`/activities`, activity),
  update: (activity: ActivityFormValues) =>
    requests.put<void>(`/activities/${activity.id}`, activity),
  delete: (id: string) => requests.del<void>(`/activities/${id}`),
  attend: (id: string) => requests.post<void>(`/activities/${id}/attend`, {}),
};

// API endpoints for account-related operations
const Account = {
  current: () => requests.get<User>("account"),
  login: (user: UserFormValues) => requests.post<User>("/account/login", user),
  regsiter: (user: UserFormValues) =>
    requests.post<User>("/account/register", user),
  fbLogin: (accessToken: string) =>
    requests.post<User>(`/account/fbLogin?accessToken=${accessToken}`, {}),
  refreshToken: () => requests.post<User>(`/account/refreshToken`, {}),
};

// API endpoints for managing user profiles
const Profiles = {
  get: (username: string) => requests.get<Profile>(`profiles/${username}`),
  uploadPhoto: (file: Blob) => {
    const formData = new FormData();
    formData.append("file", file);
    return axios.post<Photo>("photos", formData, {
      headers: { "Content-Type": "multipart/form-data" },
    });
  },
  setMainPhoto: (id: string) => requests.post(`/photos/${id}/setMain`, {}),
  deletePhoto: (id: string) => requests.del(`/photos/${id}`),
  updateFollowing: (username: string) =>
    requests.post(`/follow/${username}`, {}),
  ListFollowing: (username: string, predicate: string) =>
    requests.get<Profile[]>(`/follow/${username}?predicate=${predicate}`),
  listActivities: (username: string, predicate: string) =>
    requests.get<UserActivity[]>(
      `/profiles/${username}/activities?predicate=${predicate}`
    ),
};

// Exporting the agent object containing all API endpoints
const agent = {
  Activities,
  Account,
  Profiles,
};

export default agent;
