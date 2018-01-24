/**
 * Sample React Native App
 * https://github.com/facebook/react-native
 * @flow
 */
import Icon from 'react-native-vector-icons/FontAwesome';
import React, { Component } from 'react';
import { StackNavigator, } from 'react-navigation';
import { Container, Header, Content, Form, Item, Input, Label, Button, Text, Title, Left, Right, Body, Thumbnail, Fab, Toast } from 'native-base';
import { Col, Row, Grid } from 'react-native-easy-grid';
import {
  Platform,
  StyleSheet,
  View,
  Image
} from 'react-native';
// class AnimatedWithChildren extends Animated {
//        // Internal class, no public API.
//      }
// export class map extends AnimatedWithChildren{
//   state = {
//       markers: [
//         {
//           coordinate: {
//             latitude: 45.524548,
//             longitude: -122.6749817,
//           },
//           title: "Best Place",
//           description: "This is the best place in Portland",
//           image: Images[0],
//         },
//         {
//           coordinate: {
//             latitude: 45.524698,
//             longitude: -122.6655507,
//           },
//           title: "Second Best Place",
//           description: "This is the second best place in Portland",
//           image: Images[1],
//         },
//         {
//           coordinate: {
//             latitude: 45.5230786,
//             longitude: -122.6701034,
//           },
//           title: "Third Best Place",
//           description: "This is the third best place in Portland",
//           image: Images[2],
//         },
//         {
//           coordinate: {
//             latitude: 45.521016,
//             longitude: -122.6561917,
//           },
//           title: "Fourth Best Place",
//           description: "This is the fourth best place in Portland",
//           image: Images[3],
//         },
//       ],
//       region: new MapView.AnimatedRegion({
//         latitude: LATITUDE,
//         longitude: LONGITUDE,
//         latitudeDelta: LATITUDE_DELTA,
//         longitudeDelta: LONGITUDE_DELTA,
//       }),
//     };
// }
const HomeScreen = ({ navigation }) => (
  <Container>
    <Content style={styles.container}>
    <Grid style={styles.grid}>
    <Col>
    <Image source={require('./admin_logo.png')} style={{width: 350, height: 100}}/>
      <Form>
        <Item floatingLabel>
          <Label>Username</Label>
          <Input />
        </Item>
        <Item floatingLabel last>
          <Label>Password</Label>
          <Input secureTextEntry={true}/>
        </Item>
        <Button full style={{alignItems:'center'}}  onPress={() => navigation.navigate('Details') }>
        <Text>Log in</Text>
      </Button>
      </Form>
      </Col>
      </Grid>
    </Content>
  </Container>
);
const DetailsScreen = () => (
  <View style={{ flex: 1, alignItems: 'center', justifyContent: 'center' }}>

  </View>
);
const RootNavigator = StackNavigator({
  Home: {
    screen: HomeScreen,
    navigationOptions: {
      headerTitle: 'Home',
    },
  },
  Details: {
    screen: DetailsScreen,
    navigationOptions: {
      headerTitle: 'Map',
    },
  },
});

export default RootNavigator;
const instructions = Platform.select({
  ios: 'Press Cmd+R to reload,\n' +
    'Cmd+D or shake for dev menu',
  android: 'Double tap R on your keyboard to reload,\n' +
    'Shake or press menu button for dev menu',
});

export  class App extends Component {
  static navigationOptions = { title: 'Welcome', };
  render() {

    return HomeScreen;
  }
}

const styles = StyleSheet.create({
  grid: {
      justifyContent: 'center',
      alignItems: 'center'
    },
    container: {
      backgroundColor: 'rgba(118, 229, 199, 0.28)',
    }
});
