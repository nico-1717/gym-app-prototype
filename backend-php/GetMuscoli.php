<?php

require 'ConnectionSettings.php';

// Check connection
if ($conn->connect_error) {
  die("Connection failed: " . $conn->connect_error);
}

$IDes = $_POST["IDesercizio"];

$sql = "SELECT * FROM 
            (esercizio AS e INNER JOIN m_e AS m
            ON e.IDesercizio = m.IDesercizio) 
            WHERE m.IDesercizio = ?";
            
$statement = $conn->prepare($sql);

$statement->bind_param("s", $IDes);

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