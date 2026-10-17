<?php
require 'ConnectionSettings.php';

// Only real images, with a safe file name, can be uploaded
$allowedExtensions = array("jpg", "jpeg", "png");
$maxSize = 5 * 1024 * 1024; // 5 MB

if (!isset($_FILES['myimage']) || $_FILES['myimage']['error'] !== UPLOAD_ERR_OK) {
    echo "[error] there is no data with name [myimage]";
    exit();
}

$tmpimg = $_FILES['myimage']['tmp_name'];

// basename() removes any folder part, so the file cannot be written outside Images/
$NomeImg = basename(isset($_POST["NomeImg"]) ? $_POST["NomeImg"] : "");
$extension = strtolower(pathinfo($NomeImg, PATHINFO_EXTENSION));

// Letters, numbers, "_" and "-" only, followed by an allowed image extension
if (!preg_match('/^[A-Za-z0-9_-]+\.[A-Za-z]+$/', $NomeImg) || !in_array($extension, $allowedExtensions)) {
    echo "[error] only .jpg, .jpeg and .png files are allowed";
    exit();
}

if ($_FILES['myimage']['size'] > $maxSize || getimagesize($tmpimg) === false) {
    echo "[error] the file is not a valid image";
    exit();
}

if (move_uploaded_file($tmpimg, __DIR__ . "/Images/" . $NomeImg)) {
    echo "[success] image ($NomeImg) uploaded successfully.";
} else {
    echo "[error] upload failed";
}

?>
