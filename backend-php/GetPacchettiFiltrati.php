<?php

require 'ConnectionSettings.php';

// Check connection
if ($conn->connect_error) {
  die("Connection failed: " . $conn->connect_error);
}

$Limit = $_POST["Limit"];
$Start = $_POST["OffSet"];
$Descrizione = $_POST["Descrizione"];

//$sql = "SELECT * FROM pacchetto ORDER BY IDpacchetto ASC LIMIT ". $Limit ." OFFSET ". $Start;
$sql = "SELECT * FROM pacchetto WHERE LOWER(NomePacchetto) LIKE CONCAT('%', LOWER(?), '%') ORDER BY IDpacchetto ASC LIMIT ? OFFSET ?";
$sql = "SELECT * FROM 
            (pacchetto AS p INNER JOIN utente AS u
            ON p.IDutente = u.IDutente) 
            WHERE LOWER(p.NomePacchetto) LIKE CONCAT('%', LOWER(?), '%') ORDER BY p.IDpacchetto ASC LIMIT ? OFFSET ?";

$statement = $conn->prepare($sql);

$statement->bind_param("sss", $Descrizione, $Limit, $Start);

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
