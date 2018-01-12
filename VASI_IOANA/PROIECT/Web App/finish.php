<link rel="stylesheet" href="background.css" type="text/css">
<?php 

session_start(); 

$page = $_SERVER['PHP_SELF'];
 $sec = "15";
 header("Refresh: $sec; url=$page");
 
 
?>

 <div class="body content">
		<div class="alert alert-success"><?= $_SESSION['message'] ?></div>
			User: <span class="user"><?= $_SESSION['username'] ?></span>
			<?php
				
			?>
	</div>
</div>


<!DOCTYPE html >
  <head>
    <meta name="viewport" content="initial-scale=1.0, user-scalable=no" />
    <meta http-equiv="content-type" content="text/html; charset=UTF-8"/>
    <title>The Parking Lot</title>
    <style>
      
      #map {
        height: 550px;
        width: 100%;
      }
      /* Optional: Makes the sample page fill the window. */
      html, body {
        height: 80%;
        margin: 0;
        padding: 0;
      }
    </style>
  </head>

  <body>
    <div id="map"></div>

    <script>
		var customLabel = {
				reserved: {
				  label: 'R'
				},
				occupied: {
				  label: 'O'
				},
				free: {
				  label: 'F'
				},
				malfunction: {
				  label: 'M'
				}
			  };      

        function initMap() {
          var uluru = {lat: 45.731198678990275, lng:  21.239884793758392};
          var map = new google.maps.Map(document.getElementById('map'), {
            zoom: 24,
            center: uluru,
            mapTypeId: 'satellite'
          });
          map.setTilt(45);

          var infoWindow = new google.maps.InfoWindow;

		  
		  
		  downloadUrl('http://localhost/www_pro/doc.php', function(data) {
            var xml = data.responseXML;
            var markers = xml.documentElement.getElementsByTagName('marker');
            Array.prototype.forEach.call(markers, function(markerElem) {
              var id = markerElem.getAttribute('id');
              var status = markerElem.getAttribute('status');
              var point = new google.maps.LatLng(
                  parseFloat(markerElem.getAttribute('lat')),
                  parseFloat(markerElem.getAttribute('lng')));

              var infowincontent = document.createElement('div');
              var strong = document.createElement('strong');
              strong.textContent = id
              infowincontent.appendChild(strong);
              infowincontent.appendChild(document.createElement('br'));

              var text = document.createElement('text');
              text.textContent = status
              infowincontent.appendChild(text);
              var icon = customLabel[status] || {};
		  
			var marker = new google.maps.Marker({
                map: map,
                position: point,
                label: status
              });
		  
          marker.addListener('click', function() {
            infoWindow.setContent(infowincontent);
            infoWindow.open(map, marker);

              });
            });
          });
       }
	   
		function downloadUrl(url, callback) {
			var request = window.ActiveXObject ?
				new ActiveXObject('Microsoft.XMLHTTP') :
				new XMLHttpRequest;

			request.onreadystatechange = function() {
			  if (request.readyState == 4) {
				request.onreadystatechange = doNothing;
				callback(request, request.status);
			  }
			};

			request.open('GET', url, true);
			request.send(null);
      }

      function doNothing() {}
    </script>
    <script async defer
    src="https://maps.googleapis.com/maps/api/js?key=AIzaSyA5OyUVGhINTd1bJ2DN4dBTQGKSMGiuB3A&callback=initMap">
    </script>
	
	<div class="body-content">
		  <div class="module">
			<form class="form" action="datcAzure.html" method="post" enctype="multipart/form-data" autocomplete="off">
			  <div class="alert alert-error"></div>
			  <input type="submit" value="Log Out" name="logout" class="btn btn-block btn-primary" />
			  <input type="button" value="Close this window" onclick="self.close()" class="btn btn-block btn-primary" />	
			</form>
		  </div>
		</div>
	
  </body>
</html>