import { Link } from "react-router-dom";
import { Container, Header, Segment, Image, Button } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import { observer } from "mobx-react-lite";
import LoginForm from "../users/LoginForm";
import RegsiterForm from "../users/RegsiterForm";


export default observer(function HomePage() {
    const { userStore,modalStore } = useStore();
    return (
        <Segment inverted textAlign="center" vertical className="masthead">
            <Container text>
                <Header as='h1' inverted>
                    <Image size="massive" src="/assets/logo.png" alt='logo' style={{ marginBottom: 12 }} />
                    Reactivities
                </Header>
                {userStore.isLoggedIn ? (
                    <>
                        <Header as='h2' inverted content='welcome Reactivities' />

                        <Button as={Link} to="/activities" size="huge" inverted>
                            go to Activity!
                        </Button>
                    </>

                ) : (
                    <>
                        <Button onClick={() => modalStore.openModal(<LoginForm />)} size="huge" inverted>
                            login!
                        </Button>
                        <Button onClick={() => modalStore.openModal(<RegsiterForm />)} size="huge" inverted>
                            register
                        </Button>
                    </>
                )}





            </Container>
        </Segment>

    )
})