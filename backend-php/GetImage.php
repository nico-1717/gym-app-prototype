<?php

require 'ConnectionSettings.php';

// Check connection
if ($conn->connect_error) {
  die("Connection failed: " . $conn->connect_error);
}

$IDes = $_POST["IDesercizio"];

// Prepared statement: the ID is never concatenated into the SQL string
$sql = "SELECT Immagine FROM esercizio WHERE IDesercizio = ?";

$statement = $conn->prepare($sql);
$statement->bind_param("s", $IDes);
$statement->execute();
$result = $statement->get_result();

if ($result->num_rows > 0) {
  $row = $result->fetch_assoc();

  // Read the file from the local Images folder; basename() blocks paths like "../"
  $path = __DIR__ . '/Images/' . basename($row["Immagine"]);

  if (is_file($path)) {
    echo file_get_contents($path);
  } else {
    echo "Not found";
  }
}
else {
  echo "Not found";
}

$conn->close();

?>
