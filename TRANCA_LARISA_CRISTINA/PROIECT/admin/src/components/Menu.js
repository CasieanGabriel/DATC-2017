import React, { Component } from 'react';
import { View, ScrollView, Text, TouchableOpacity, 	Animated, Easing, } from 'react-native';
import Logo from './Logo';
import Wallpaper from './Wallpaper';
import { Actions, ActionConst } from 'react-native-router-flux';

const menuList = require('./Constants.js');

export default class Menu extends Component {

    _onPress() {
      Actions.pop();
    };
  render() {
    return (
      <View style={{ flex:1, backgroundColor: 'rgba(255, 255, 255, 0.4)'}}>
      <Wallpaper>
        <Logo/>
        <ScrollView>
            {menuList.MENU_LIST.map(item => (
              <TouchableOpacity
                key={item.index}
                onPress={this._onPress}
              >
                <Text style={{color: 'white', fontSize: 16, paddingLeft: 20, paddingTop: 16}}>{item.name}</Text>
              </TouchableOpacity>
            ))}
          </ScrollView>
      </Wallpaper>
      </View>
    );
  }
}
