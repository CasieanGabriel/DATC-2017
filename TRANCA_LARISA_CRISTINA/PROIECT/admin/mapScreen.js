import React from 'react';
import { Dimensions, AppRegistry, StyleSheet, View, Image, Platform } from 'react-native';
import { Container, Header, Content, Form, Item, Input, Label, Button, Text, Title, Left, Right, Body, Thumbnail, Fab, Toast } from 'native-base';
import MapView from 'react-native-maps';
export class mapScreen extends React.Component {
    state = {
        latitude: 20.9948891,
        longitude: 105.799677,
        latitudeDelta: 0.021,
        longitudeDelta: 0.021
    }

    render() {
        return (
            <View style={styles.container}>
                <MapView style={styles.map} initialRegion={this.state}>
                    <MapView.Marker coordinate={this.state} />
                </MapView>
            </View>
        );
    }
}
const width = Dimensions.get('window').width;
const height = Dimensions.get('window').height;

const styles = {
    container: {
        justifyContent: 'center',
        alignItems: 'center',
        backgroundColor: '#fff'
    },
    text: {
        fontSize: 30,
        fontWeight: '700',
        color: '#59656C',
        marginBottom: 10,
    },
    map: {
      width,
      height
    }
};
