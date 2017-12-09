import React from 'react';
import { Dimensions, AppRegistry, StyleSheet, View, Image, Platform, Text } from 'react-native';
import {StackNavigator,TabNavigator} from 'react-navigation';
import {Login} from './login';
import { mapScreen} from './mapScreen';

 const Stack = StackNavigator({
  login: {
    screen: Login,
    navigationOptions: {
      title: 'Login'
    }
  },
  map: {
    screen: mapScreen,
    navigationOptions: {
      title: 'Map'
    }
  }
}, {
  swipeEnabled: false,
  animationEnabled: false
});
export default Stack;
AppRegistry.registerComponent("admin", () => App);
