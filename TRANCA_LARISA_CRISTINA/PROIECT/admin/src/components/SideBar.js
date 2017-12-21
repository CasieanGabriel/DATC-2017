import React, { Component } from 'react';
import { Container, Header, Content, Button } from 'native-base';
export default class SideBar extends Component {
  render() {
    return (
      <Container>
        <Header />
        <Content>
          <Button ref={ (c) => this._button = c }>
            Click Me
          </Button>
        </Content>
      </Container>
    );
  }
}
