import React, { Component, PropTypes } from 'react';
import Dimensions from 'Dimensions';
import {
	StyleSheet,
	KeyboardAvoidingView,
	View,
	ActivityIndicator,
	TouchableOpacity,
	Image,
	Keyboard,
} from 'react-native';
import { Actions } from 'react-native-router-flux';

import UserInput from './UserInput';
import ButtonSubmit from './ButtonSubmit';
import SignupSection from './SignupSection';

import usernameImg from '../images/username.png';
import passwordImg from '../images/password.png';
import eyeImg  from '../images/eye_black.png';
export default class Form extends Component {
	constructor(props) {
    super(props);
    this.state = {
			showPass: true,
			press: false,
			username: '',
			password: '',
			visibleHeight: Dimensions.get('window').height -450
		};
		this.showPass = this.showPass.bind(this);
	}

	showPass() {
  this.state.press === false ? this.setState({ showPass: false, press: true }) :this.setState({ showPass: true, press: false });
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
					placeholder='Username'
					autoCapitalize={'none'}
					returnKeyType={'done'}
					autoCorrect={false}
					onChangeText={(text) => this.setState({username: text})}/>
				<UserInput source={passwordImg}
					secureTextEntry={this.state.showPass}
					placeholder='Password'
					returnKeyType={'done'}
					autoCapitalize={'none'}
					autoCorrect={false}
					onChangeText={(text) => this.setState({password: text})} />
					<TouchableOpacity
						activeOpacity={0.7}
						style={styles.btnEye}
						onPress={this.showPass}
					>
						<Image source={eyeImg} style={styles.iconEye} />
					</TouchableOpacity>
					<ButtonSubmit username={this.state.username} password={this.state.password}/>
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
		alignItems: 'center',
	},
	btnEye: {
    position: 'absolute',
    top: 60,
    right: 28,
  },
  iconEye: {
    width: 25,
    height: 25,
    tintColor: 'rgba(0,0,0,0.2)',
  },
});
