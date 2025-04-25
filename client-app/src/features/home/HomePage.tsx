// Import necessary modules and components
import { Link } from "react-router-dom";
import {
  Container,
  Header,
  Segment,
  Image,
  Button,
  Divider,
} from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import { observer } from "mobx-react-lite";
import LoginForm from "../users/LoginForm";
import RegsiterForm from "../users/RegsiterForm";
import FacebookLogin, {
  FailResponse,
  SuccessResponse,
} from "@greatsumini/react-facebook-login";

// HomePage component displays the main landing page
export default observer(function HomePage() {
  const { userStore, modalStore } = useStore(); // Access user and modal store from MobX

  return (
    <Segment inverted textAlign="center" vertical className="masthead">
      {" "}
      {/* Main page layout */}
      <Container text>
        <Header as="h1" inverted>
          {" "}
          {/* App title and logo */}
          <Image
            size="massive"
            src="/assets/logo.png"
            alt="logo"
            style={{ marginBottom: 12 }}
          />
          Reactivities
        </Header>
        {userStore.isLoggedIn ? ( // Conditional rendering based on login status
          <>
            <Header as="h2" inverted content="welcome Reactivities" />{" "}
            {/* Welcome message */}
            <Button as={Link} to="/activities" size="huge" inverted>
              {" "}
              {/* Navigation button */}
              go to Activity!
            </Button>
          </>
        ) : (
          <>
            <Button
              onClick={() => modalStore.openModal(<LoginForm />)}
              size="huge"
              inverted
            >
              {" "}
              {/* Open login modal */}
              login!
            </Button>
            <Button
              onClick={() => modalStore.openModal(<RegsiterForm />)}
              size="huge"
              inverted
            >
              {" "}
              {/* Open register modal */}
              register
            </Button>
            <Divider horizontal inverted>
              Or
            </Divider>{" "}
            {/* Divider for alternative login */}
            <FacebookLogin
              appId={"221460500899348"} // Facebook App ID
              onSuccess={(respons: SuccessResponse) => {
                // Handle successful Facebook login
                userStore.facebookLogin(respons.accessToken);
                console.log("login success", respons);
              }}
              onFail={(respons: FailResponse) => {
                // Handle failed Facebook login
                console.log("Login Failed", respons);
              }}
              className={`ui button facebook huge inverted" ${
                userStore.fbLoading && "loading"
              }`} // Facebook login button
            />
          </>
        )}
      </Container>
    </Segment>
  );
});
