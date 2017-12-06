import React from 'react';
import { Dimensions, AppRegistry, StyleSheet, View, Image, Platform, ListView, TouchableOpacity } from 'react-native';
import { Container, Header, Content, Form, Item, Input, Label, Button, Text, Title, Left, Right, Body, Thumbnail, Fab, Toast } from 'native-base';
import MapView from 'react-native-maps';
import api from './api';

function randomColor() {
  return `#${Math.floor(Math.random() * 16777215).toString(16)}`;
}
let id = 0;
export class mapScreen extends React.Component {

  constructor(props){
    super(props);
    this.state = {
      resource: [],
      region: {
        latitude: 45.75999832,
        longitude: 21.22999954,
        latitudeDelta: 0.0021,
        longitudeDelta: 0.0021
      },
      marker: {
        coordinate: {
          latitude: 45.75999832,
          longitude: 21.22999954,
          latitudeDelta: 0.0021,
          longitudeDelta: 0.0021
        }
      }
    }
    this.onRegionChange = this.onRegionChange.bind(this);
    this.jumpRandom = this.jumpRandom.bind(this);
    this.onMapPress = this.onMapPress.bind(this);
  }

  componentWillMount(){
    api.getResource().then((res) => {
      this.setState({
        resource: res
      })
    });
  }
  show() {
      this.state.marker.showCallout();
    }

    hide() {
      this.state.marker.hideCallout();
    }
  onMapPress(e) {
  this.setState({
    marker:
      {
        coordinate: e.nativeEvent.coordinate,
        color: randomColor()
      }
    //   },
    // region:
    //   e.nativeEvent.coordinate
    // ,
  });
}
    onRegionChange(region) {
      this.setState({ region: region });
    }
    jumpRandom() {
      this.setState({ region: this.randomRegion() });
    }
    animateRandom() {
    this.map.animateToRegion(this.randomRegion());
  }

  animateRandomCoordinate() {
    this.map.animateToCoordinate(this.randomCoordinate());
  }

  animateToRandomBearing() {
    this.map.animateToBearing(this.getRandomFloat(-360, 360));
  }

  animateToRandomViewingAngle() {
    this.map.animateToViewingAngle(this.getRandomFloat(0, 90));
  }

  getRandomFloat(min, max) {
    return (Math.random() * (max - min)) + min;
  }

  randomCoordinate() {
    const region = this.state.region;
    return {
      latitude: region.latitude + ((Math.random() - 0.5) * (region.latitudeDelta / 2)),
      longitude: region.longitude + ((Math.random() - 0.5) * (region.longitudeDelta / 2)),
    };
  }

  randomRegion() {
    return {
      ...this.state.region,
      ...this.randomCoordinate(),
    };
  }
    getInitialState() {
  return {
    region: {
      latitude: 45.75999832,
      longitude: 21.22999954,
      latitudeDelta: 0.0021,
      longitudeDelta: 0.0021
    },
  };
}
    coordinate = {
        latitude: 45.75999832,
        longitude: 21.22999954,
        latitudeDelta: 0.0021,
        longitudeDelta: 0.0021
    };
    mapStyle = [
  {
    "elementType": "geometry",
    "stylers": [
      {
        "color": "#1d2c4d"
      }
    ]
  },
  {
    "elementType": "labels.text.fill",
    "stylers": [
      {
        "color": "#8ec3b9"
      }
    ]
  },
  {
    "elementType": "labels.text.stroke",
    "stylers": [
      {
        "color": "#1a3646"
      }
    ]
  },
  {
    "featureType": "administrative.country",
    "elementType": "geometry.stroke",
    "stylers": [
      {
        "color": "#4b6878"
      }
    ]
  },
  {
    "featureType": "administrative.land_parcel",
    "elementType": "labels.text.fill",
    "stylers": [
      {
        "color": "#64779e"
      }
    ]
  },
  {
    "featureType": "administrative.province",
    "elementType": "geometry.stroke",
    "stylers": [
      {
        "color": "#4b6878"
      }
    ]
  },
  {
    "featureType": "landscape.man_made",
    "elementType": "geometry.stroke",
    "stylers": [
      {
        "color": "#334e87"
      }
    ]
  },
  {
    "featureType": "landscape.natural",
    "elementType": "geometry",
    "stylers": [
      {
        "color": "#023e58"
      }
    ]
  },
  {
    "featureType": "poi",
    "elementType": "geometry",
    "stylers": [
      {
        "color": "#283d6a"
      }
    ]
  },
  {
    "featureType": "poi",
    "elementType": "labels.text.fill",
    "stylers": [
      {
        "color": "#6f9ba5"
      }
    ]
  },
  {
    "featureType": "poi",
    "elementType": "labels.text.stroke",
    "stylers": [
      {
        "color": "#1d2c4d"
      }
    ]
  },
  {
    "featureType": "poi.park",
    "elementType": "geometry.fill",
    "stylers": [
      {
        "color": "#023e58"
      }
    ]
  },
  {
    "featureType": "poi.park",
    "elementType": "labels.text.fill",
    "stylers": [
      {
        "color": "#3C7680"
      }
    ]
  },
  {
    "featureType": "road",
    "elementType": "geometry",
    "stylers": [
      {
        "color": "#304a7d"
      }
    ]
  },
  {
    "featureType": "road",
    "elementType": "labels.text.fill",
    "stylers": [
      {
        "color": "#98a5be"
      }
    ]
  },
  {
    "featureType": "road",
    "elementType": "labels.text.stroke",
    "stylers": [
      {
        "color": "#1d2c4d"
      }
    ]
  },
  {
    "featureType": "road.highway",
    "elementType": "geometry",
    "stylers": [
      {
        "color": "#2c6675"
      }
    ]
  },
  {
    "featureType": "road.highway",
    "elementType": "geometry.stroke",
    "stylers": [
      {
        "color": "#255763"
      }
    ]
  },
  {
    "featureType": "road.highway",
    "elementType": "labels.text.fill",
    "stylers": [
      {
        "color": "#b0d5ce"
      }
    ]
  },
  {
    "featureType": "road.highway",
    "elementType": "labels.text.stroke",
    "stylers": [
      {
        "color": "#023e58"
      }
    ]
  },
  {
    "featureType": "transit",
    "elementType": "labels.text.fill",
    "stylers": [
      {
        "color": "#98a5be"
      }
    ]
  },
  {
    "featureType": "transit",
    "elementType": "labels.text.stroke",
    "stylers": [
      {
        "color": "#1d2c4d"
      }
    ]
  },
  {
    "featureType": "transit.line",
    "elementType": "geometry.fill",
    "stylers": [
      {
        "color": "#283d6a"
      }
    ]
  },
  {
    "featureType": "transit.station",
    "elementType": "geometry",
    "stylers": [
      {
        "color": "#3a4762"
      }
    ]
  },
  {
    "featureType": "water",
    "elementType": "geometry",
    "stylers": [
      {
        "color": "#0e1626"
      }
    ]
  },
  {
    "featureType": "water",
    "elementType": "labels.text.fill",
    "stylers": [
      {
        "color": "#4e6d70"
      }
    ]
  }
];
    render() {
        return (
            <View style={styles.container}>
                <MapView style={styles.map} initialRegion = {this.coordinate} region={this.state.region} onRegionChange={this.onRegionChange}
                customMapStyle={this.mapStyle} ref={ref => { this.map = ref; }} onPress={(e) => this.onMapPress(e)}
                >
                  <MapView.Marker
                    coordinate={this.state.marker.coordinate}
                    pinColor={this.state.marker.color}
                  />
                </MapView>
                <View style={[styles.bubble, styles.latlng]}>
                  <Text style={{ textAlign: 'center' }}>
                    {this.state.region.latitude.toPrecision(7)},
                    {this.state.region.longitude.toPrecision(7)}
                  </Text>
                </View>
                <View style={styles.buttonContainer}>
                    <TouchableOpacity
                      onPress={() => this.jumpRandom()}
                      style={[styles.bubble, styles.button]}
                    >
                    <Text style={styles.buttonText}>Jump</Text>
                  </TouchableOpacity>
                 <TouchableOpacity
                   onPress={() => this.animateRandom()}
                   style={[styles.bubble, styles.button]}
                 >
                   <Text style={styles.buttonText}>Region</Text>
                 </TouchableOpacity>
                 <TouchableOpacity
                   onPress={() => this.animateRandomCoordinate()}
                   style={[styles.bubble, styles.button]}
                 >
                   <Text style={styles.buttonText}>Coordinate</Text>
                 </TouchableOpacity>
                 <TouchableOpacity
                   onPress={() => this.animateToRandomBearing()}
                   style={[styles.bubble, styles.button]}
                 >
                   <Text style={styles.buttonText}>Bearing</Text>
                 </TouchableOpacity>
                 <TouchableOpacity
                   onPress={() => this.animateToRandomViewingAngle()}
                   style={[styles.bubble, styles.button]}
                 >
                   <Text style={styles.buttonText}>View Angle</Text>
                 </TouchableOpacity>
          </View>
            </View>
        );
    }
}
const width = Dimensions.get('window').width;
const height = Dimensions.get('window').height;

const styles = {
    container: {
      ...StyleSheet.absoluteFillObject,
  justifyContent: 'flex-end',
  alignItems: 'center',
    },
    text: {
        fontSize: 30,
        fontWeight: '700',
        color: '#59656C',
        marginBottom: 10,
    },
    map: {
      ...StyleSheet.absoluteFillObject,
    },
    bubble: {
     backgroundColor: 'rgba(255,255,255,0.7)',
     paddingHorizontal: 18,
     paddingVertical: 12,
     borderRadius: 20,
   },
   latlng: {
     width: 200,
     alignItems: 'stretch',
   },
   button: {
    width: 60,
    paddingHorizontal: 8,
    alignItems: 'center',
    justifyContent: 'center',
    marginHorizontal: 5,
  },
  buttonContainer: {
    flexDirection: 'row',
    marginVertical: 20,
    backgroundColor: 'transparent',
  },
  buttonText: {
    textAlign: 'center',
  },
};
