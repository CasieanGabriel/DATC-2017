import React, { Component } from 'react';
import { Router, Scene, Actions, ActionConst } from 'react-native-router-flux';

import LoginScreen from './LoginScreen';
import SecondScreen from './SecondScreen';
import {mapScreen} from './mapScreen';
import createAccount from './createAccount';
import forgotPassword from './forgotPassword';

export default class Main extends Component {
  render() {
	  return (
	    <Router>
	      <Scene key="root">
	        <Scene key="loginScreen"
	          component={LoginScreen}
	        	animation='fade'
	          hideNavBar={true}
	          initial={true}
	        />
	        <Scene key="mapScreen"
	          component={mapScreen}
	          animation='fade'
	          hideNavBar={true}
	        />
          <Scene key="createAccount"
            component={createAccount}
            animation='fade'
            hideNavBar={true}
          />
          <Scene key="forgotPassword"
            component={forgotPassword}
            animation='fade'
            hideNavBar={true}
          />
	      </Scene>
	    </Router>
	  );
	}
}
