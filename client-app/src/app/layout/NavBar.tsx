import React from 'react';
import { Button, Container, Menu } from 'semantic-ui-react';

interface Props{
    openform:()=>void;
}

export default function NavBar({openform}:Props){
    return(
        <Menu inverted fixed='top'>
            <Container>
                <Menu.Item header>
                    <img src="/assets/logo.png" alt="logo" style={{marginRight: "10px"}}/>
                    Reactivitiy
                </Menu.Item>
                <Menu.Item  name='Activity'/>
                <Menu.Item>
                    <Button onClick={openform}  positive content="Create Activity"/>
                </Menu.Item>
            </Container>
        </Menu>
    )
}