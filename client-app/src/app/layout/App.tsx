// Import necessary components and libraries.
import { Container } from "semantic-ui-react";
import NavBar from "./NavBar";
import { observer } from "mobx-react-lite";
import { Outlet, ScrollRestoration, useLocation } from "react-router-dom";
import HomePage from "../../features/home/HomePage";
import { ToastContainer } from "react-toastify";
import { useStore } from "../stores/store";
import LoadingComponent from "./loadingComponent";
import ModalContainer from "../common/modals/ModalContainer";
import { useEffect } from "react";

function App() {
  // Get the current location from the router.
  const location = useLocation();
  // Access commonStore and userStore from the application's store.
  const { commonStore, userStore } = useStore();

  useEffect(() => {
    // Load the user if a token exists, otherwise mark the app as loaded.
    if (commonStore.token) {
      userStore.getUser().finally(() => commonStore.setAppLoaded());
    } else {
      commonStore.setAppLoaded();
    }
  }, [commonStore, userStore]);

  // Show a loading component until the app is fully loaded.
  if (!commonStore.appLoaded)
    return <LoadingComponent content="Loading app..." />;

  return (
    <>
      {/* Restore scroll position on navigation. */}
      <ScrollRestoration />
      {/* Display modals managed by the modal store. */}
      <ModalContainer />
      {/* Toast notifications for user feedback. */}
      <ToastContainer position="bottom-right" hideProgressBar theme="colored" />
      {location.pathname === "/" ? (
        // Render the home page for the root path.
        <HomePage />
      ) : (
        <>
          {/* Navigation bar for non-root paths. */}
          <NavBar />
          {/* Main content container with a top margin. */}
          <Container style={{ marginTop: "7em" }}>
            <Outlet />
          </Container>
        </>
      )}
    </>
  );
}

// Wrap the component with MobX observer to react to store changes.
export default observer(App);
