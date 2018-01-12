<link rel="stylesheet" href="background.css" type="text/css">
<?php 

session_start(); 
?>

 <div class="body content">
		<div class="alert alert-success"><?= $_SESSION['message'] ?></div>
			Welcome <span class="user"><?= $_SESSION['username'] ?></span>
			<?php
				
			?>
	</div>
</div>


<?php 

$page = $_SERVER['PHP_SELF'];
 $sec = "15";
 header("Refresh: $sec; url=$page");
 
 
 
$connectionInfo = array("UID" => "intellipark@intelli", "pwd" => "#3intellius", "Database" => "intellipark", "LoginTimeout" => 30, "Encrypt" => 1, "TrustServerCertificate" => 0);
$serverName = "tcp:intelli.database.windows.net,1433";

//Establishes the connection
$conn = sqlsrv_connect($serverName, $connectionInfo);


if($conn === false)
{
    die(print_r(sqlsrv_errors(), true));
}


if ($_SERVER["REQUEST_METHOD"] == "POST")
{
	
    $idul = $_POST['id'];
	$username1 = $_SESSION['username'];
    $username2 = $_POST['username'];

	if ($username1==$username2)
	{
		
		$row_count1 = 0;
		$sql = "SELECT * FROM Projections where id='$idul' and status='F'";
		$params = array();
		$options =  array( "Scrollable" => SQLSRV_CURSOR_KEYSET );
		$stmt = sqlsrv_query( $conn, $sql , $params, $options );

		$row_count1 = sqlsrv_num_rows( $stmt );

		if ($row_count1==1)
		{ 
			$sql="insert into rezervari (nr_loc, rezervation_name, transmis) values('$idul', '$username1', '0')"; 
			$sql = sqlsrv_query($conn,$sql);
			$_SESSION['message'] = "Successful! ";
			header( "location: finish.php" );
		}
		else
		{
			alert("Cannot select this parking lot! Please choose another one.");
		}
	}
    else
	{
		alert("Incorrect username! ");
	}
	
	
}

function alert($msg) 
{
    echo "<script type='text/javascript'>alert('$msg');</script>";
}
 
?>

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
			<h2>Choose your parking lot!</h2>
			<form class="form" action="welcome.php" method="post" enctype="multipart/form-data" autocomplete="off">
			  <div class="alert alert-error"></div>
			  <input type="text" placeholder="User name" name="username" required />
			  <input type="text" placeholder="Parking Lot" name="id" required />
			  <input type="submit" value="Save" name="save" class="btn btn-block btn-primary" />
			  <input type="submit" value="Reset" name="reset" class="btn btn-block btn-primary" />	
	    	</form>
			<form class="form" action="datcAzure.html" method="post" enctype="multipart/form-data" autocomplete="off">
			  <div class="alert alert-error"></div>	
			  <input type="submit" value="Log Out" name="logout" class="btn btn-block btn-primary" />
			  <input type="button" value="Close this window" onclick="self.close()" class="btn btn-block btn-primary" />	
			</form>
		  </div>
		</div>

		
  </body>
</html>