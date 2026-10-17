<?php

require 'ConnectionSettings.php';

// Check connection
if ($conn->connect_error) {
  die("Connection failed: " . $conn->connect_error);
}

$IDpa = $_POST["IDpacchetto"];

$sql = "SELECT * FROM 
            (dettagli_es AS de INNER JOIN scheda AS sc
            ON de.IDscheda = sc.IDscheda)
            WHERE sc.IDpacchetto = ?";

$statement = $conn->prepare($sql);

$statement->bind_param("s", $IDpa);

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