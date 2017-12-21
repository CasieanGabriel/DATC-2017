import React, { Component, PropTypes } from 'react';
import Dimensions from 'Dimensions';
import {
	StyleSheet,
	KeyboardAvoidingView,
	View,
	ActivityIndicator,
	TouchableOpacity,
	Image,
	Text,
	Animated,
	Easing,
	Keyboard
} from 'react-native';
import { Actions, ActionConst } from 'react-native-router-flux';
import UserInput from './UserInput';
import ButtonSubmit from './ButtonSubmit';
import SignupSection from './SignupSection';

import usernameImg from '../images/username.png';
import passwordImg from '../images/password.png';
import eyeImg  from '../images/eye_black.png';
import arrowImg from '../images/left-arrow.png';
const SIZE = 40;
export default class ForgotPasswordForm extends Component {
	constructor(props) {
    super(props);
    this.state = {
			showPass: true,
			press: false,
			isLoading: false,
			visibleHeight: Dimensions.get('window').height -450
		};
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
	        <Text style={styles.textMessage}> Please type your e-mail and we will send you the new password on e-mail. And after you recive the new password login in the app with the new password.</Text>
					<UserInput source={usernameImg}
						placeholder='E-mail'
						autoCapitalize={'none'}
						returnKeyType={'done'}
						autoCorrect={false} />
						<Text style={styles.text}>Thank you, ADMIN team.</Text>
				</KeyboardAvoidingView>
			</View>
		);
	}
}

const DEVICE_WIDTH = Dimensions.get('window').width;
const DEVICE_HEIGHT = Dimensions.get('window').height;

const styles = StyleSheet.create({
	container: {
		// top:-150,
		flex: 2,
		alignItems: 'center',
		width: DEVICE_WIDTH,
		height: DEVICE_HEIGHT,
	},
  text: {
    color: 'white',
    backgroundColor: 'transparent',
		alignItems: 'center',
		// top:45,
  },
	textMessage: {
		color: 'white',
    backgroundColor: 'transparent',
		textAlign: 'center', // <-- the magic
    fontWeight: 'bold',
    fontSize: 16,
		// top: -100,
	}
});
