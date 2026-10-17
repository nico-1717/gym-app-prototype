<?php

require 'ConnectionSettings.php';

// Check connection
if ($conn->connect_error) {
  die("Connection failed: " . $conn->connect_error);
}

$IDsc = $_POST["IDscheda"];

$sql = "SELECT * FROM 
            ((dettagli_es AS d INNER JOIN scheda AS s
            ON d.IDscheda = s.IDscheda) 
            INNER JOIN esercizio AS e
            ON d.IDesercizio = e.IDesercizio)
            WHERE d.IDscheda = ?
            ORDER BY d.Posizione";

$statement = $conn->prepare($sql);

$statement->bind_param("s", $IDsc);

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