using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;

public class SQLite : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        //// Create table
        //IDbCommand dbcmd;
        //dbcmd = dbcon.CreateCommand();
        //string q_createTable = "CREATE TABLE IF NOT EXISTS my_table (id INTEGER PRIMARY KEY, val INTEGER )";

        //dbcmd.CommandText = q_createTable;
        //dbcmd.ExecuteReader();

        //// Insert values in table
        //IDbCommand cmnd = dbcon.CreateCommand();
        //cmnd.CommandText = "INSERT INTO my_table (id, val) VALUES (0, 5)";
        //cmnd.ExecuteNonQuery();

        //// Read and print all values in table
        //IDbCommand cmnd_read = dbcon.CreateCommand();
        //IDataReader reader;
        //string query = "SELECT * FROM my_table";
        //cmnd_read.CommandText = query;
        //reader = cmnd_read.ExecuteReader();

        //while (reader.Read())
        //{
        //    Debug.Log("id: " + reader[0].ToString());
        //    Debug.Log("val: " + reader[1].ToString());
        //}

        //// Close connection
        //dbcon.Close();


        // Create database
        string connection = "URI=file:" + Application.persistentDataPath + "/" + "My_Database";

        // Open connection
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();

        // Creazione Tabelle
        IDbCommand cmnd = dbcon.CreateCommand();
        //Allenamento
        cmnd.CommandText = "CREATE TABLE IF NOT EXISTS " + Globals.TABL_ALLENAMENTO + " (" + Globals.COL_ALL_ID + " INTEGER, " + Globals.COL_ALL_DATA + " VARCHAR(20), " + Globals.COL_ALL_DURATA + " VARCHAR(9), " + Globals.COL_ALL_FINITO + " BIT, " + Globals.COL_ALL_ID_SCHEDA + " INT, PRIMARY KEY(" + Globals.COL_ALL_ID + " AUTOINCREMENT))";
        cmnd.ExecuteNonQuery();
        //Attività
        cmnd.CommandText = "CREATE TABLE IF NOT EXISTS " + Globals.TABL_ATTIVITA + " ( " + Globals.COL_ATT_ID + " INTEGER, " + Globals.COL_ATT_SERIE + " INT, " + Globals.COL_ATT_RIPETIZIONIn + " INT, " + Globals.COL_ATT_RIPETIZIONIt + " CHAR(5), " + Globals.COL_ATT_PESO + " REAL, " + Globals.COL_ATT_RECUPERO + " CHAR(5), " + Globals.COL_ATT_ID_DETTAGLI + " INT, " + Globals.COL_ATT_ID_ALLENAMENTO + " INT, PRIMARY KEY(" + Globals.COL_ATT_ID + " AUTOINCREMENT) )";
        cmnd.ExecuteNonQuery();
        //Dettagli_Es
        cmnd.CommandText = "CREATE TABLE IF NOT EXISTS " + Globals.TABL_DETTAGLI + " ( " + Globals.COL_DET_ID + " INTEGER, " + Globals.COL_DET_SERIE + " INT, " + Globals.COL_DET_RIPnOt + " CHAR(1), " + Globals.COL_DET_RIPETIZIONIn + " INT, " + Globals.COL_DET_RIPETIZIONIt + " CHAR(5), " + Globals.COL_DET_RECUPERO + " CHAR(5), " + Globals.COL_DET_ATTESA + " CHAR(5), " + Globals.COL_DET_POSIZIONE + " INTEGER, " + Globals.COL_DET_ID_ESERCIZIO + " INTEGER, " + Globals.COL_DET_ID_SCHEDA + " INTEGER, PRIMARY KEY(" + Globals.COL_DET_ID + " AUTOINCREMENT) )";
        cmnd.ExecuteNonQuery();
        //Esercizio
        cmnd.CommandText = "CREATE TABLE IF NOT EXISTS " + Globals.TABL_ESERCIZIO + " ( " + Globals.COL_ESE_ID + " INTEGER, " + Globals.COL_ESE_NOME + " VARCHAR(20), " + Globals.COL_ESE_CONSIGLI + " VARCHAR(200), " + Globals.COL_ESE_NOTE + " VARCHAR(200), " + Globals.COL_ESE_IMMAGINE + " BLOB, PRIMARY KEY(" + Globals.COL_ESE_ID + " AUTOINCREMENT) )";
        cmnd.ExecuteNonQuery();
        //Muscolo
        cmnd.CommandText = "CREATE TABLE IF NOT EXISTS " + Globals.TABL_MUSCOLO + " ( " + Globals.COL_MUS_ID + " INTEGER, " + Globals.COL_MUS_NOME + " VARCHAR(20), PRIMARY KEY(" + Globals.COL_MUS_ID + " AUTOINCREMENT) )";
        cmnd.ExecuteNonQuery();
        //M_E
        cmnd.CommandText = "CREATE TABLE IF NOT EXISTS " + Globals.TABL_ME + " ( " + Globals.COL_MxE_ID + " INTEGER, " + Globals.COL_MxE_IDESERCIZIO + " INT, " + Globals.COL_MxE_IDMUSCULO + " INT, PRIMARY KEY(" + Globals.COL_MxE_ID + " AUTOINCREMENT) )";
        cmnd.ExecuteNonQuery();
        //Scheda
        cmnd.CommandText = "CREATE TABLE IF NOT EXISTS " + Globals.TABL_SCHEDA + " ( " + Globals.COL_SCH_ID + " INTEGER, " + Globals.COL_SCH_NOME + " VARCHAR(20), PRIMARY KEY(" + Globals.COL_SCH_ID + " AUTOINCREMENT) )";
        cmnd.ExecuteNonQuery();

        #region Muscoli
        string[] Muscoli = new string[6];
        Muscoli[0] = "Spalle";
        Muscoli[1] = "Petto";
        Muscoli[2] = "Schiena";
        Muscoli[3] = "Braccia";
        Muscoli[4] = "ABS";
        Muscoli[5] = "Gambe";

        DataTable dataTable = AppManager.Instance.sql.SelectAllMuscoli("", Globals.COL_MUS_ID);
        int iInseriti = 0;
        for (int row = 0; row < Muscoli.Length; row++)
        {
            if (row >= dataTable.Rows.Count)
            {
                AppManager.Instance.sql.InsertMuscolo(row, Muscoli[row]);
                iInseriti += 1;
            }
            else
            {
                if (Muscoli[row] != dataTable.Rows[row - iInseriti][Globals.COL_MUS_NOME].ToString())
                    AppManager.Instance.sql.UpdateMuscolo(row, Muscoli[row]);
            }
        }
        #endregion

        // Close connection
        dbcon.Close();

    }
}
