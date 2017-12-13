import React from 'react';
import { Dimensions, AppRegistry, StyleSheet, View, Image, Platform, ListView, TouchableOpacity, ToolbarAndroid } from 'react-native';
import { Container, Header, Content, Form, Item, Input, Label, Button, Text, Title, Left, Right, Body, Thumbnail, Fab, Toast, Drawer } from 'native-base';
import MapView from 'react-native-maps';
import ActionBar from 'react-native-action-bar';
import DrawerLayout from 'react-native-drawer-layout';
// import MapView from 'react-native-map-clustering';
// import { Marker } from 'react-native-maps';
import api from './api';
import coordinateImg from '../images/coordinate.png';
import regionImg from '../images/region.png';
import angleImg  from '../images/angle.png';
import jumpImg from '../images/jump.png';
import bearingImg from '../images/bearing.png';
import mapStyle from '../assets/mapStyle';
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
        latitude: 45.7473215,
        longitude: 21.226200199999994,
        latitudeDelta: 0.0021,
        longitudeDelta: 0.0021
      },
      marker: {
        coordinate: {
          latitude: 45.7473215,
          longitude: 21.226200199999994,
          latitudeDelta: 0.0021,
          longitudeDelta: 0.0021
        }
      },
      drawerClosed: true,
    }
    this.onRegionChange = this.onRegionChange.bind(this);
    this.jumpRandom = this.jumpRandom.bind(this);
    this.onMapPress = this.onMapPress.bind(this);
    this.toggleDrawer = this.toggleDrawer.bind(this);
   this.setDrawerState = this.setDrawerState.bind(this);
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
        color: randomColor(),
        title:e.nativeEvent.title,
        description:e.nativeEvent.description
      }
  });
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
      this.setState({ region: region,
        marker:
          {
            coordinate: region
          }
       });
    }
    jumpRandom() {
      const region = this.randomRegion();
      this.setState({ region: region,
        marker:
          {
            coordinate: region,
          }
       });
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

    render() {
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
                customMapStyle={mapStyle} ref={ref => { this.map = ref; }} onPress={(e) => this.onMapPress(e)}
                >
                  <MapView.Marker
                    coordinate={this.state.marker.coordinate}
                    pinColor={this.state.marker.color}
                    title={this.state.marker.title}
                    description={this.state.marker.description}
                  >
                  </MapView.Marker>
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
