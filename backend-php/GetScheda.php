<?php

require 'ConnectionSettings.php';

// Check connection
if ($conn->connect_error) {
  die("Connection failed: " . $conn->connect_error);
}

$ID = $_POST["IDscheda"];

$sql = "SELECT * FROM 
            (scheda AS s INNER JOIN pacchetto AS p
            ON s.IDpacchetto = p.IDpacchetto) 
            WHERE s.IDscheda = ?";

$statement = $conn->prepare($sql);

$statement->bind_param("s", $ID);

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
$conn->close();

?>