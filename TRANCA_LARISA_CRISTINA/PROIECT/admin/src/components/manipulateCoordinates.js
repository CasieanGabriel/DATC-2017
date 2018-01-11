import api from './api';
var manipulateCoordinates = {
  manipulate(geoJson){
    var weather = [];
    var i=0;
   //  const selectedFrequency = JSON.parse(geoJson.features[0].geometry.coordinates);
   //  console.log(geoJson.features[0].geometry);
   //   for (let j = 0; j < selectedFrequency.length; j++) {
   //     const coordinates = JSON.parse(selectedFrequency[i].value);
   //
   //     // for (let i = 0; i < coordinates.length; i++) {
   //     //    var temps= api.getResource(coordinates[i].value).then((res) => {
   //     //      var obj = {
   //     //        'latitude': res.location.lat,
   //     //        'longitude': res.location.lon,
   //     //        'temperature': res.hourly_forecast[0].temp.english,
   //     //        'humidity': res.hourly_forecast[0].humidity
   //     //      }
   //     //     weather[i] = obj;
   //     //     i++;
   //     //   })
   //   // }
   // }
  geoJson.features[0].geometry.coordinates.forEach(function(coordinates){
      var terms = coordinates.map(function(term){

        fetch('http://api.wunderground.com/api/7371ed5d87903525/geolookup/hourly/q/'+ term[1]+','+term[0]+'.json')
              .then((response) => response.json())
            .then((responseJson) => {
              console.log(responseJson);
            })
            .catch((error) => {
              console.error(error);
            });
         // var temps= api.getResource(term).then((res) => {
         //   return{
         //     'latitude': res.location.lat,
         //     'longitude': res.location.lon,
         //     'temperature': res.hourly_forecast[0].temp.english,
         //     'humidity': res.hourly_forecast[0].humidity
         //   }
         //   });
         //   console.log(temps);
          // weather[i] = JSON.parse(obj);
          // i++;
        });
      });
  // });
  console.log(JSON.stringify(weather));
  return weather;
}
}
module.exports = manipulateCoordinates;
