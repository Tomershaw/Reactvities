import { Grid, GridColumn } from "semantic-ui-react";
import ProfileHeader from "./ProfileHeadar";
import ProfileContent from "./ProfileContent";
import { observer } from "mobx-react-lite";
import { useStore } from "../../app/stores/store";
import { useEffect } from "react";
import LoadingComponent from "../../app/layout/loadingComponent";
import { useParams } from "react-router-dom";

export default observer(function ProfilePage() {

    const { username } = useParams<{ username: string }>();
    const { profileStore } = useStore();
    const { loadingProfile, loadProfile, profile } = profileStore

    useEffect(() => {
        if (username)
            loadProfile(username);
    }, [loadProfile, username])

    if (loadingProfile) return <LoadingComponent content="Loading profile..." />
    return (
        <Grid>
            <Grid.Column with={16}>
                {profile &&
                    <>
                        <ProfileHeader profile={profile} />
                        <ProfileContent profile={profile} />
                    </>}
            </Grid.Column>
        </Grid>
    )
})