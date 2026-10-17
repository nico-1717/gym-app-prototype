<?php

require 'ConnectionSettings.php';

// Check connection
if ($conn->connect_error) {
  die("Connection failed: " . $conn->connect_error);
}

$IDpac = $_POST["IDpacchetto"];

//$sql = "SELECT * FROM pacchetto WHERE IDpacchetto = '". $IDpac . "'";
$sql = "SELECT * FROM 
            (pacchetto AS p INNER JOIN utente AS u
            ON p.IDutente = u.IDutente) 
            WHERE p.IDpacchetto = ?";

$statement = $conn->prepare($sql);

$statement->bind_param("i", $IDpac);

$statement->execute();

$result = $statement->get_result();

if ($result->num_rows > 0) {
  // output data of each row
  $rows = array();
  while($row = $result->fetch_assoc()) {
    $rows[] = $row;
  }
  echo json_encode($rows);
} else {
  echo "0 results";
}
$statement->close();
$conn->close();

?>
