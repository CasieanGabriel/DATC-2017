import React from 'react';
import { Dimensions, AppRegistry, StyleSheet, View, Image, Platform, ListView, TouchableOpacity, ToolbarAndroid } from 'react-native';
import { Container, Header, Content, Form, Item, Input, Label, Button, Text, Title, Left, Right, Body, Thumbnail, Fab, Toast, Drawer } from 'native-base';
import MapView from 'react-native-maps';
import Geojson from 'react-native-geojson';
import ActionBar from 'react-native-action-bar';
import DrawerLayout from 'react-native-drawer-layout';

import api from './api';
import coordinateImg from '../images/coordinate.png';
import regionImg from '../images/region.png';
import angleImg  from '../images/angle.png';
import jumpImg from '../images/jump.png';
import bearingImg from '../images/bearing.png';
import mapStyle from '../assets/mapStyle';
import geoJson from '../assets/geoJson';
import redIcon from '../images/red.png';
import blueIcon from '../images/blue.png';
import Menu from './Menu';
import SideBar from './SideBar';

function randomColor() {
  return `#${Math.floor(Math.random() * 16777215).toString(16)}`;
}
let id = 0;
const DEFAULT_PADDING = { top: 40, right: 40, bottom: 40, left: 40 };
const width = Dimensions.get('window').width;
const height = Dimensions.get('window').height;
export class mapScreen extends React.Component {

  constructor(props){
    super(props);
    this.state = {
      resource: [],
      region: {
        latitude: 45.74449980000001,
        longitude: 21.22336506843567,
        latitudeDelta: 0.0021,
        longitudeDelta: 0.0021
      },
      marker: {
        coordinate: {
          latitude: 45.74449980000001,
          longitude: 21.22336506843567,
          latitudeDelta: 0.0021,
          longitudeDelta: 0.0021
        }
      },
      drawerClosed: true,
      polygons: [],
editing: null,
creatingHole: false,
    }
    this.onRegionChange = this.onRegionChange.bind(this);
    this.jumpRandom = this.jumpRandom.bind(this);
    this.onMapPress = this.onMapPress.bind(this);
    this.toggleDrawer = this.toggleDrawer.bind(this);
    this.setDrawerState = this.setDrawerState.bind(this);
    this.getTemperature = this.getTemperature.bind(this);
    this.getHumidity = this.getHumidity.bind(this);
  }

  componentWillMount(){
    var weather = [];
    var i=0;
    var coord = geoJson.features[0].geometry.coordinates.forEach(function(coordinates){
        var terms = coordinates.forEach(function(term){
          var tes= api.getResource(term);
          console.log(tes);
           var temps= api.getResource(term).then((res) => {
             var obj = {
               'latitude': res.location.lat,
               'longitude': res.location.lon,
               'temperature': res.hourly_forecast[0].temp.english,
               'humidity': res.hourly_forecast[0].humidity
             }
            weather[i] = obj;
            i++;
          })
        });
    });
    console.log(weather);

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
          color: randomColor(),
          title:e.nativeEvent.title,
          description:e.nativeEvent.description
        }
    });
    const { editing, creatingHole } = this.state;
    if (!editing) {
      this.setState({
        editing: {
          id: id++,
          coordinates: [e.nativeEvent.coordinate],
          holes: [],
        },
      });
    } else if (!creatingHole) {
      this.setState({
        editing: {
          ...editing,
          coordinates: [
            ...editing.coordinates,
            e.nativeEvent.coordinate,
          ],
        },
      });
    } else {
      const holes = [...editing.holes];
      holes[holes.length - 1] = [
        ...holes[holes.length - 1],
        e.nativeEvent.coordinate,
      ];
      this.setState({
        editing: {
          ...editing,
          id: id++, // keep incrementing id to trigger display refresh
          coordinates: [
            ...editing.coordinates,
          ],
          holes,
        },
      });
}
  }
  setDrawerState() {
    this.setState({
      drawerClosed: !this.state.drawerClosed,
    });
  }

  toggleDrawer = () => {
    if (this.state.drawerClosed) {
      this.DRAWER.openDrawer();
    } else {
      this.DRAWER.closeDrawer();
    }
  }
    onRegionChange(region) {
      this.setState({ region: region
       });
    }
    jumpRandom() {
      const region = this.randomRegion();
      this.setState({ region: region
       });
    }
  getTemperature(){

  }
  getHumidity(){

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
  finish() {
  const { polygons, editing } = this.state;
  this.setState({
    polygons: [...polygons, editing],
    editing: null,
    creatingHole: false,
  });
}

createHole() {
  const { editing, creatingHole } = this.state;
  if (!creatingHole) {
    this.setState({
      creatingHole: true,
      editing: {
        ...editing,
        holes: [
          ...editing.holes,
          [],
        ],
      },
    });
  } else {
    const holes = [...editing.holes];
    if (holes[holes.length - 1].length === 0) {
      holes.pop();
      this.setState({
        editing: {
          ...editing,
          holes,
        },
      });
    }
    this.setState({ creatingHole: false });
  }
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

    render() {
      const mapOptions = {
  scrollEnabled: true,
};
      if (this.state.editing) {
  mapOptions.scrollEnabled = false;
  mapOptions.onPanDrag = e => this.onPress(e);
}
        return (

            <DrawerLayout
               drawerWidth={300}
               ref={drawerElement => {
                 this.DRAWER = drawerElement;
               }}
               drawerPosition={DrawerLayout.positions.left}
               onDrawerOpen={this.setDrawerState}
               onDrawerClose={this.setDrawerState}
               renderNavigationView={() => <Menu />}
             >
               <ActionBar
                 containerStyle={styles.bar}
                 backgroundColor="#F035E0"
                 leftIconName={'menu'}
                 onLeftPress={this.toggleDrawer}/>

     <View style={styles.container}>
                <MapView style={styles.map} initialRegion = {this.coordinate} region={this.state.region} onRegionChange={this.onRegionChange}
                customMapStyle={mapStyle}
                 ref={ref => { this.map = ref; }} onPress={(e) => this.onMapPress(e)} {...mapOptions}
                >

                {this.state.polygons.map(polygon => (
                    <MapView.Polygon
                      key={polygon.id}
                      coordinates={polygon.coordinates}
                      holes={polygon.holes}
                      strokeColor="#F00"
                      fillColor="rgba(255,0,0,0.5)"
                      strokeWidth={1}
                    />
                  ))}
                  {this.state.editing && (
                    <MapView.Polygon
                      key={this.state.editing.id}
                      coordinates={this.state.editing.coordinates}
                      holes={this.state.editing.holes}
                      strokeColor="#000"
                      fillColor="rgba(255,0,0,0.5)"
                      strokeWidth={1}
                    />
                  )}
                  <Geojson geojson={geoJson} />
                  <MapView.Marker
                    coordinate={this.state.marker.coordinate}
                    pinColor={this.state.marker.color}
                    title={this.state.marker.title}
                    description={this.state.marker.description}
                  >
                  </MapView.Marker>
                </MapView>
                <View style={styles.inputWrapper}>
                  <TouchableOpacity
                      onPress={() => this.getTemperature()}>
            				<Image source={redIcon}
            					style={styles.inlineImg} />
            				<Text style={styles.input}>Temperature</Text>
                  </TouchableOpacity>
                  <TouchableOpacity
                    onPress={() => this.getHumidity()}>
            				<Image source={blueIcon}
            					style={styles.inlineImg} />
            				<Text style={styles.input}>Humidity</Text>
                  </TouchableOpacity>
          			</View>

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
                    <Image source={jumpImg} style={styles.image} />
                  </TouchableOpacity>
                 <TouchableOpacity
                   onPress={() => this.animateRandom()}
                   style={[styles.bubble, styles.button]}
                 >
                   <Image source={regionImg} style={styles.image} />
                 </TouchableOpacity>
                 <TouchableOpacity
                   onPress={() => this.animateRandomCoordinate()}
                   style={[styles.bubble, styles.button]}
                 >
                   <Image source={coordinateImg} style={styles.image} />
                 </TouchableOpacity>
                 <TouchableOpacity
                   onPress={() => this.animateToRandomBearing()}
                   style={[styles.bubble, styles.button]}
                 >
                   <Image source={bearingImg} style={styles.image} />
                 </TouchableOpacity>
                 <TouchableOpacity
                   onPress={() => this.animateToRandomViewingAngle()}
                   style={[styles.bubble, styles.button]}
                 >
                   <Image source={angleImg} style={styles.image} />
                 </TouchableOpacity>

                    </View>
            </View>
               </DrawerLayout>
        );
    }
}

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
      // ...StyleSheet.absoluteFillObject,
      position: 'absolute',
      top: 40,
      right: 0,
      bottom: 0,
      left: 0,
    },
    bubble: {
     backgroundColor: 'rgba(255,255,255,0.7)',
     paddingHorizontal: 18,
     paddingVertical: 12,
     borderRadius: 20,
   },
   input: {
 		backgroundColor: 'rgba(255, 255, 255, 0.4)',
 		width: 150,
 		height: 40,
 		marginHorizontal: 20,
 		paddingLeft: 45,
 		// borderRadius: 20,
 		color: '#ffffff',
 	},
 	inputWrapper: {
 		flex: 2,
    top: 50,
    alignSelf: 'flex-end',
 	},
 	inlineImg: {
 		position: 'absolute',
 		zIndex: 99,
 		width: 22,
 		height: 22,
 		left: 35,
 		top: 1,
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
  callout:{
    flex:1,
    paddingRight:10,
    paddingBottom:10,
    marginRight:10,
    marginBottom:10
  },
  calloutPhoto:{
    flex:1,
    width:166,
    height:83
  },
  calloutTitle:{
    fontSize:16,
  },
  image: {
    width: 24,
    height: 24,
  },
  screen: {
    backgroundColor: '#33cc33',
    flex: 1,
    paddingTop: 10,
    alignItems: 'center',
    padding: 10
  },
};
