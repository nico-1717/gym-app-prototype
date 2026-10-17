using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System;
using UnityEngine.UI;
using System.IO;
using System.Text;
using System.Threading.Tasks;

public class SQLiteEvents : MonoBehaviour
{
    private void Start()
    {
        Globals.SQL += 1;
        if (Globals.SQL > 1)
            Destroy(this);
    }
    #region CONNESSIONE
    private IDbConnection Connection()
    {
        string connection = "URI=file:" + Application.persistentDataPath + "/" + "My_Database";
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();
        return dbcon;
    }
    #endregion 

    #region INSERT
    #region Allenamento
    public int InsertAllenamento(string Data, string Durata, int Bit, int IDscheda)
    {
        int ID = 0;
        try
        {
            IDbConnection dbcon = Connection();
            IDbCommand cmnd = dbcon.CreateCommand();

            cmnd.CommandText = "INSERT INTO " + Globals.TABL_ALLENAMENTO +
                " (" + Globals.COL_ALL_DATA + ", " + Globals.COL_ALL_DURATA + ", " + Globals.COL_ALL_FINITO + ", " + Globals.COL_ALL_ID_SCHEDA + ")" +
                "VALUES ('" + Data + "', '" + Durata + "', " + Bit + ", " + IDscheda + ")";
            cmnd.ExecuteNonQuery();
            //per l'ID
            IDbCommand cmnd_read = dbcon.CreateCommand();
            IDataReader reader;
            cmnd_read.CommandText = "SELECT seq FROM sqlite_sequence WHERE name = '" + Globals.TABL_ALLENAMENTO + "'";
            reader = cmnd_read.ExecuteReader();
            while (reader.Read())
            {
                ID = int.Parse(reader[0].ToString());
            }
        }
        catch
        {
            print("errore");
        }
        return ID;
    }
    #endregion
    #region Attività
    public void InsertAttivita(int Serie, int RipN, string RipT, double Peso, string Recupero, int IDdet, int IDall)
    {
        try
        {
            IDbConnection dbcon = Connection();
            IDbCommand cmnd = dbcon.CreateCommand();


            cmnd.CommandText = "INSERT INTO " + Globals.TABL_ATTIVITA +
                " (" + Globals.COL_ATT_SERIE + ", " + Globals.COL_ATT_RIPETIZIONIn + ", " + Globals.COL_ATT_RIPETIZIONIt + ", " + Globals.COL_ATT_PESO + ", " + Globals.COL_ATT_RECUPERO + ", " + Globals.COL_ATT_ID_DETTAGLI + ", " + Globals.COL_ATT_ID_ALLENAMENTO + ")" +
                "VALUES (" + Serie + ", " + RipN + ", '" + RipT + "', " + Peso + ", '" + Recupero + "', " + IDdet + ", " + IDall + ")";
            cmnd.ExecuteNonQuery();
        }
        catch
        {
            print("errore");
        }
    }
    #endregion
    #region Esercizio
    public int InsertEsercizio(string Nome, string Note, string Consigli, byte[] byteImage)
    {
        DataTable dtTable = new DataTable();
        dtTable.Columns.Add(Globals.COL_ESE_ID, typeof(int));
        try
        {
            IDbConnection dbcon = Connection();
            IDbCommand cmnd = dbcon.CreateCommand();

            cmnd.CommandText = "INSERT INTO " + Globals.TABL_ESERCIZIO +
                " (" + Globals.COL_ESE_NOME + ", " + Globals.COL_ESE_NOTE + ", " + Globals.COL_ESE_CONSIGLI + ", " + Globals.COL_ESE_IMMAGINE + ")" +
                "VALUES ('" + Nome + "', '" + Note + "', '" + Consigli + "', @image)";
            SqliteParameter parameter = new SqliteParameter("@image", System.Data.DbType.Binary);
            parameter.Value = byteImage;
            cmnd.Parameters.Add(parameter);
            cmnd.ExecuteNonQuery();

            //per l'ID
            IDbCommand cmnd_read = dbcon.CreateCommand();
            IDataReader reader;
            cmnd_read.CommandText = "SELECT seq FROM sqlite_sequence WHERE name = '" + Globals.TABL_ESERCIZIO + "'";
            reader = cmnd_read.ExecuteReader();
            while (reader.Read())
            {
                dtTable.Rows.Add(int.Parse(reader[0].ToString()));
            }
        }
        catch (Exception ex)
        {
            print("errore: " + ex);
        }
        return int.Parse(dtTable.Rows[0][Globals.COL_ESE_ID].ToString());
    }
    #endregion
    #region Dettagli es
    public void InsertDettagli(int Serie, string NoT, int RipN, string RipT, string Recupero, string Attesa, int Posizione, int IDese, int IDsch)
    {
        try
        {
            IDbConnection dbcon = Connection();
            IDbCommand cmnd = dbcon.CreateCommand();


            cmnd.CommandText = "INSERT INTO " + Globals.TABL_DETTAGLI +
                " (" + Globals.COL_DET_SERIE + ", " + Globals.COL_DET_RIPnOt + ", " + Globals.COL_DET_RIPETIZIONIn + ", " + Globals.COL_DET_RIPETIZIONIt + ", " + Globals.COL_DET_RECUPERO + ", " + Globals.COL_DET_ATTESA + ", " + Globals.COL_DET_POSIZIONE + ", " + Globals.COL_DET_ID_ESERCIZIO + ", " + Globals.COL_DET_ID_SCHEDA + ")" +
                "VALUES (" + Serie + ", '" + NoT + "', " + RipN + ", '" + RipT + "', '" + Recupero + "', '" + Attesa + "', " + Posizione + ", " + IDese + ", " + IDsch + ")";
            cmnd.ExecuteNonQuery();
        }
        catch
        {
            print("errore");
        }
    }
    #endregion
    #region Muscolo
    public void InsertMuscolo(int ID, string Nome)
    {
        try
        {
            IDbConnection dbcon = Connection();
            IDbCommand cmnd = dbcon.CreateCommand();


            cmnd.CommandText = "INSERT INTO " + Globals.TABL_MUSCOLO +
                " (" + Globals.COL_MUS_ID + ", " + Globals.COL_MUS_NOME + ")" +
                "VALUES (" + ID + ", '" + Nome + "')";
            cmnd.ExecuteNonQuery();
        }
        catch
        {
            print("errore");
        }
    }
    #endregion
    #region M_E
    public void InsertM_E(int IDes, int IDmu)
    {
        try
        {
            IDbConnection dbcon = Connection();
            IDbCommand cmnd = dbcon.CreateCommand();

            cmnd.CommandText = "INSERT INTO " + Globals.TABL_ME +
                " (" + Globals.COL_MxE_IDESERCIZIO + ", " + Globals.COL_MxE_IDMUSCULO + ")" +
                "VALUES (" + IDes + ", " + IDmu + ")";
            cmnd.ExecuteNonQuery();
        }
        catch
        {
            print("errore");
        }
    }
    #endregion
    #region Scheda
    public int InsertScheda(string Nome)
    {
        int ID = 0;
        try
        {
            IDbConnection dbcon = Connection();
            IDbCommand cmnd = dbcon.CreateCommand();


            cmnd.CommandText = "INSERT INTO " + Globals.TABL_SCHEDA +
                " (" + Globals.COL_SCH_NOME + ")" +
                "VALUES ('" + Nome + "')";
            cmnd.ExecuteNonQuery();
            //per l'ID
            IDbCommand cmnd_read = dbcon.CreateCommand();
            IDataReader reader;
            cmnd_read.CommandText = "SELECT seq FROM sqlite_sequence WHERE name = '" + Globals.TABL_SCHEDA + "'";
            reader = cmnd_read.ExecuteReader();
            while (reader.Read())
            {
                ID = int.Parse(reader[0].ToString());
            }
        }
        catch
        {
            print("errore");
        }
        return ID;
    }
    #endregion
    #endregion

    #region DELETE
    #region Allenamento
    public void DeleteAllenamento(int ID)
    {
        try
        {
            IDbConnection dbcon = Connection();
            IDbCommand cmnd = dbcon.CreateCommand();


            cmnd.CommandText = "DELETE FROM " + Globals.TABL_ALLENAMENTO + " WHERE " + Globals.COL_ALL_ID + " = " + ID;
            cmnd.ExecuteNonQuery();
        }
        catch
        {
            print("errore");
        }
    }
    #endregion
    #region Attivita
    public void DeleteAttivita(int ID)
    {
        try
        {
            IDbConnection dbcon = Connection();
            IDbCommand cmnd = dbcon.CreateCommand();


            cmnd.CommandText = "DELETE FROM " + Globals.TABL_ATTIVITA + " WHERE " + Globals.COL_ATT_ID + " = " + ID;
            cmnd.ExecuteNonQuery();
        }
        catch
        {
            print("errore");
        }
    }
    #endregion
    #region Esercizio
    public bool DeleteEsercizio(int ID)
    {
        try
        {
            IDbConnection dbcon = Connection();
            IDbCommand cmnd = dbcon.CreateCommand();


            cmnd.CommandText = "DELETE FROM " + Globals.TABL_ESERCIZIO + " WHERE " + Globals.COL_ESE_ID + " = " + ID;
            cmnd.ExecuteNonQuery();
            return true;
        }
        catch
        {
            print("errore");
            return false;
        }
    }
    #endregion
    #region Dettagli es
    public bool DeleteDettagli(int ID)
    {
        try
        {
            IDbConnection dbcon = Connection();
            IDbCommand cmnd = dbcon.CreateCommand();


            cmnd.CommandText = "DELETE FROM " + Globals.TABL_DETTAGLI + " WHERE " + Globals.COL_DET_ID + " = " + ID;
            cmnd.ExecuteNonQuery();
            return true;
        }
        catch
        {
            print("errore");
            return false;
        }
    }
    #endregion
    #region Muscolo
    public void DeleteMuscolo(int ID)
    {
        try
        {
            IDbConnection dbcon = Connection();
            IDbCommand cmnd = dbcon.CreateCommand();


            cmnd.CommandText = "DELETE FROM " + Globals.TABL_MUSCOLO + " WHERE " + Globals.COL_MUS_ID + " = " + ID;
            cmnd.ExecuteNonQuery();
        }
        catch
        {
            print("errore");
        }
    }
    #endregion
    #region M_E
    public bool DeleteME(int ID)
    {
        try
        {
            IDbConnection dbcon = Connection();
            IDbCommand cmnd = dbcon.CreateCommand();


            cmnd.CommandText = "DELETE FROM " + Globals.TABL_ME + " WHERE " + Globals.COL_MxE_ID + " = " + ID;
            cmnd.ExecuteNonQuery();
            return true;
        }
        catch
        {
            print("errore");
            return false;
        }
    }
    #endregion
    #region Scheda
    public bool DeleteScheda(int ID)
    {
        try
        {
            IDbConnection dbcon = Connection();
            IDbCommand cmnd = dbcon.CreateCommand();


            cmnd.CommandText = "DELETE FROM " + Globals.TABL_SCHEDA + " WHERE " + Globals.COL_SCH_ID + " = " + ID;
            cmnd.ExecuteNonQuery();
            return true;
        }
        catch
        {
            print("errore");
            return false;
        }
    }
    #endregion
    #endregion

    #region UPDATE
    #region Allenamento
    public void UpdateAllenamento(int ID, string Data, string Durata, int Bit, int IDscheda)
    {
        try
        {
            IDbConnection dbcon = Connection();
            IDbCommand cmnd = dbcon.CreateCommand();


            cmnd.CommandText = "UPDATE " + Globals.TABL_ALLENAMENTO +
                " SET " + Globals.COL_ALL_DATA + " = '" + Data + "', " + Globals.COL_ALL_DURATA + " = '" + Durata + "', " + Globals.COL_ALL_FINITO + " = " + Bit + ", " + Globals.COL_ALL_ID_SCHEDA + " = " + IDscheda +
                " WHERE " + Globals.COL_ALL_ID + " = " + ID;
            cmnd.ExecuteNonQuery();
        }
        catch
        {
            print("errore");
        }
    }
    #endregion
    #region Attività
    public void UpdateAttività(int ID, int Serie, int RipN, string RipT, double Peso, string Recupero, int IDdet, int IDall)
    {
        try
        {
            IDbConnection dbcon = Connection();
            IDbCommand cmnd = dbcon.CreateCommand();


            cmnd.CommandText = "UPDATE " + Globals.TABL_ATTIVITA +
                " SET " + Globals.COL_ATT_SERIE + " = " + Serie + ", " + Globals.COL_ATT_RIPETIZIONIn + " = " + RipN + ", " + Globals.COL_ATT_RIPETIZIONIt + " = '" + RipT + "', " + Globals.COL_ATT_PESO + " = " + Peso + ", " + Globals.COL_ATT_RECUPERO + " = '" + Recupero + "', " + Globals.COL_ATT_ID_DETTAGLI + " = " + IDdet + ", " + Globals.COL_ATT_ID_ALLENAMENTO + " = " + IDall +
                " WHERE " + Globals.COL_ATT_ID + " = " + ID;
            cmnd.ExecuteNonQuery();
        }
        catch
        {
            print("errore");
        }
    }
    #endregion
    #region Esercizio
    public void UpdateEsercizio(int ID, string Nome, string Note, string Consigli, byte[] byteImage)
    {
        try
        {
            IDbConnection dbcon = Connection();
            IDbCommand cmnd = dbcon.CreateCommand();


            cmnd.CommandText = "UPDATE " + Globals.TABL_ESERCIZIO +
                " SET " + Globals.COL_ESE_NOME + " = '" + Nome + "', " + Globals.COL_ESE_NOTE + " = '" + Note + "', " + Globals.COL_ESE_CONSIGLI + " = '" + Consigli + "', " + Globals.COL_ESE_IMMAGINE + " = @image" +
                " WHERE " + Globals.COL_ESE_ID + " = " + ID;
            SqliteParameter parameter = new SqliteParameter("@image", System.Data.DbType.Binary);
            parameter.Value = byteImage;
            cmnd.Parameters.Add(parameter);
            cmnd.ExecuteNonQuery();
        }
        catch
        {
            print("errore");
        }
    }
    #endregion
    #region Dettagli es
    public bool UpdateDettagli(int ID, int Serie, string NoT, int RipN, string RipT, int Posizione, string Recupero, string Attesa, int IDes, int IDsc)
    {
        try
        {
            IDbConnection dbcon = Connection();
            IDbCommand cmnd = dbcon.CreateCommand();


            cmnd.CommandText = "UPDATE " + Globals.TABL_DETTAGLI +
                " SET " + Globals.COL_DET_SERIE + " = " + Serie + ", " + Globals.COL_DET_RIPnOt + " = '" + NoT + "', " + Globals.COL_DET_RIPETIZIONIn + " = " + RipN + ", " + Globals.COL_DET_RIPETIZIONIt + " = '" + RipT + "', " + Globals.COL_DET_RECUPERO + " = '" + Recupero + "', " + Globals.COL_DET_ATTESA + " = '" + Attesa + "', " + Globals.COL_DET_POSIZIONE + " = " + Posizione + ", " + Globals.COL_DET_ID_ESERCIZIO + " = " + IDes + ", " + Globals.COL_DET_ID_SCHEDA + " = " + IDsc +
                " WHERE " + Globals.COL_DET_ID + " = " + ID;
            cmnd.ExecuteNonQuery();
            return true;
        }
        catch
        {
            print("errore");
            return false;
        }
    }
    #endregion
    #region Muscolo
    public void UpdateMuscolo(int ID, string Nome)
    {
        try
        {
            IDbConnection dbcon = Connection();
            IDbCommand cmnd = dbcon.CreateCommand();


            cmnd.CommandText = "UPDATE " + Globals.TABL_MUSCOLO +
                " SET " + Globals.COL_MUS_NOME + " = '" + Nome + "'" +
                " WHERE " + Globals.COL_MUS_ID + " = " + ID;
            cmnd.ExecuteNonQuery();
        }
        catch
        {
            print("errore");
        }
    }
    #endregion
    #region M_E
    public void UpdateME(int ID, int IDes, int IDmu)
    {
        try
        {
            IDbConnection dbcon = Connection();
            IDbCommand cmnd = dbcon.CreateCommand();


            cmnd.CommandText = "UPDATE " + Globals.TABL_ME +
                " SET " + Globals.COL_MxE_IDESERCIZIO + " = " + IDes + ", " + Globals.COL_MxE_IDMUSCULO + " = " + IDmu +
                " WHERE " + Globals.COL_MxE_ID + " = " + ID;
            cmnd.ExecuteNonQuery();
        }
        catch
        {
            print("errore");
        }
    }
    #endregion
    #region Scheda
    public bool UpdateScheda(int ID, string Nome)
    {
        try
        {
            IDbConnection dbcon = Connection();
            IDbCommand cmnd = dbcon.CreateCommand();


            cmnd.CommandText = "UPDATE " + Globals.TABL_SCHEDA +
                " SET " + Globals.COL_SCH_NOME + " = '" + Nome + "'" +
                " WHERE " + Globals.COL_SCH_ID + " = " + ID;
            cmnd.ExecuteNonQuery();
            return true;
        }
        catch
        {
            print("errore");
            return false;
        }
    }
    #endregion
    #endregion

    #region SELECT ALL
    #region Allenamento
    public DataTable SelectAllAllenamento(string Condizione, string Ordinamento)
    {
        DataTable dtTable = new DataTable();
        try
        {
            IDbConnection dbcon = Connection();
            IDbCommand cmnd = dbcon.CreateCommand();


            string query = "SELECT * FROM " + Globals.TABL_ALLENAMENTO + Condizione + " ORDER BY " + Ordinamento;
            IDbCommand cmnd_read = dbcon.CreateCommand();
            IDataReader reader;
            cmnd_read.CommandText = query;
            reader = cmnd_read.ExecuteReader();
            dtTable.Columns.Add(Globals.COL_ALL_ID, typeof(int)); //0
            dtTable.Columns.Add(Globals.COL_ALL_DATA, typeof(DateTime)); //1
            dtTable.Columns.Add(Globals.COL_ALL_DURATA, typeof(string)); //2
            dtTable.Columns.Add(Globals.COL_ALL_FINITO, typeof(bool)); //3
            dtTable.Columns.Add(Globals.COL_ALL_ID_SCHEDA, typeof(int)); //4
            while (reader.Read())
            {
                int r0 = int.Parse(reader[0].ToString());
                DateTime r1 = DateTime.Parse(reader[1].ToString());
                string r2 = reader[2].ToString();
                bool r3 = (bool)reader[3];
                int r4 = int.Parse(reader[4].ToString());

                dtTable.Rows.Add(r0, r1, r2, r3, r4);
            }
        }
        catch (Exception ex)
        {
            print("errore: " + ex);
        }
        return dtTable;
    }
    #endregion
    #region Attivita
    public DataTable SelectAllAttivita(string Condizione, string Ordinamento)
    {
        DataTable dtTable = new DataTable();
        try
        {
            IDbConnection dbcon = Connection();
            IDbCommand cmnd = dbcon.CreateCommand();


            string query = "SELECT * FROM ((((" + Globals.TABL_ATTIVITA + " AS tt " +
                "INNER JOIN " + Globals.TABL_ALLENAMENTO + " AS ll " +
                "ON tt." + Globals.COL_ATT_ID_ALLENAMENTO + " = ll." + Globals.COL_ALL_ID + ") " +
                "INNER JOIN " + Globals.TABL_DETTAGLI + " AS de " +
                "ON tt." + Globals.COL_ATT_ID_DETTAGLI + " = de." + Globals.COL_DET_ID + ") " +
                "INNER JOIN " + Globals.TABL_SCHEDA + " AS sc " +
                "ON de." + Globals.COL_DET_ID_SCHEDA + " = sc." + Globals.COL_SCH_ID + ") " +
                "INNER JOIN " + Globals.TABL_ESERCIZIO + " AS es " +
                "ON de." + Globals.COL_DET_ID_ESERCIZIO + " = es." + Globals.COL_ESE_ID + ")" +
                Condizione + " ORDER BY " + Ordinamento;
            IDbCommand cmnd_read = dbcon.CreateCommand();
            IDataReader reader;
            cmnd_read.CommandText = query;
            reader = cmnd_read.ExecuteReader();
            dtTable.Columns.Add(Globals.COL_ATT_ID, typeof(int)); //0
            dtTable.Columns.Add(Globals.COL_ATT_SERIE, typeof(int)); //1
            dtTable.Columns.Add(Globals.COL_ATT_RIPETIZIONIn, typeof(int)); //2
            dtTable.Columns.Add(Globals.COL_ATT_RIPETIZIONIt, typeof(string)); //3
            dtTable.Columns.Add(Globals.COL_ATT_PESO, typeof(double)); //4
            dtTable.Columns.Add(Globals.COL_ATT_RECUPERO, typeof(string)); //5
            //dtTable.Columns.Add(Globals.COL_ATT_ID_DETTAGLI, typeof(int)); //6
            //dtTable.Columns.Add(Globals.COL_ATT_ID_ALLENAMENTO, typeof(int)); //7
            dtTable.Columns.Add(Globals.COL_ALL_ID, typeof(int)); //8
            dtTable.Columns.Add(Globals.COL_ALL_DATA, typeof(string)); //9
            dtTable.Columns.Add(Globals.COL_ALL_DURATA, typeof(string)); //10
            dtTable.Columns.Add(Globals.COL_ALL_FINITO, typeof(string)); //11
            //dtTable.Columns.Add(Globals.COL_ALL_ID_SCHEDA, typeof(int)); //12
            dtTable.Columns.Add(Globals.COL_DET_ID, typeof(int)); //13
            dtTable.Columns.Add(Globals.COL_DET_SERIE, typeof(int)); //14
            dtTable.Columns.Add(Globals.COL_DET_RIPnOt, typeof(string)); //15
            dtTable.Columns.Add(Globals.COL_DET_RIPETIZIONIn, typeof(int)); //16
            dtTable.Columns.Add(Globals.COL_DET_RIPETIZIONIt, typeof(string)); //17
            dtTable.Columns.Add(Globals.COL_DET_RECUPERO, typeof(string)); //18
            dtTable.Columns.Add(Globals.COL_DET_ATTESA, typeof(string)); //19
            dtTable.Columns.Add(Globals.COL_DET_POSIZIONE, typeof(int)); //20
            //dtTable.Columns.Add(Globals.COL_DET_ID_ESERCIZIO, typeof(int)); //21
            //dtTable.Columns.Add(Globals.COL_DET_ID_SCHEDA, typeof(int)); //22
            dtTable.Columns.Add(Globals.COL_SCH_ID, typeof(int)); //23
            dtTable.Columns.Add(Globals.COL_SCH_NOME, typeof(string)); //24
            dtTable.Columns.Add(Globals.COL_ESE_ID, typeof(int)); //25
            dtTable.Columns.Add(Globals.COL_ESE_NOME, typeof(string)); //26
            dtTable.Columns.Add(Globals.COL_ESE_CONSIGLI, typeof(string)); //27
            dtTable.Columns.Add(Globals.COL_ESE_NOTE, typeof(string)); //28
            //dtTable.Columns.Add(Globals.COL_ESE_IMMAGINE, typeof(string)); //29
            while (reader.Read())
            {
                int r0 = int.Parse(reader[0].ToString());
                int r1 = int.Parse(reader[1].ToString());
                int r2 = int.Parse(reader[2].ToString());
                string r3 = reader[3].ToString();
                double r4 = double.Parse(reader[4].ToString());
                string r5 = reader[5].ToString();
                //int r6 = int.Parse(reader[6].ToString());
                //int r7 = int.Parse(reader[7].ToString());
                int r8 = int.Parse(reader[8].ToString());
                string r9 = reader[9].ToString();
                string r10 = reader[10].ToString();
                string r11 = reader[11].ToString();
                //int r12 = int.Parse(reader[12].ToString());
                int r13 = int.Parse(reader[13].ToString());
                int r14 = int.Parse(reader[14].ToString());
                string r15 = reader[15].ToString();
                int r16 = int.Parse(reader[16].ToString());
                string r17 = reader[17].ToString();
                string r18 = reader[18].ToString();
                string r19 = reader[19].ToString();
                int r20 = int.Parse(reader[20].ToString());
                //int r21 = int.Parse(reader[21].ToString());
                //int r22 = int.Parse(reader[22].ToString());
                int r23 = int.Parse(reader[23].ToString());
                string r24 = reader[24].ToString();
                int r25 = int.Parse(reader[25].ToString());
                string r26 = reader[26].ToString();
                string r27 = reader[27].ToString();
                string r28 = reader[28].ToString();
                //string r29 = reader[29].ToString();

                dtTable.Rows.Add(r0, r1, r2, r3, r4, r5, r8, r9, r10, r11, r13, r14, r15, r16, r17, r18, r19, r20, r23, r24, r25, r26, r27, r28);
            }
        }
        catch (Exception EX)
        {
            print("errore" + EX);
        }
        return dtTable;
    }
    #endregion
    #region Esercizio
    public Tuple<DataTable, List<byte[]>> SelectAllEsercizi(string Condizione, string Ordinamento)
    {
        DataTable dtTable = new DataTable();
        List<byte[]> arrayList = new List<byte[]>();
        try
        {
            IDbConnection dbcon = Connection();
            IDbCommand cmnd = dbcon.CreateCommand();


            string query = "SELECT * FROM " + Globals.TABL_ESERCIZIO + Condizione + " ORDER BY " + Ordinamento;
            IDbCommand cmnd_read = dbcon.CreateCommand();
            IDataReader reader;
            cmnd_read.CommandText = query;
            reader = cmnd_read.ExecuteReader();
            dtTable.Columns.Add(Globals.COL_ESE_ID, typeof(int)); //0
            dtTable.Columns.Add(Globals.COL_ESE_NOME, typeof(string)); //1
            dtTable.Columns.Add(Globals.COL_ESE_CONSIGLI, typeof(string)); //2
            dtTable.Columns.Add(Globals.COL_ESE_NOTE, typeof(string)); //3
            //dtTable.Columns.Add(Globals.COL_ESE_IMMAGINE, typeof(byte[])); //4
            while (reader.Read())
            {
                int r0 = int.Parse(reader[0].ToString());
                string r1 = reader[1].ToString();
                string r2 = reader[2].ToString();
                string r3 = reader[3].ToString();
                string sCheckByteNull = reader[4].ToString();
                byte[] r4 = null;
                if (sCheckByteNull != "")
                    r4 = (byte[])reader[4];

                dtTable.Rows.Add(r0, r1, r2, r3);
                arrayList.Add(r4);
            }
        }
        catch
        {
            print("errore");
        }
        return new Tuple<DataTable, List<byte[]>>(dtTable, arrayList);
    }
    #endregion
    #region Dettagli es
    public Tuple<DataTable, List<byte[]>> SelectAllDettagli(string Condizione, string Ordinamento)
    {
        List<byte[]> arrayList = new List<byte[]>();
        DataTable dtTable = new DataTable();
        try
        {
            IDbConnection dbcon = Connection();
            IDbCommand cmnd = dbcon.CreateCommand();


            string query = "SELECT * FROM ((" + Globals.TABL_DETTAGLI + " AS det " +
                "INNER JOIN " + Globals.TABL_SCHEDA + " AS sc " +
                "ON det." + Globals.COL_DET_ID_SCHEDA + " = sc." + Globals.COL_SCH_ID + ") " +
                "INNER JOIN " + Globals.TABL_ESERCIZIO + " AS es " +
                "ON det." + Globals.COL_DET_ID_ESERCIZIO + " = es." + Globals.COL_ESE_ID + ")" +
                Condizione + " ORDER BY " + Ordinamento;
            IDbCommand cmnd_read = dbcon.CreateCommand();
            IDataReader reader;
            cmnd_read.CommandText = query;
            reader = cmnd_read.ExecuteReader();
            dtTable.Columns.Add(Globals.COL_DET_ID, typeof(int)); //0
            dtTable.Columns.Add(Globals.COL_DET_SERIE, typeof(int)); //1
            dtTable.Columns.Add(Globals.COL_DET_RIPnOt, typeof(string)); //2
            dtTable.Columns.Add(Globals.COL_DET_RIPETIZIONIn, typeof(int)); //3
            dtTable.Columns.Add(Globals.COL_DET_RIPETIZIONIt, typeof(string)); //4
            dtTable.Columns.Add(Globals.COL_DET_RECUPERO, typeof(string)); //5
            dtTable.Columns.Add(Globals.COL_DET_ATTESA, typeof(string)); //6
            dtTable.Columns.Add(Globals.COL_DET_POSIZIONE, typeof(int)); //7
            //dtTable.Columns.Add(Globals.COL_DET_ID_ESERCIZIO, typeof(int)); //8
            //dtTable.Columns.Add(Globals.COL_DET_ID_SCHEDA, typeof(int)); //9
            dtTable.Columns.Add(Globals.COL_SCH_ID, typeof(int)); //10
            dtTable.Columns.Add(Globals.COL_SCH_NOME, typeof(string)); //11
            dtTable.Columns.Add(Globals.COL_ESE_ID, typeof(int)); //12
            dtTable.Columns.Add(Globals.COL_ESE_NOME, typeof(string)); //13
            dtTable.Columns.Add(Globals.COL_ESE_CONSIGLI, typeof(string)); //14
            dtTable.Columns.Add(Globals.COL_ESE_NOTE, typeof(string)); //15
            //dtTable.Columns.Add(Globals.COL_ESE_IMMAGINE, typeof(string)); //16

            while (reader.Read())
            {
                int r0 = int.Parse(reader[0].ToString());
                int r1 = int.Parse(reader[1].ToString());
                string r2 = reader[2].ToString();
                int r3 = int.Parse(reader[3].ToString());
                string r4 = reader[4].ToString();
                string r5 = reader[5].ToString();
                string r6 = reader[6].ToString();
                int r7 = int.Parse(reader[7].ToString());
                //int r8 = int.Parse(reader[8].ToString());
                //int r9 = int.Parse(reader[9].ToString());
                int r10 = int.Parse(reader[10].ToString());
                string r11 = reader[11].ToString();
                int r12 = int.Parse(reader[12].ToString());
                string r13 = reader[13].ToString();
                string r14 = reader[14].ToString();
                string r15 = reader[15].ToString();

                string sCheckByteNull = reader[16].ToString();
                byte[] r16 = null;
                if (sCheckByteNull != "")
                    r16 = (byte[])reader[16];

                dtTable.Rows.Add(r0, r1, r2, r3, r4, r5, r6, r7, r10, r11, r12, r13, r14, r15);
                arrayList.Add(r16);
            }
        }
        catch (Exception ex)
        {
            print("errore:" + ex);
        }
        return new Tuple<DataTable, List<byte[]>>(dtTable, arrayList);
    }
    #endregion
    #region Muscolo
    public DataTable SelectAllMuscoli(string Condizione, string Ordinamento)
    {
        DataTable dtTable = new DataTable();
        try
        {
            IDbConnection dbcon = Connection();
            IDbCommand cmnd = dbcon.CreateCommand();


            string query = "SELECT * FROM " + Globals.TABL_MUSCOLO + Condizione + " ORDER BY " + Ordinamento;
            IDbCommand cmnd_read = dbcon.CreateCommand();
            IDataReader reader;
            cmnd_read.CommandText = query;
            reader = cmnd_read.ExecuteReader();
            dtTable.Columns.Add(Globals.COL_MUS_ID, typeof(int));
            dtTable.Columns.Add(Globals.COL_MUS_NOME, typeof(string));
            while (reader.Read())
            {
                int r0 = int.Parse(reader[0].ToString());
                string r1 = reader[1].ToString();

                dtTable.Rows.Add(r0, r1);
            }
        }
        catch
        {
            print("errore");
        }
        return dtTable;
    }
    #endregion
    #region M_E
    public DataTable SelectAllME(string Condizione, string Ordinamento)
    {
        DataTable dtTable = new DataTable();
        try
        {
            IDbConnection dbcon = Connection();
            IDbCommand cmnd = dbcon.CreateCommand();

            string query = "SELECT * FROM ((" + Globals.TABL_ME + " AS me " +
                "INNER JOIN " + Globals.TABL_MUSCOLO + " AS mu " +
                "ON me." + Globals.COL_MxE_IDMUSCULO + " = mu." + Globals.COL_MUS_ID + ") " +
                "INNER JOIN " + Globals.TABL_ESERCIZIO + " AS es " +
                "ON me." + Globals.COL_MxE_IDESERCIZIO + " = es." + Globals.COL_ESE_ID + ") " +
                Condizione + " ORDER BY " + Ordinamento;
            IDbCommand cmnd_read = dbcon.CreateCommand();
            IDataReader reader;
            cmnd_read.CommandText = query;
            reader = cmnd_read.ExecuteReader();
            dtTable.Columns.Add(Globals.COL_MxE_ID, typeof(int)); //0
            //dtTable.Columns.Add(Globals.COL_MxE_IDESERCIZIO, typeof(int)); //1
            //dtTable.Columns.Add(Globals.COL_MxE_IDMUSCULO, typeof(int)); //2
            dtTable.Columns.Add(Globals.COL_MUS_ID, typeof(int)); //3
            dtTable.Columns.Add(Globals.COL_MUS_NOME, typeof(string)); //4
            dtTable.Columns.Add(Globals.COL_ESE_ID, typeof(int)); //5
            dtTable.Columns.Add(Globals.COL_ESE_NOME, typeof(string)); //6
            dtTable.Columns.Add(Globals.COL_ESE_CONSIGLI, typeof(string)); //7
            dtTable.Columns.Add(Globals.COL_ESE_NOTE, typeof(string)); //8
            //dtTable.Columns.Add(Globals.COL_ESE_IMMAGINE, typeof(byte[])); //9
            while (reader.Read())
            {
                int r0 = int.Parse(reader[0].ToString());
                //int r1 = int.Parse(reader[1].ToString());
                //int r2 = int.Parse(reader[2].ToString());
                int r3 = int.Parse(reader[3].ToString());
                string r4 = reader[4].ToString();
                int r5 = int.Parse(reader[5].ToString());
                string r6 = reader[6].ToString();
                string r7 = reader[7].ToString();
                string r8 = reader[8].ToString();

                dtTable.Rows.Add(r0, r3, r4, r5, r6, r7, r8);
            }
        }
        catch
        {
            print("errore");
        }
        return dtTable;
    }
    #endregion
    #region Scheda
    public DataTable SelectAllSchede(string Condizione, string Ordinamento)
    {
        DataTable dtTable = new DataTable();
        try
        {
            IDbConnection dbcon = Connection();
            IDbCommand cmnd = dbcon.CreateCommand();


            string query = "SELECT * FROM " + Globals.TABL_SCHEDA + Condizione + " ORDER BY " + Ordinamento;
            IDbCommand cmnd_read = dbcon.CreateCommand();
            IDataReader reader;
            cmnd_read.CommandText = query;
            reader = cmnd_read.ExecuteReader();
            dtTable.Columns.Add(Globals.COL_SCH_ID, typeof(int));
            dtTable.Columns.Add(Globals.COL_SCH_NOME, typeof(string));
            while (reader.Read())
            {
                int r0 = int.Parse(reader[0].ToString());
                string r1 = reader[1].ToString();

                dtTable.Rows.Add(r0, r1);
            }
        }
        catch
        {
            print("errore");
        }
        return dtTable;
    }
    #endregion
    #endregion

    #region SELECT
    #region MAX posizione
    public int SelectMaxPosizione(string Condizione)
    {
        int Max = 0;
        try
        {
            IDbConnection dbcon = Connection();
            IDbCommand cmnd = dbcon.CreateCommand();


            string query = "SELECT MAX(" + Globals.COL_DET_POSIZIONE + ") FROM " + Globals.TABL_DETTAGLI + Condizione;
            IDbCommand cmnd_read = dbcon.CreateCommand();
            IDataReader reader;
            cmnd_read.CommandText = query;
            reader = cmnd_read.ExecuteReader();
            while (reader.Read())
            {
                Max = int.Parse(reader[0].ToString());
            }
        }
        catch
        {
            print("errore");
        }
        return Max;
    }
    #endregion
    #endregion
}
