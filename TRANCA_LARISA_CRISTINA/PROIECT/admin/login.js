import React from 'react';
import {Col, Row,Grid} from 'react-native-easy-grid';
import { Dimensions, AppRegistry, StyleSheet, View, Image, Platform } from 'react-native';
import { Container, Header, Content, Form, Item, Input, Label, Button, Text, Title, Left, Right, Body, Thumbnail, Fab, Toast } from 'native-base';
export class Login extends React.Component {
  render() {
    const navigation = this.props.navigation;
    return (
      <Container>
      <Content style = {styles.container}>
      <Grid style = { styles.grid}>
      <Col>
      <Image source = {require('./admin_logo.png')} style = {{width: 350,height: 100}}/>
      <Form>
      <Item floatingLabel>
      <Label> Username </Label>
      <Input/>
      </Item>
      <Item floatingLabel last >
      <Label> Password </Label>
      <Input secureTextEntry = {true}/>
      </Item>
      <Button full style = {{alignItems: 'center'}} onPress = {() => navigation.navigate('map')} >
      <Text > Log in < /Text>
      </Button>
      </Form>
      </Col>
      </Grid>
      </Content>
      </Container>
    );
  }
}

const styles = {
  container: {
    backgroundColor: 'rgba(118, 229, 199, 0.28)'
  }
};
