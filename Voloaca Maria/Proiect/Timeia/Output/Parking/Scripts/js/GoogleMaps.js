function initMap() {
    var map = new google.maps.Map(document.getElementById('map'), {
        zoom: 20,
        center: { lat: 45.750496, lng: 21.225179 },
        mapTypeId: 'terrain'
    });
    var parkingLot = [
        {
            lat: 45.749590, lng: 21.225675
        },
        {
            lat: 45.749615, lng: 21.225693
        },
        {
            lat: 45.749643, lng: 21.225611
        },
        {
            lat: 45.749626, lng: 21.225592
        },
        {
            lat: 45.749590, lng: 21.225675
        }
    ];

    var rectangle = new google.maps.Rectangle(parkingLot
        
    );

    var flightPath = new google.maps.Polyline(
        {
            path: parkingLot,
            geodesic: true,

            strokeColor: "#00FF40",
            strokeOpacity: 2.0,
            strokeWeight: 3
        }

    )
    flightPath.setMap(map);
}
   /*
    function Repet() {
        $.get("/Home/GetPArkingLotsInfo", function (data) {

            for (var i = 0; i < data.length; i++) {

                var parkingLot = [
                    { lat: data[i].TopLeftLat, lng: data[i].TopLeftLng },
                    { lat: data[i].TopRightLat, lng: data[i].TopRightLng },
                    { lat: data[i].BottomRightLat, lng: data[i].BottomRightLng },
                    { lat: data[i].BottomLeftLat, lng: data[i].BottomLeftLng },
                    { lat: data[i].TopLeftLat, lng: data[i].TopLeftLng }
                ];

                var color = "";
                if (data[i].Status === false) {
                    color = '#FF0000';
                }
                else {
                    color = '#00FF00';
                }

                var flightPath = new google.maps.Polyline(
                    {
                        path: parkingLot,
                        geodesic: true,

                        strokeColor: color,
                        strokeOpacity: 2.0,
                        strokeWeight: 3
                    }

                )
                flightPath.setMap(map);
            }
            setTimeout(Repet, 1000);
        });
        
    }
   // setInterval(Repet, 1000);
    Repet();
}

//var markers = [];

    //function clearOverlays() {
    //    while (markers.length) { markers.pop().setMap(null); }
    //    markers.length = 0;
    //}

    //markers.push(marker);
    //google.maps.event.addListener(marker, "click", function () { });

function deleteOverlays() {
    if (markersArray) {
        for (i in markersArray) {
            markersArray[i].setMap(null);
        }
        markersArray.length = 0;
    }
}
 
    */