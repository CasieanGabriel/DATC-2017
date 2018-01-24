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

$ok=0;

if ($_SERVER["REQUEST_METHOD"] == "POST") {
    if ($_POST['pass'] == $_POST['confirmpassword']) 
	{
        //define other variables with submitted values from $_POST
        $firstname = $_POST['firstname'];
        $surname = $_POST['surname'];
        $email = $_POST['email'];
        $username = $_POST['username'];
		$pass = md5($_POST['pass']);
		$_SESSION['username'] = $username;


		
		$row_count = 0;
		$sql = "SELECT * FROM utilizator where email='$email'";
		$params = array();
		$options =  array( "Scrollable" => SQLSRV_CURSOR_KEYSET );
		$stmt = sqlsrv_query( $conn, $sql , $params, $options );

		$row_count = sqlsrv_num_rows( $stmt );
	
		if ($row_count!=0)
		{
			$ok=1;
			alert("An account already exists with this email address. Please choose another email address.");
		}
		
		
		
		$row_count = 0;
		$sql = "SELECT * FROM utilizator where username='$username'";
		$params = array();
		$options =  array( "Scrollable" => SQLSRV_CURSOR_KEYSET );
		$stmt = sqlsrv_query( $conn, $sql , $params, $options );

		$row_count = sqlsrv_num_rows( $stmt );
	
		if ($row_count!=0)
		{
			$ok=1;
			alert("An account already exists with this username address. Please choose another username.");
		}
		
		if($ok==0)
		{
			// adaugare cu parametri
				$sql="insert into utilizator (firstname, surname, email, username, pass) values('$firstname', '$surname', '$email', '$username', '$pass')"; 
				$sql = sqlsrv_query($conn,$sql);
				$_SESSION['message'] = "Registration successful! "."Welcome to IntelliPark!";
				header( "location: welcome.php" );
		}
		
		//create SQL query string for inserting data into the database
		 
	}
	else 
	{
		alert("Two passwords do not match!");
    }
}


function alert($msg) {
    echo "<script type='text/javascript'>alert('$msg');</script>";
}

?>
<html>
<link rel="stylesheet" href="background.css" type="text/css">
	<div class="body-content">
	  <div class="module">
		<h1>Create an account</h1>
		<form class="form" action="registerAzure.php" method="post" enctype="multipart/form-data" autocomplete="off">
		  <div class="alert alert-error"></div>
		  <input type="text" placeholder="First Name" name="firstname" required />
		  <input type="text" placeholder="Surname" name="surname" required />
		  <input type="email" placeholder="Email" name="email" required />
		  <input type="text" placeholder="User Name" name="username" required />
		  <input type="password" placeholder="Password" name="pass" autocomplete="new-password" required />
		  <input type="password" placeholder="Confirm Password" name="confirmpassword" autocomplete="new-password" required />
		  <input type="submit" value="Register" name="register" class="btn btn-block btn-primary" />	
		  <input type="submit" value="Reset" name="reset" class="btn btn-block btn-primary" />	
		</form>
	  </div>
	</div>
</html>
