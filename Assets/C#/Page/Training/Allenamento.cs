using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Allenamento : MonoBehaviour
{
    public void InitAllenamento(int IDscheda)
    {
        Globals.INTERO = IDscheda;
        AppManager.Instance.objects.PagesTraining("Allenamento");
        Globals.POSIZIONE_ATTUALE = 0;
        Globals.PP_ESERCIZIO.gameObject.SetActive(false);


        Globals.ID_ALLENAMENTO = AppManager.Instance.sql.InsertAllenamento(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), "00:00:00", 0, Globals.INTERO);

        string sCondition = " WHERE sc." + Globals.COL_SCH_ID + " = " + Globals.INTERO + " ";
        var tuple = AppManager.Instance.sql.SelectAllDettagli(sCondition, "det." + Globals.COL_DET_POSIZIONE);
        DataTable dtTable = tuple.Item1;

        int Tot = 0;
        string Recupero;
        for (int row = 0; row < dtTable.Rows.Count; row++)
        {
            for (int SerieN = 0; SerieN < int.Parse(dtTable.Rows[row][Globals.COL_DET_SERIE].ToString()); SerieN++)
            {
                if (SerieN == 0)
                    Recupero = dtTable.Rows[row][Globals.COL_DET_ATTESA].ToString();
                else
                    Recupero = dtTable.Rows[row][Globals.COL_DET_RECUPERO].ToString();
                if (Recupero != "00:00")
                    Tot += 1;
                Tot += 1;
            }
        }

        Globals.POSIZIONE_FINALE = Tot;
        Globals.ES_FATTO = "SI";

        gameObject.AddComponent<BarraSuperiore>();

        Globals.LAST_SECONDS = 0;

        Next();
    }

    public Tuple<int, string> OttieniDati()
    {
        DataTable dtTable;

        string sCondition = " WHERE sc." + Globals.COL_SCH_ID + " = " + Globals.INTERO + " ";
        var tuple = AppManager.Instance.sql.SelectAllDettagli(sCondition, "det." + Globals.COL_DET_POSIZIONE);
        dtTable = tuple.Item1;

        int Num = 0;
        string Recupero;

        int Secondi = 0;
        int IDdet = 0;
        string Tipo = "Recupero";

        int[] Serie = new int[dtTable.Rows.Count];
        int ActualSerie = 0;
        int RowSerie = 0;

        bool Rec = true;

        //la logica è che il recupero c'è sempre a meno che non sia 0
        for (int row = 0; row < dtTable.Rows.Count; row++)
        {
            for (int rep = 0; rep < int.Parse(dtTable.Rows[row][Globals.COL_DET_SERIE].ToString()); rep++)
            {
                if (rep == 0)
                    Recupero = dtTable.Rows[row][Globals.COL_DET_ATTESA].ToString();
                else
                    Recupero = dtTable.Rows[row][Globals.COL_DET_RECUPERO].ToString();
                if (Recupero != "00:00")
                {
                    Num += 1;
                    if (Num == Globals.POSIZIONE_ATTUALE)
                    {
                        Secondi = int.Parse(Recupero.Substring(0, 2)) * 60 + int.Parse(Recupero.Substring(3, 2));
                        Tipo = "Recupero";
                        ActualSerie = Serie[row];
                        RowSerie = row;
                    }
                }
                Num += 1;
                if (Num == Globals.POSIZIONE_ATTUALE)
                {
                    IDdet = int.Parse(dtTable.Rows[row][Globals.COL_DET_ID].ToString());
                    Tipo = "Esercizio";
                    Rec = false;
                    ActualSerie = Serie[row];
                    RowSerie = row;
                }
                Serie[row] += 1;
            }
        }
        Series(ActualSerie + 1, Serie[RowSerie]);
        if (Rec)
            return new Tuple<int, string>(Secondi, Tipo);
        else
            return new Tuple<int, string>(IDdet, Tipo);

    }
    public void Next()
    {
        if (Globals.ALLENAMENTO_MODE == "Recupero")
        {
            string sRecSeconds = Globals.PAGE_ALLENAMENTO.gameObject.transform.Find("BarraSuperiore/Tempo/Cronometro").GetComponent<TextMeshProUGUI>().text;
            int iRecSeconds = int.Parse(sRecSeconds.Substring(0, 2)) * 3060 + int.Parse(sRecSeconds.Substring(3, 2)) * 60 + int.Parse(sRecSeconds.Substring(6, 2));
            var RecTimeConverted = TimeSpan.FromSeconds(iRecSeconds - Globals.LAST_SECONDS_xRECUPERO);
            Globals.TIME_RECUPERO = RecTimeConverted.ToString(@"mm\:ss");
            Globals.LAST_ATT_DONE = "Recupero";
        }

        Globals.ALLENAMENTO_MODE = "Null";

        if (Globals.ES_FATTO == "SI")
        {
            var tupleDati1 = OttieniDati();
            if (tupleDati1.Item2 == "Esercizio")
            {
                DataTable dtAtt = AppManager.Instance.sql.SelectAllAttivita(" WHERE de." + Globals.COL_DET_ID + " = " + tupleDati1.Item1 + " AND ll." + Globals.COL_ALL_ID + " = " + Globals.ID_ALLENAMENTO, "de." + Globals.COL_DET_ID);
                int NSerie = 0;
                if (dtAtt.Rows.Count > 0)
                {
                    NSerie = int.Parse(dtAtt.Rows[0][Globals.COL_ATT_SERIE].ToString());
                }
                int RipN = 0;
                if (Globals.CNV_ESE_REPS.gameObject.activeSelf)
                    RipN = int.Parse(Globals.CNV_ESE_REPS.gameObject.transform.Find("txtReps").GetComponent<TextMeshProUGUI>().text);
                string sActualSeconds = Globals.PAGE_ALLENAMENTO.gameObject.transform.Find("BarraSuperiore/Tempo/Cronometro").GetComponent<TextMeshProUGUI>().text;
                int iActualSeconds = int.Parse(sActualSeconds.Substring(0, 2)) * 3060 + int.Parse(sActualSeconds.Substring(3, 2)) * 60 + int.Parse(sActualSeconds.Substring(6, 2));
                var TimeConverted = TimeSpan.FromSeconds(iActualSeconds - Globals.LAST_SECONDS);
                if (Globals.TIME_RECUPERO == null || Globals.LAST_ATT_DONE == "Esercizio")
                    Globals.TIME_RECUPERO = "00:00";

                AppManager.Instance.sql.InsertAttivita(NSerie + 1, RipN, TimeConverted.ToString(@"mm\:ss"), int.Parse(Globals.CNV_WEIGHT.gameObject.transform.Find("InputFieldWeight").GetComponent<TMP_InputField>().text), Globals.TIME_RECUPERO, tupleDati1.Item1, Globals.ID_ALLENAMENTO);
                Globals.LAST_ATT_DONE = "Esercizio";
                Globals.LAST_SECONDS_xRECUPERO = iActualSeconds;
            }
        }

        if (Globals.POSIZIONE_ATTUALE == Globals.POSIZIONE_FINALE)
        {
            Finito();
        }
        else
        {
            Globals.ES_FATTO = "SI";
            string sLastSeconds = Globals.PAGE_ALLENAMENTO.gameObject.transform.Find("BarraSuperiore/Tempo/Cronometro").GetComponent<TextMeshProUGUI>().text;
            Globals.LAST_SECONDS = int.Parse(sLastSeconds.Substring(0, 2)) * 3060 + int.Parse(sLastSeconds.Substring(3, 2)) * 60 + int.Parse(sLastSeconds.Substring(6, 2));

            Globals.PAGE_ALLENAMENTO.gameObject.transform.Find("Recupero").GetComponent<Canvas>().gameObject.SetActive(false);
            Globals.PAGE_ALLENAMENTO.gameObject.transform.Find("Esercizio").GetComponent<Canvas>().gameObject.SetActive(false);

            AllenamentoRecupero recupero = gameObject.AddComponent<AllenamentoRecupero>();
            Globals.COUNT_TIMER += 1;

            AllenamentoEsercizio esercizio = gameObject.AddComponent<AllenamentoEsercizio>();
            DataTable dtTable;

            string sCondition = " WHERE sc." + Globals.COL_SCH_ID + " = " + Globals.INTERO + " ";
            var tuple = AppManager.Instance.sql.SelectAllDettagli(sCondition, "det." + Globals.COL_DET_POSIZIONE);
            dtTable = tuple.Item1;

            Globals.POSIZIONE_ATTUALE += 1;

            Globals.PAGE_ALLENAMENTO.transform.Find("BarraEsercizi/cnvProgressBar").GetComponent<Canvas>().GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width * Globals.POSIZIONE_ATTUALE / Globals.POSIZIONE_FINALE, 5);

            BarraEsercizi barraEsercizi = gameObject.AddComponent<BarraEsercizi>();
            barraEsercizi.beMain();

            var tupleDati = OttieniDati();
            if (tupleDati.Item2 == "Recupero")
            {
                Globals.PAGE_ALLENAMENTO.gameObject.transform.Find("Esercizio").GetComponent<Canvas>().gameObject.SetActive(false);
                Globals.PAGE_ALLENAMENTO.gameObject.transform.Find("Recupero").GetComponent<Canvas>().gameObject.SetActive(true);
                recupero.Carica(tupleDati.Item1);
                Globals.ALLENAMENTO_MODE = "Recupero";
            }
            else
                esercizio.Carica(tupleDati.Item1);
        }
    }
    public int OttieniDatiNextEsercizio()
    {
        DataTable dtTable;

        string sCondition = " WHERE sc." + Globals.COL_SCH_ID + " = " + Globals.INTERO + " ";
        var tuple = AppManager.Instance.sql.SelectAllDettagli(sCondition, "det." + Globals.COL_DET_POSIZIONE);
        dtTable = tuple.Item1;

        int Num = 0;
        string Recupero;

        int Plus = 1;
        for (int row = 0; row < dtTable.Rows.Count; row++)
        {
            for (int rep = 0; rep < int.Parse(dtTable.Rows[row][Globals.COL_DET_SERIE].ToString()); rep++)
            {
                if (rep == 0)
                    Recupero = dtTable.Rows[row][Globals.COL_DET_ATTESA].ToString();
                else
                    Recupero = dtTable.Rows[row][Globals.COL_DET_RECUPERO].ToString();
                if (Recupero != "00:00")
                {
                    Num += 1;
                    if (Num == Globals.POSIZIONE_ATTUALE + Plus)
                    {
                        Plus += 1;
                    }
                }
                Num += 1;
                if (Num == Globals.POSIZIONE_ATTUALE + Plus)
                {
                    return int.Parse(dtTable.Rows[row][Globals.COL_DET_ID].ToString());
                }
            }
        }
        return default;
    }
    public void I_D_Weight(string IorD)
    {
        switch (IorD)
        {
            case "I":
                Globals.CNV_WEIGHT.gameObject.transform.Find("InputFieldWeight").GetComponent<TMP_InputField>().text = (int.Parse(Globals.CNV_WEIGHT.gameObject.transform.Find("InputFieldWeight").GetComponent<TMP_InputField>().text) + 1).ToString();
                break;
            case "D":
                if (int.Parse(Globals.CNV_WEIGHT.gameObject.transform.Find("InputFieldWeight").GetComponent<TMP_InputField>().text) > 0)
                    Globals.CNV_WEIGHT.gameObject.transform.Find("InputFieldWeight").GetComponent<TMP_InputField>().text = (int.Parse(Globals.CNV_WEIGHT.gameObject.transform.Find("InputFieldWeight").GetComponent<TMP_InputField>().text) - 1).ToString();
                break;
        }
    }
    public void WeightCanged()
    {
        int Num;
        int.TryParse(Globals.CNV_WEIGHT.gameObject.transform.Find("InputFieldWeight").GetComponent<TMP_InputField>().text, out Num);
        if (Num < 0)
            Globals.CNV_WEIGHT.gameObject.transform.Find("InputFieldWeight").GetComponent<TMP_InputField>().text = "0";
        if (Num > 1000)
            Globals.CNV_WEIGHT.gameObject.transform.Find("InputFieldWeight").GetComponent<TMP_InputField>().text = "1000";
        if (Globals.CNV_WEIGHT.gameObject.transform.Find("InputFieldWeight").GetComponent<TMP_InputField>().text == "")
            Globals.CNV_WEIGHT.gameObject.transform.Find("InputFieldWeight").GetComponent<TMP_InputField>().text = "0";
    }
    public void Back()
    {
        if (Globals.POSIZIONE_ATTUALE > 1)
        {
            Globals.ES_FATTO = "NO";
            Globals.POSIZIONE_ATTUALE -= 2;
            Globals.ALLENAMENTO_MODE = "Back";
            Globals.BOOL = false;
            Globals.CURRENT_TIME = 0;
        }
    }
    public void Skip()
    {
        Globals.ES_FATTO = "NO";
        Globals.BOOL = false;
        Globals.CURRENT_TIME = 0;
    }
    #region SQL
    private void sqlInsert()
    {
        string sCondition = " WHERE sc." + Globals.COL_SCH_ID + " = " + Globals.INTERO + " ";
        var tuple = AppManager.Instance.sql.SelectAllDettagli(sCondition, "det." + Globals.COL_DET_POSIZIONE);
        DataTable dtTable = tuple.Item1;
    }
    private void sqlFinish()
    {
        DataTable dtAllenamento = AppManager.Instance.sql.SelectAllAllenamento(" WHERE " + Globals.COL_ALL_ID + " = " + Globals.ID_ALLENAMENTO, Globals.COL_ALL_ID);
        string Durata = Globals.PAGE_ALLENAMENTO.gameObject.transform.Find("BarraSuperiore/Tempo/Cronometro").GetComponent<TextMeshProUGUI>().text;
        AppManager.Instance.sql.UpdateAllenamento(Globals.ID_ALLENAMENTO, dtAllenamento.Rows[0][Globals.COL_ALL_DATA].ToString(), Durata, 1, int.Parse(dtAllenamento.Rows[0][Globals.COL_ALL_ID_SCHEDA].ToString()));
    }
    private void Series(int ActualSerie, int TotalSeries)
    {
        Globals.CNV_ESE_TIMER.transform.parent.gameObject.transform.Find("txtSeries").GetComponent<TextMeshProUGUI>().text = ActualSerie + "/" + TotalSeries;
    }
    #endregion
    public void ClosePopupEsercizio()
    {
        Globals.PP_ESERCIZIO.gameObject.SetActive(false);
    }
    public void Finito()
    {
        Globals.ALLENAMENTO_MODE = "Null";

        DataTable dataTable = AppManager.Instance.sql.SelectAllAttivita(" WHERE ll." + Globals.COL_ALL_ID + " = " + Globals.ID_ALLENAMENTO, "ll." + Globals.COL_ALL_ID);
        if (dataTable.Rows.Count == 0)
        {
            AppManager.Instance.objects.btnEsciAllenamento();
        }
        else
        {
            sqlFinish();
            Finito finito = gameObject.AddComponent<Finito>();
            finito.Init();
        }
    }
}
