import React, { Component } from 'react';
import { View, ScrollView, Text, TouchableOpacity, 	Animated, Easing,StyleSheet, Image } from 'react-native';
import Logo from './Logo';
import Wallpaper from './Wallpaper';
import { Actions, ActionConst } from 'react-native-router-flux';

const menuList = require('./Constants.js');

export default class Menu extends Component {

    _onPress() {
      Actions.loginScreen({prop:''});
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
                <Image source={item.image} style={styles.inlineIcon} />
                <Text style={{color: 'white', fontSize: 16, paddingLeft: 40, paddingTop: 16}}>{item.name}</Text>
              </TouchableOpacity>
            ))}
          </ScrollView>
      </Wallpaper>
      </View>
    );
  }
}

const styles = StyleSheet.create({
  inlineIcon: {
    position: 'absolute',
    zIndex: 99,
    width: 22,
    height: 22,
    left: 15,
    top: 15,
    paddingLeft: 20, paddingTop: 16
  },
});
