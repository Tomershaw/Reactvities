// Import necessary components from Semantic UI React and other libraries
import { Button, Container, Dropdown, Image, Menu } from "semantic-ui-react";
import { Link, NavLink } from "react-router-dom";
import { useStore } from "../stores/store";
import { observer } from "mobx-react-lite";

// Functional component for the navigation bar
export default observer(function NavBar() {
  // Extract user and logout function from the store
  const {
    userStore: { user, logout },
  } = useStore();

  return (
    // Main navigation menu with fixed position and inverted style
    <Menu inverted fixed="top">
      <Container>
        {/* Logo and home link */}
        <Menu.Item as={NavLink} to="/" header>
          <img
            src="/assets/logo.png"
            alt="logo"
            style={{ marginRight: "10px" }}
          />
          Reactivitiy
        </Menu.Item>
        {/* Link to activities page */}
        <Menu.Item as={NavLink} to="/activities" name="Activity" />
        {/* Link to errors page */}
        <Menu.Item as={NavLink} to="/errors" name="Errors" />
        {/* Button to create a new activity, visible only if the user has permission */}
        {user?.canCreateActivity && (
          <Menu.Item>
            <Button
              as={NavLink}
              to="/CreateActivity"
              positive
              content="Create Activity"
            />
          </Menu.Item>
        )}
        {/* User profile and logout dropdown menu */}
        <Menu.Item position="right">
          <Image
            src={user?.image || "/assets/user.png"}
            avatar
            spaced="right"
          />
          <Dropdown pointing="top left" text={user?.displayName}>
            <Dropdown.Menu>
              {/* Link to the user's profile */}
              <Dropdown.Item
                as={Link}
                to={`/profiles/${user?.username}`}
                text="My Profile"
                icon="user"
              />
              {/* Logout option */}
              <Dropdown.Item onClick={logout} text="Logout" icon="power" />
            </Dropdown.Menu>
          </Dropdown>
        </Menu.Item>
      </Container>
    </Menu>
  );
});
