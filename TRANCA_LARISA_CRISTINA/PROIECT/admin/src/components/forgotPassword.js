import React, { Component, PropTypes } from 'react';
import Logo from './Logo';
import Wallpaper from './Wallpaper';
import ForgotPasswordForm from './ForgotPasswordForm';
import SecondScreen from './SecondScreen';

export default class forgotPassword extends Component {
	render() {
		return (
			<Wallpaper>
				<Logo />
				<ForgotPasswordForm/>
				<SecondScreen/>
			</Wallpaper>
		);
	}
}
