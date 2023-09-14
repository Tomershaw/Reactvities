import { Link } from "react-router-dom";
import { Container, Header, Segment, Image, Button, Divider } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import { observer } from "mobx-react-lite";
import LoginForm from "../users/LoginForm";
import RegsiterForm from "../users/RegsiterForm";
import FacebookLogin, { FailResponse, SuccessResponse } from "@greatsumini/react-facebook-login";


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
                        <Divider horizontal inverted>Or</Divider>
                        <FacebookLogin
                         appId={"221460500899348"}
                         onSuccess={(respons:SuccessResponse)=>{
                            userStore.facebookLogin(respons.accessToken)
                            console.log('login success',respons)
                         }}
                         onFail={(respons:FailResponse)=>{
                            console.log('Login Failed',respons)
                         }}
                         className={`ui button facebook huge inverted" ${userStore.fbLoading && 'loading'}`}
                        />

                    </>
                )}





            </Container>
        </Segment>

    )
})