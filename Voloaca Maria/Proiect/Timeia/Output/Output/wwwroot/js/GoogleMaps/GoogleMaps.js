function initMap() {
    var pLot1 = 1;
    var pLot2 = 1;
    var pLot3 = 1;
    var map = new google.maps.Map(document.getElementById('map'), {
        zoom: 20,
        center: { lat: 45.750496, lng: 21.225179 },
        mapTypeId: 'terrain'
    });
    var parkingLot1 = [
        { lat: 45.750548, lng: 21.225330 },
        { lat: 45.750566, lng: 21.225352 },
        { lat: 45.750594, lng: 21.225300 },
        { lat: 45.750577, lng: 21.225277 },
        { lat: 45.750548, lng: 21.225330 }
    ];

    var parkingLot2 = [
        { lat: 45.750510, lng: 21.225330 },
        { lat: 45.750528, lng: 21.225357 },
        { lat: 45.750558, lng: 21.225306 },
        { lat: 45.750541, lng: 21.225279 },
        { lat: 45.750510, lng: 21.225330 }
    ];

    var parkingLot3 = [
        { lat: 45.750468, lng: 21.225336 },
        { lat: 45.750489, lng: 21.225362 },
        { lat: 45.750516, lng: 21.225316 },
        { lat: 45.750499, lng: 21.225288 },
        { lat: 45.750467, lng: 21.225336 }
    ];
    
    if (pLot1 == 1 && pLot2==0 && pLot3==0) {
       
        var flightPath = new google.maps.Polyline({
            path: parkingLot1,
            geodesic: true,
            strokeColor: '#009900',
            strokeOpacity: 2.0,
            strokeWeight: 3
        });

        var flightPath2 = new google.maps.Polyline({
            path: parkingLot2,
            geodesic: true,
            strokeColor: '#FF0000',
            strokeOpacity: 2.0,
            strokeWeight: 3
        });

        var flightPath3 = new google.maps.Polyline({
            path: parkingLot3,
            geodesic: true,
            strokeColor: '#FF0000',
            strokeOpacity: 2.0,
            strokeWeight: 3
        });

      
    }
    if (pLot1 === 0 && pLot2 === 1 && pLot3 === 0) {

        var flightPath = new google.maps.Polyline({
            path: parkingLot1,
            geodesic: true,
            strokeColor: '#FF0000',
            strokeOpacity: 2.0,
            strokeWeight: 3
        });

        var flightPath2 = new google.maps.Polyline({
            path: parkingLot2,
            geodesic: true,
            strokeColor: '#009900',
            strokeOpacity: 2.0,
            strokeWeight: 3
        });

        var flightPath3 = new google.maps.Polyline({
            path: parkingLot3,
            geodesic: true,
            strokeColor: '#FF0000',
            strokeOpacity: 2.0,
            strokeWeight: 3
        });


    }
    if (pLot1 == 0 && pLot2 == 0 && pLot3 == 1) {

        var flightPath = new google.maps.Polyline({
            path: parkingLot1,
            geodesic: true,
            strokeColor: '#FF0000',
            strokeOpacity: 2.0,
            strokeWeight: 3
        });

        var flightPath2 = new google.maps.Polyline({
            path: parkingLot2,
            geodesic: true,
            strokeColor: '#FF0000',
            strokeOpacity: 2.0,
            strokeWeight: 3
        });

        var flightPath3 = new google.maps.Polyline({
            path: parkingLot3,
            geodesic: true,
            strokeColor: '#009900',
            strokeOpacity: 2.0,
            strokeWeight: 3
        });


    }

    if (pLot1 == 1 && pLot2 == 1 && pLot3 == 0)
    {
        var flightPath = new google.maps.Polyline({
            path: parkingLot1,
            geodesic: true,
            strokeColor: '#009900',
            strokeOpacity: 2.0,
            strokeWeight: 3
        });
        var flightPath2 = new google.maps.Polyline({
            path: parkingLot2,
            geodesic: true,
            strokeColor: '#009900',
            strokeOpacity: 2.0,
            strokeWeight: 3
        });

        var flightPath3 = new google.maps.Polyline({
            path: parkingLot3,
            geodesic: true,
            strokeColor: '#FF0000',
            strokeOpacity: 2.0,
            strokeWeight: 3
        });
    }
    if (pLot1 == 1 && pLot2 == 0 && pLot3 == 1)
    {
        var flightPath = new google.maps.Polyline({
            path: parkingLot1,
            geodesic: true,
            strokeColor: '#009900',
            strokeOpacity: 2.0,
            strokeWeight: 3
        });
        var flightPath2 = new google.maps.Polyline({
            path: parkingLot2,
            geodesic: true,
            strokeColor: '#FF0000',
            strokeOpacity: 2.0,
            strokeWeight: 3
        });

        var flightPath3 = new google.maps.Polyline({
            path: parkingLot3,
            geodesic: true,
            strokeColor: '#009900',
            strokeOpacity: 2.0,
            strokeWeight: 3
        });
    }
    if (pLot1 == 0 && pLot2 == 2 && pLot3 == 3) {
        var flightPath = new google.maps.Polyline({
            path: parkingLot1,
            geodesic: true,
            strokeColor: '#FF0000',
            strokeOpacity: 2.0,
            strokeWeight: 3
        });
        var flightPath2 = new google.maps.Polyline({
            path: parkingLot2,
            geodesic: true,
            strokeColor: '#009900',
            strokeOpacity: 2.0,
            strokeWeight: 3
        });

        var flightPath3 = new google.maps.Polyline({
            path: parkingLot3,
            geodesic: true,
            strokeColor: '#009900',
            strokeOpacity: 2.0,
            strokeWeight: 3
        });
    }
    if (pLot1 == 0 && pLot2 == 0 && pLot3 == 0) {
        var flightPath = new google.maps.Polyline({
            path: parkingLot1,
            geodesic: true,
            strokeColor: '#FF0000',
            strokeOpacity: 2.0,
            strokeWeight: 3
        });
        var flightPath2 = new google.maps.Polyline({
            path: parkingLot2,
            geodesic: true,
            strokeColor: '#FF0000',
            strokeOpacity: 2.0,
            strokeWeight: 3
        });

        var flightPath3 = new google.maps.Polyline({
            path: parkingLot3,
            geodesic: true,
            strokeColor: '#FF0000',
            strokeOpacity: 2.0,
            strokeWeight: 3
        });
    }
    if (pLot1 == 1 && pLot2 == 1 && pLot3 == 1) {
        var flightPath = new google.maps.Polyline({
            path: parkingLot1,
            geodesic: true,
            strokeColor: '#009900',
            strokeOpacity: 2.0,
            strokeWeight: 3
        });
        var flightPath2 = new google.maps.Polyline({
            path: parkingLot2,
            geodesic: true,
            strokeColor: '#009900',
            strokeOpacity: 2.0,
            strokeWeight: 3
        });

        var flightPath3 = new google.maps.Polyline({
            path: parkingLot3,
            geodesic: true,
            strokeColor: '#009900',
            strokeOpacity: 2.0,
            strokeWeight: 3
        });
    }
    flightPath.setMap(map);
    flightPath2.setMap(map);
    flightPath3.setMap(map);
}

