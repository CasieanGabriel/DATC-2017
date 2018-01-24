<?php

$connectionInfo = array("UID" => "intellipark@intelli", "pwd" => "#3intellius", "Database" => "intellipark", "LoginTimeout" => 30, "Encrypt" => 1, "TrustServerCertificate" => 0);
$serverName = "tcp:intelli.database.windows.net,1433";

//Establishes the connection
$conn = sqlsrv_connect($serverName, $connectionInfo);


if($conn === false)
{
    die(print_r(sqlsrv_errors(), true));
}

session_start(); 

 
if ($_SERVER["REQUEST_METHOD"] == "POST") {
	
    $username = $_POST['username'];
	$pass = md5($_POST['pass']);
	$_SESSION['username'] = $username;
	
	$row_count = 0;
	$sql = "SELECT * FROM utilizator where username='$username' AND pass='$pass'";
	$params = array();
	$options =  array( "Scrollable" => SQLSRV_CURSOR_KEYSET );
	$stmt = sqlsrv_query( $conn, $sql , $params, $options );

	$row_count = sqlsrv_num_rows( $stmt );
	 
	 
	if ($row_count==0)
	{
		$_SESSION['message'] = 'Invalid username or password!';
		alert("Invalid username or password!");
	}
    else
	{
		$_SESSION['message'] = 'Welcome to IntelliPark!'. $row;
		header( "location: welcome.php" );
	}
	
}

function alert($msg) 
{
    echo "<script type='text/javascript'>alert('$msg');</script>";
}
?>
<link rel="stylesheet" href="background.css" type="text/css">
<html>
	<head>
		<title>Register</title>
	</head>
	<body>
		<div class="body-content">
		  <div class="module">
			<h1>Login to your account</h1>
			<form class="form" action="loginAzure.php" method="post" enctype="multipart/form-data" autocomplete="off">
			  <div class="alert alert-error"></div>
			  <input type="text" placeholder="User Name" name="username" required />
			  <input type="password" placeholder="Password" name="pass" autocomplete="password" required />
			  <input type="submit" value="Login" name="login" class="btn btn-block btn-primary" />
			  <input type="submit" value="Reset" name="reset" class="btn btn-block btn-primary" />	
			</form>
		  </div>
		</div>
	</body>
</html>