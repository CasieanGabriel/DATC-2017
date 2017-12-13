import React from 'react';
import { Dimensions, AppRegistry, StyleSheet, View, Image, Platform, Text } from 'react-native';
import {StackNavigator,TabNavigator} from 'react-navigation';
import {Login} from './login';
import LoginScreen from './src/components/LoginScreen';
import Main from './src/components/Main';

//  const Stack = StackNavigator({
//   login: {
//     screen: LoginScreen,
//     navigationOptions: {
//       title: 'Login'
//     }
//   },
//   map: {
//     screen: mapScreen,
//     navigationOptions: {
//       title: 'Map'
//     }
//   }
// }, {
//   swipeEnabled: false,
//   animationEnabled: false
// });
// export default Stack;

export default class App extends React.Component  {
  render() {
    return (
      <View style={styles.container}>
        <Main />
      </View>
    );
  }
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#F5FCFF',
  },
});

AppRegistry.registerComponent("admin", () => App);
