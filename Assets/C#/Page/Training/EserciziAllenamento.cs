using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EserciziAllenamento : MonoBehaviour
{
    #region DETTAGLI
    private void CaricaDettaglidaScheda(int ID)
    {
        Globals.INTERO = ID;
        DataTable dtTable;
        List<byte[]> arrayList = new List<byte[]>();

        SvuotaPaginaDettagliEsercizi();
        ControlloPosizioni(ID);

        string sCondition = " WHERE sc." + Globals.COL_SCH_ID + " = " + ID + " ";
        var tuple = AppManager.Instance.sql.SelectAllDettagli(sCondition, "det." + Globals.COL_DET_POSIZIONE);
        dtTable = tuple.Item1;
        arrayList = tuple.Item2;
        Button[] Duplicato = new Button[dtTable.Rows.Count];

        Globals.PAGE_ESERCIZI.gameObject.transform.Find("btnNomeScheda").GetComponentInChildren<TextMeshProUGUI>().text = dtTable.Rows[0][Globals.COL_SCH_NOME].ToString();

        if (!gameObject.GetComponent<Image>())
            gameObject.AddComponent<Image>();
        Image immagine = gameObject.GetComponent<Image>();
        Globals.BTN_ADD_ESERCIZIO_DET_ALNMT.gameObject.SetActive(true);


        for (int row = 0; row < dtTable.Rows.Count; row++)
        {
            CaricaBottone(immagine, dtTable, Duplicato, arrayList, row, Globals.BTN_ADD_ESERCIZIO_DET_ALNMT);
            RectTransform rt = GameObject.Find(Duplicato[row].gameObject.name).transform.Find("Canvas Details").GetComponent<Canvas>().GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(1, 0);
            rt.anchorMax = new Vector2(1, 0);
            rt.pivot = new Vector2(1, 0);
            rt.sizeDelta = new Vector2(500, 50);
            rt.anchoredPosition = new Vector2(0, 0);

            GameObject.Find(Duplicato[row].gameObject.name).transform.Find("Canvas Details/txtSeriesNum").GetComponent<TextMeshProUGUI>().text = dtTable.Rows[row][Globals.COL_DET_SERIE].ToString();
            GameObject.Find(Duplicato[row].gameObject.name).transform.Find("Canvas Details/txtRecNum").GetComponent<TextMeshProUGUI>().text = dtTable.Rows[row][Globals.COL_DET_RECUPERO].ToString();
            if (dtTable.Rows[row][Globals.COL_DET_RIPnOt].ToString() == "N")
            {
                GameObject.Find(Duplicato[row].gameObject.name).transform.Find("Canvas Details/txtRepsNum").GetComponent<TextMeshProUGUI>().text = dtTable.Rows[row][Globals.COL_DET_RIPETIZIONIn].ToString();
                GameObject.Find(Duplicato[row].gameObject.name).transform.Find("Canvas Details/txtReps").GetComponent<TextMeshProUGUI>().text = "Ripet.:";
            }
            else
            {
                GameObject.Find(Duplicato[row].gameObject.name).transform.Find("Canvas Details/txtRepsNum").GetComponent<TextMeshProUGUI>().text = dtTable.Rows[row][Globals.COL_DET_RIPETIZIONIt].ToString();
                GameObject.Find(Duplicato[row].gameObject.name).transform.Find("Canvas Details/txtReps").GetComponent<TextMeshProUGUI>().text = "Durata:";
            }
        }
        Globals.BTN_ADD_ESERCIZIO_DET_ALNMT.gameObject.SetActive(false);
    }
    private void CaricaBottone(Image immagine, DataTable dtTable, Button[] Duplicato, List<byte[]> arrayList, int row, Button button)
    {
        UploadImage uploadImage = gameObject.AddComponent<UploadImage>();

        #region Creazione bottoni
        //Duplica bottone
        Duplicato[row] = Instantiate(button);
        //Rinominalo
        Duplicato[row].name = "EsN" + row;
        //Assegnagli come padre "Content"
        Duplicato[row].transform.SetParent((GameObject.Find("ContentDet")).transform);
        //Assegnazione indice (posizione)
        Duplicato[row].transform.SetSiblingIndex(row + 1);
        //Cambia la stringa del testo figlio
        GameObject.Find(Duplicato[row].gameObject.name).transform.Find("Text").GetComponent<TextMeshProUGUI>().text = dtTable.Rows[row][Globals.COL_ESE_NOME].ToString();
        #endregion
        #region Immagine nel bottone
        try
        {
            //Converto in un array di byte la riga n della lista contenente gli array di byte
            byte[] ByteImmagine = arrayList[row];
            if (ByteImmagine != null)
            {
                //Ottengo l'immagine dai byte
                immagine = uploadImage.ConvertByteArrayToImage(ByteImmagine);
                //Assegno la suddetta immagine al figlio (immagine) del figlio (ImgCircle = immagine circolare per creare la maschera rotonda) del bottone
                GameObject.Find(Duplicato[row].gameObject.name).transform.Find("ImgCircle/Image").GetComponent<Image>().sprite = immagine.sprite;
                //Gli impogno di preservare il suo aspetto in modo da non ottere l'iimagine strecciata
                GameObject.Find(Duplicato[row].gameObject.name).transform.Find("ImgCircle/Image").GetComponent<Image>().preserveAspect = true;
                //Riporto l'immagine alle sue dimensioni originali (ma solo per calcolare l'altezza e la laeghezza originale)
                GameObject.Find(Duplicato[row].gameObject.name).transform.Find("ImgCircle/Image").GetComponent<Image>().SetNativeSize();
                //Ottengo i dati del "rect transform" dell'immagine
                RectTransform rt = GameObject.Find(Duplicato[row].gameObject.name).transform.Find("ImgCircle/Image").GetComponent<Image>().GetComponent<RectTransform>();
                //Prendo la larghezza e la larghezza
                double width = rt.sizeDelta.x * rt.localScale.x;
                double height = rt.sizeDelta.y * rt.localScale.y;
                //Se la larghezza è maggiore dall'altezza, imposto l'altezza a 100 pixels precisi (quelli del riquadro dell'immagine) mentre la larghezza al massimo che gli possa occorre
                if (width > height)
                {
                    GameObject.Find(Duplicato[row].gameObject.name).transform.Find("ImgCircle/Image").GetComponent<Image>().rectTransform.sizeDelta = new Vector3(4000, 100, 1);
                }
                else
                {
                    GameObject.Find(Duplicato[row].gameObject.name).transform.Find("ImgCircle/Image").GetComponent<Image>().rectTransform.sizeDelta = new Vector3(100, 4000, 1);
                }
                //Porto l'immagine nella posizione x = 0, y = 0
                GameObject.Find(Duplicato[row].gameObject.name).transform.Find("ImgCircle/Image").GetComponent<Image>().rectTransform.anchoredPosition = new Vector3(0, 0, 1);
            }
            //Se è nulla distruggo l'immagine
            else
            {
                Destroy(GameObject.Find(Duplicato[row].gameObject.name).transform.Find("ImgCircle").gameObject);
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
        #endregion
    }
    public void SvuotaPaginaDettagliEsercizi()
    {
        GameObject Parent = Globals.BTN_ADD_ESERCIZIO_DET_ALNMT.transform.parent.gameObject;
        Globals.BTN_ADD_ESERCIZIO_DET_ALNMT.gameObject.SetActive(false);
        while (Parent.transform.childCount > 1)
        {
            if (Parent.transform.GetChild(Parent.transform.childCount - 1).gameObject.activeSelf == true)
                DestroyImmediate(Parent.transform.GetChild(Parent.transform.childCount - 1).gameObject);
        }
    }
    private void ControlloPosizioni(int IDscheda)
    {

        string sCondition = " WHERE sc." + Globals.COL_SCH_ID + " = " + IDscheda + " ";
        var tuple = AppManager.Instance.sql.SelectAllDettagli(sCondition, "det." + Globals.COL_DET_POSIZIONE);
        DataTable dtTable = tuple.Item1;
        int jsd;
        for (int row = 0; row < dtTable.Rows.Count; row++)
        {
            jsd = int.Parse(dtTable.Rows[row][Globals.COL_DET_POSIZIONE].ToString());
            if (int.Parse(dtTable.Rows[row][Globals.COL_DET_POSIZIONE].ToString()) != (row + 1))
            {
                AppManager.Instance.sql.UpdateDettagli(int.Parse(dtTable.Rows[row][Globals.COL_DET_ID].ToString()), int.Parse(dtTable.Rows[row][Globals.COL_DET_SERIE].ToString()), dtTable.Rows[row][Globals.COL_DET_RIPnOt].ToString(), int.Parse(dtTable.Rows[row][Globals.COL_DET_RIPETIZIONIn].ToString()), dtTable.Rows[row][Globals.COL_DET_RIPETIZIONIt].ToString(), (row + 1), dtTable.Rows[row][Globals.COL_DET_RECUPERO].ToString(), dtTable.Rows[row][Globals.COL_DET_ATTESA].ToString(), int.Parse(dtTable.Rows[row][Globals.COL_ESE_ID].ToString()), int.Parse(dtTable.Rows[row][Globals.COL_SCH_ID].ToString()));
            }
        }
    }
    #endregion
    #region DETTAGLI EXPLOSE
    private void CaricaExplose(int ID)
    {
        DataTable dtTable;

        SvuotaPaginaExplose();

        string sCondition = " WHERE sc." + Globals.COL_SCH_ID + " = " + ID + " ";
        var tuple = AppManager.Instance.sql.SelectAllDettagli(sCondition, "det." + Globals.COL_DET_POSIZIONE);
        dtTable = tuple.Item1;
        int Num = 0;
        for (int row = 0; row < dtTable.Rows.Count; row++)
        {
            Num += int.Parse(dtTable.Rows[row][Globals.COL_DET_SERIE].ToString());
        }
        Button[] DuplicatoExp = new Button[Num + 1];
        Button[] DuplicatoRec = new Button[Num + 1];
        if (!gameObject.GetComponent<Image>())
            gameObject.AddComponent<Image>();
        Globals.BTN_ADD_ESERCIZIO_EXPL_ALNMT.gameObject.SetActive(true);
        Globals.BTN_ADD_ESERCIZIO_EXPL_REC_ALNMT.gameObject.SetActive(true);

        Num = 1;
        int NumRec = 0;
        int NumEs = 0;
        string Recupero;
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
                    CaricaBottoneRecupero(Recupero, DuplicatoRec, row, Globals.BTN_ADD_ESERCIZIO_EXPL_REC_ALNMT, Num, NumRec);
                    Num += 1;
                    NumRec += 1;
                }
                CaricaBottoneExplose(dtTable, DuplicatoExp, row, Globals.BTN_ADD_ESERCIZIO_EXPL_ALNMT, Num, NumEs);
                Num += 1;
                NumEs += 1;
            }
        }
        Globals.BTN_ADD_ESERCIZIO_EXPL_ALNMT.gameObject.SetActive(false);
        Globals.BTN_ADD_ESERCIZIO_EXPL_REC_ALNMT.gameObject.SetActive(false);
    }
    private void CaricaBottoneExplose(DataTable dtTable, Button[] Duplicato, int row, Button button, int Index, int NumBtn)
    {
        //Duplica bottone
        Duplicato[NumBtn] = Instantiate(button);
        //Rinominalo
        Duplicato[NumBtn].name = "ExpN" + Index;
        //Assegnagli come padre "Content"
        GameObject Parent = Globals.BTN_ADD_ESERCIZIO_EXPL_ALNMT.transform.parent.gameObject;
        Duplicato[NumBtn].transform.SetParent(Parent.gameObject.transform);
        //Assegnazione indice (posizione)
        Duplicato[NumBtn].transform.SetSiblingIndex(Index + 1);
        //Cambia la stringa del testo figlio
        GameObject.Find(Duplicato[NumBtn].gameObject.name).transform.Find("Pos").GetComponent<TextMeshProUGUI>().text = (NumBtn + 1).ToString();
        GameObject.Find(Duplicato[NumBtn].gameObject.name).transform.Find("Nome").GetComponent<TextMeshProUGUI>().text = dtTable.Rows[row][Globals.COL_ESE_NOME].ToString();
        GameObject.Find(Duplicato[NumBtn].gameObject.name).transform.Find("RipetizioniN").GetComponent<TextMeshProUGUI>().text = "X " + dtTable.Rows[row][Globals.COL_DET_RIPETIZIONIn].ToString();
        GameObject.Find(Duplicato[NumBtn].gameObject.name).transform.Find("RipetizioniT").GetComponent<TextMeshProUGUI>().text = "X " + dtTable.Rows[row][Globals.COL_DET_RIPETIZIONIt].ToString();
        if (dtTable.Rows[row][Globals.COL_DET_RIPnOt].ToString() == "N")
        {
            GameObject.Find(Duplicato[NumBtn].gameObject.name).transform.Find("RipetizioniN").GetComponent<TextMeshProUGUI>().gameObject.SetActive(true);
            GameObject.Find(Duplicato[NumBtn].gameObject.name).transform.Find("RipetizioniT").GetComponent<TextMeshProUGUI>().gameObject.SetActive(false);
        }
        else
        {
            GameObject.Find(Duplicato[NumBtn].gameObject.name).transform.Find("RipetizioniN").GetComponent<TextMeshProUGUI>().gameObject.SetActive(false);
            GameObject.Find(Duplicato[NumBtn].gameObject.name).transform.Find("RipetizioniT").GetComponent<TextMeshProUGUI>().gameObject.SetActive(true);
        }
    }
    private void CaricaBottoneRecupero(string Tempo, Button[] Duplicato, int row, Button button, int Index, int NumBtn)
    {
        //Duplica bottone
        Duplicato[NumBtn] = Instantiate(button);
        //Rinominalo
        Duplicato[NumBtn].name = "ExpN" + Index;
        //Assegnagli come padre "Content"
        GameObject Parent = button.transform.parent.gameObject;
        Duplicato[NumBtn].transform.SetParent(Parent.gameObject.transform);
        //Assegnazione indice (posizione)
        Duplicato[NumBtn].transform.SetSiblingIndex(Index + 1);
        //Cambia la stringa del testo figlio
        GameObject.Find(Duplicato[NumBtn].gameObject.name).transform.Find("Tempo").GetComponent<TextMeshProUGUI>().text = Tempo;
    }
    private void SvuotaPaginaExplose()
    {
        GameObject Parent = Globals.BTN_ADD_ESERCIZIO_EXPL_ALNMT.transform.parent.gameObject;
        Globals.BTN_ADD_ESERCIZIO_EXPL_ALNMT.gameObject.SetActive(false);
        while (Parent.transform.childCount > 2)
        {
            DestroyImmediate(Parent.transform.GetChild(Parent.transform.childCount - 1).gameObject);
        }
    }
    #endregion
    public void SwitchView()
    {
        if (Globals.PAGE_ESERCIZI.gameObject.transform.Find("ExploseView").GetComponent<Canvas>().gameObject.activeSelf)
        {
            Globals.PAGE_ESERCIZI.gameObject.transform.Find("ExploseView").GetComponent<Canvas>().gameObject.SetActive(false);
            Globals.PAGE_ESERCIZI.gameObject.transform.Find("DetailsView").GetComponent<Canvas>().gameObject.SetActive(true);
            CaricaDettaglidaScheda(Globals.INTERO);
        }
        else
        {
            Globals.PAGE_ESERCIZI.gameObject.transform.Find("ExploseView").GetComponent<Canvas>().gameObject.SetActive(true);
            Globals.PAGE_ESERCIZI.gameObject.transform.Find("DetailsView").GetComponent<Canvas>().gameObject.SetActive(false);
        }
    }
    public void InitEs(int ID)
    {
        AppManager.Instance.objects.PagesTraining("Esercizi");
        Globals.PAGE_ESERCIZI.gameObject.transform.Find("DetailsView").GetComponent<Canvas>().gameObject.SetActive(true);
        Globals.PAGE_ESERCIZI.gameObject.transform.Find("ExploseView").GetComponent<Canvas>().gameObject.SetActive(true);
        CaricaDettaglidaScheda(ID);
        CaricaExplose(ID);
        SwitchView();

        Globals.PAGE_ESERCIZI.gameObject.transform.Find("btnInizia").GetComponent<Button>().onClick.RemoveAllListeners();
        Globals.PAGE_ESERCIZI.gameObject.transform.Find("btnInizia").GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.allenamento.InitAllenamento(ID));
    }
}
