using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Insert : MonoBehaviour
{
    static bool[] bMuscolo;
    static string[] sMuscolo;
    static private TMP_InputField ifName;
    static private TMP_InputField ifAdvices;
    static private TMP_InputField ifNote;
    static private Image iImage;
    private void Start()
    {
        ifName = Globals.PAGE_EXER_ADD.transform.Find("InputFieldName").GetComponent<TMP_InputField>();
        ifAdvices = Globals.PAGE_EXER_ADD.transform.Find("InputFieldAdvices").GetComponent<TMP_InputField>();
        ifNote = Globals.PAGE_EXER_ADD.transform.Find("InputFieldNote").GetComponent<TMP_InputField>();
        iImage = Globals.PAGE_EXER_ADD.transform.Find("PanelImage/Image").GetComponent<Image>();
    }
    #region Esercizi
    private void InsertEsercizio()
    {
        try
        {
            string sNome = ifName.text;
            string sNote = ifNote.text;
            string sConsigli = ifAdvices.text;

            int IDes = AppManager.Instance.sql.InsertEsercizio(sNome, sNote, sConsigli, Globals.IMG_BYTE_INSERT);
            Globals.IMG_BYTE_INSERT = null;
            for (int row = 0; row < bMuscolo.Length; row++)
            {
                if (bMuscolo[row] == true)
                {
                    AppManager.Instance.sql.InsertM_E(IDes, row);
                }
            }
            AppManager.Instance.navManager.NavigateBack();
        }
        catch
        {
            AppManager.Instance.navManager.NavigateTo(() => AppManager.Instance.objects.Scenes("Esercizi"));
        }
    }
    private void UpdateEsercizio()
    {
        try
        {
            string sNome = ifName.text;
            string sNote = ifNote.text;
            string sConsigli = ifAdvices.text;

            DataTable dtEsMuscoli = AppManager.Instance.sql.SelectAllME(" WHERE me." + Globals.COL_MxE_IDESERCIZIO + " = " + Globals.INTERO, "me." + Globals.COL_MxE_IDMUSCULO);
            bool Trovato;
            for (int row = 0; row < bMuscolo.Length; row++)
            {
                if (bMuscolo[row] == true)
                {
                    Trovato = false;
                    for (int row2 = 0; row2 < dtEsMuscoli.Rows.Count; row2++)
                    {
                        if (row == int.Parse(dtEsMuscoli.Rows[row2][Globals.COL_MxE_IDMUSCULO].ToString()))
                            Trovato = true;
                    }
                    if (Trovato == false)
                        AppManager.Instance.sql.InsertM_E(Globals.INTERO, row);
                }
                else
                {
                    for (int row2 = 0; row2 < dtEsMuscoli.Rows.Count; row2++)
                    {
                        if (row == int.Parse(dtEsMuscoli.Rows[row2][Globals.COL_MxE_IDMUSCULO].ToString()))
                            AppManager.Instance.sql.DeleteME(int.Parse(dtEsMuscoli.Rows[row2][Globals.COL_MxE_ID].ToString()));
                    }
                }
            }
            AppManager.Instance.sql.UpdateEsercizio(Globals.INTERO, sNome, sNote, sConsigli, Globals.IMG_BYTE_INSERT);
            Globals.IMG_BYTE_INSERT = null;
            AppManager.Instance.navManager.NavigateTo(() => AppManager.Instance.objects.BtnHome());
        }
        catch
        {
            AppManager.Instance.navManager.NavigateTo(() => AppManager.Instance.objects.Scenes("Esercizi"));
        }
    }
    public void Init_InsertEsercizio()
    {
        Globals.STATO = "Insert";
        bMuscolo = new bool[6];
        sMuscolo = new string[6];
        for (int row = 0; row < bMuscolo.Length; row++)
        {
            bMuscolo[row] = false;
        }
        sMuscolo[0] = "Spalle";
        sMuscolo[1] = "Petto";
        sMuscolo[2] = "Schiena";
        sMuscolo[3] = "Braccia";
        sMuscolo[4] = "ABS";
        sMuscolo[5] = "Gambe";

        ifName.text = "";
        ifAdvices.text = "";
        ifNote.text = "";

        Globals.PAGE_EXER_ADD.transform.Find("btnMuscles").GetComponent<Button>().onClick.AddListener(() => btnMuscles());

        Page_InsertEsercizio(bMuscolo);
    }
    public void Init_InsertEsercizio(int ID)
    {
        Globals.INTERO = ID;
        Globals.STATO = "Update";
        var tuple = AppManager.Instance.sql.SelectAllEsercizi(" WHERE " + Globals.COL_ESE_ID + " = " + ID, Globals.COL_ESE_NOME);
        DataTable dtTable = tuple.Item1;
        List<byte[]> arrayList = tuple.Item2;
        DataTable dtEsMuscoli = AppManager.Instance.sql.SelectAllME(" WHERE me." + Globals.COL_MxE_IDESERCIZIO + " = " + ID, "me." + Globals.COL_MxE_IDMUSCULO);

        bMuscolo = new bool[6];
        sMuscolo = new string[6];
        bool YesNo;
        for (int row = 0; row < bMuscolo.Length; row++)
        {
            YesNo = false;
            for (int row2 = 0; row2 < dtEsMuscoli.Rows.Count; row2++)
            {
                if (row == int.Parse(dtEsMuscoli.Rows[row2][Globals.COL_MxE_IDMUSCULO].ToString()))
                    YesNo = true;
            }
            bMuscolo[row] = YesNo;
        }
        sMuscolo[0] = "Spalle";
        sMuscolo[1] = "Petto";
        sMuscolo[2] = "Schiena";
        sMuscolo[3] = "Braccia";
        sMuscolo[4] = "ABS";
        sMuscolo[5] = "Gambe";

        #region Immagine
        byte[] ByteImmagine = arrayList[0];
        if (ByteImmagine != null)
        {
            UploadImage uploadImage = gameObject.AddComponent<UploadImage>();
            if (!gameObject.GetComponent<Image>())
                gameObject.AddComponent<Image>();
            Image immagine = gameObject.GetComponent<Image>();
            //Ottengo l'immagine dai byte
            immagine = uploadImage.ConvertByteArrayToImage(ByteImmagine);
            //Assegno la suddetta immagine 
            iImage.sprite = immagine.sprite;
            //Gli impogno di preservare il suo aspetto in modo da non ottere l'iimagine strecciata
            iImage.preserveAspect = true;
        }
        #endregion
        string A = dtTable.Rows[0][Globals.COL_ESE_NOME].ToString();
        string B = dtTable.Rows[0][Globals.COL_ESE_CONSIGLI].ToString();
        string C = dtTable.Rows[0][Globals.COL_ESE_NOTE].ToString();
        ifName.text = dtTable.Rows[0][Globals.COL_ESE_NOME].ToString();
        ifAdvices.text = dtTable.Rows[0][Globals.COL_ESE_CONSIGLI].ToString();
        ifNote.text = dtTable.Rows[0][Globals.COL_ESE_NOTE].ToString();

        Page_InsertEsercizio(bMuscolo);
    }
    public void Page_InsertEsercizio(bool[] NewMuscolo)
    {
        bMuscolo = NewMuscolo;
        for (int row = 0; row < NewMuscolo.Length; row++)
        {
            bMuscolo[row] = NewMuscolo[row];
        }


        Globals.PAGE_EXER_ADD.SetActive(true);

        Globals.BTN_CONFIRM.onClick.RemoveAllListeners();
        Globals.BTN_CANCEL.onClick.RemoveAllListeners();
        Globals.BTN_CANCEL.onClick.AddListener(() => AppManager.Instance.navManager.NavigateBack());
        if (Globals.STATO == "Insert")
        {
            Globals.BTN_CONFIRM.onClick.AddListener(() => InsertEsercizio());
        }
        else
        {
            Globals.BTN_CONFIRM.onClick.AddListener(() => UpdateEsercizio());
        }
        string sElenco = "";
        for (int row = 0; row < bMuscolo.Length; row++)
        {
            if (bMuscolo[row] == true)
            {
                sElenco += "-" + sMuscolo[row] + "\n\r";
            }
        }

        Globals.PAGE_EXER_ADD.transform.Find("txtMuscles").GetComponent<TextMeshProUGUI>().text = sElenco;
    }


    #region Muscoli
    public void btnMuscles()
    {
        Muscles muscles = gameObject.AddComponent<Muscles>();
        muscles.Muscoli_Show(bMuscolo, "InsertEsercizio");
    }
    #endregion
    #endregion
    #region Schede
    void InsertScheda()
    {

    }
    #endregion
    #region Relazione Esercizio-Scheda
    public void InsertRelazioneES(int IDesercizio, int IDscheda)
    {
        try
        {

            string sCondition = " WHERE sc." + Globals.COL_SCH_ID + " = " + IDscheda + " ";
            var tuple = AppManager.Instance.sql.SelectAllDettagli(sCondition, "det." + Globals.COL_DET_POSIZIONE);
            DataTable dtTable = tuple.Item1;
            int Pos = 1;
            if (dtTable.Rows.Count > 0)
            {
                Pos = AppManager.Instance.sql.SelectMaxPosizione(" WHERE " + Globals.COL_DET_ID_SCHEDA + " = " + IDscheda) + 1;
            }
            AppManager.Instance.sql.InsertDettagli(3, "N", 10, "00:00", "01:00", "01:00", Pos, IDesercizio, IDscheda);
        }
        catch (Exception ex)
        {
            Debug.Log("inserimento esercizio-scheda " + ex);
        }
    }
    #endregion
}
