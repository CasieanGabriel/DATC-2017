import React, { Component, PropTypes } from 'react';
import Dimensions from 'Dimensions';
import {
	StyleSheet,
	KeyboardAvoidingView,
	View,
	ActivityIndicator,
	TouchableOpacity,
	Image,
	Animated,
	Easing,
	Keyboard,
} from 'react-native';
import api from './api.js';
import { Actions, ActionConst } from 'react-native-router-flux';
import UserInput from './UserInput';
import SubmitCreateAccount from './SubmitCreateAccount';
import SignupSection from './SignupSection';

import usernameImg from '../images/username.png';
import passwordImg from '../images/password.png';
import eyeImg  from '../images/eye_black.png';
import arrowImg from '../images/left-arrow.png';
const SIZE = 40;
export default class AccountForm extends Component {
	constructor(props) {
    super(props);
    this.state = {
			showPass: true,
			showConfirmPass: true,
			press: false,
			pressConfirm:false,
			isLoading: false,
			visibleHeight: Dimensions.get('window').height -450
		};
		this.showPass = this.showPass.bind(this);
		this.showConfirmPass = this.showConfirmPass.bind(this);
	}
	showPass() {
  	this.state.press === false ? this.setState({ showPass: false, press: true }) :this.setState({ showPass: true, press: false });
  }
	showConfirmPass(){
		this.state.pressConfirm === false ? this.setState({ showConfirmPass: false, pressConfirm: true }) :this.setState({ showConfirmPass: true, pressConfirm: false });
	}
	componentWillMount () {
		Keyboard.addListener('keyboardWillShow', this.keyboardWillShow.bind(this))
		Keyboard.addListener('keyboardWillHide', this.keyboardWillHide.bind(this))
	}
	keyboardWillShow (e) {
		 let newSize = Dimensions.get('window').height - e.endCoordinates.height
		 this.setState({visibleHeight: newSize})
	 }

	 keyboardWillHide (e) {
		 this.setState({visibleHeight: Dimensions.get('window').height})
	 }

	render() {
		return (
			<View style={{height: this.state.visibleHeight}}>
				<KeyboardAvoidingView behavior='padding'
					style={styles.container}>
					<UserInput source={usernameImg}
						placeholder='Email'
						autoCapitalize={'none'}
						returnKeyType={'done'}
						autoCorrect={false}
						onChangeText={(text) => this.setState({email: text})}/>
					<UserInput source={passwordImg}
						secureTextEntry={this.state.showPass}
						placeholder='Password'
						returnKeyType={'done'}
						autoCapitalize={'none'}
						autoCorrect={false}
						onChangeText={(text) => this.setState({password: text})}/>
						<TouchableOpacity
							activeOpacity={0.7}
							style={styles.btnEye}
							onPress={this.showPass}
						>
							<Image source={eyeImg} style={styles.iconEye} />
						</TouchableOpacity>
	          <UserInput source={passwordImg}
	            secureTextEntry={this.state.showConfirmPass}
	            placeholder='Confirm Password'
	            returnKeyType={'done'}
	            autoCapitalize={'none'}
	            autoCorrect={false}
							onChangeText={(text) => this.setState({confirmPassword: text})}/>
	            <TouchableOpacity
							activeOpacity={0.7}
	              style={styles.btnEyeConfirm}
	              onPress={this.showConfirmPass}
	            >
	              <Image source={eyeImg} style={styles.iconEye} />
	            </TouchableOpacity>
							<SubmitCreateAccount email={this.state.email} password={this.state.password} confirmPassword={this.state.confirmPassword}/>
				</KeyboardAvoidingView>
			</View>
		);
	}
}

const DEVICE_WIDTH = Dimensions.get('window').width;
const DEVICE_HEIGHT = Dimensions.get('window').height;

const styles = StyleSheet.create({
	container: {
		flex: 2,
		margin: -20,
		alignItems: 'center',
	},
	btnEye: {
    position: 'absolute',
		top: 60,
		right: 48,
  },
	btnEyeConfirm: {
		position: 'absolute',
		top: 100,
		right: 48,
	},
  iconEye: {
    width: 25,
    height: 25,
    tintColor: 'rgba(0,0,0,0.2)',
  },
	image: {
		width: 24,
		height: 24,
	},
});
