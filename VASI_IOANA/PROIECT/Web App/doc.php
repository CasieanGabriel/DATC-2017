<?php
		$dom = new DOMDocument("1.0");
		$node = $dom->createElement("Markers");
		$parnode = $dom->appendChild($node);
		
		$connectionInfo = array("UID" => "intellipark@intelli", "pwd" => "#3intellius", "Database" => "intellipark", "LoginTimeout" => 30, "Encrypt" => 1, "TrustServerCertificate" => 0);
		$serverName = "tcp:intelli.database.windows.net,1433";


		//Establishes the connection
		$conn = sqlsrv_connect($serverName, $connectionInfo);

		if($conn === false)
		{
			die(print_r(sqlsrv_errors(), true));
		}

		$tsql = "select * from Projections";
		
		$getResults = sqlsrv_query( $conn, $tsql);
		
		if ($getResults == FALSE)
		{
			echo (sqlsrv_errors());
		}
		
	/*	while ($row = sqlsrv_fetch_array($getResults, SQLSRV_FETCH_ASSOC))
		{
			echo ($row['id']." ".$row['status'].PHP_EOL);
		}
	*/	
		header("Content-type: text/xml");
		
		// Start XML file, echo parent node

		while ($row = sqlsrv_fetch_array($getResults, SQLSRV_FETCH_ASSOC))
		{
			 // Add to XML document node
			  $node = $dom->createElement("marker");
			  $newnode = $parnode->appendChild($node);
			  $newnode->setAttribute("id",$row['id']);
			  $newnode->setAttribute("status",$row['status']);
			  $newnode->setAttribute("lat",$row['lat']);
			  $newnode->setAttribute("lng",$row['lng']);
		}
		
		echo $dom->saveXML();
			 // */
?>
