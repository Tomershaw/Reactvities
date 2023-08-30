import { ErrorMessage, Form, Formik } from "formik";
import { observe, values } from "mobx";
import { Default } from "react-toastify/dist/utils";
import MyTextInput from "../../app/common/form/MyTextInput";
import { Button, Divider, Header, Label } from "semantic-ui-react";
import {useStore} from '../../app/stores/store'
import { observer } from "mobx-react-lite";
import { error } from "console";
import FacebookLogin from "@greatsumini/react-facebook-login";

export default observer(function LoginForm() {
   const {userStore} = useStore();
    return (
        <Formik
            initialValues={{ email: '', password: '',error:null }}
            onSubmit={(values,{setErrors}) => userStore.login(values).catch(error => setErrors({error:'Invalid email or password'}))}
        >
            {({ handleSubmit, isSubmitting,errors }) => (
                <Form className="ui form" onSubmit={handleSubmit} autoComplete="off" >
                    <Header  as ='h2' content='login to Reactivities' color="teal" textAlign="center" />
                    <MyTextInput placeholder="Email" name='email' />
                    <MyTextInput placeholder="Password" name='password' type='password' />
                    <ErrorMessage 
                    name='error' render={() => <Label style={{marginBottom:10}} basic color='red' content ={errors.error}/>}
                    />
                    <Button loading={isSubmitting} positive content='Login' type="submit" fluid />
                </Form>
            )}

        </Formik>

    )
})