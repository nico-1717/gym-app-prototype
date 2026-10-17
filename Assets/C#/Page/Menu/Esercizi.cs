using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Esercizi : MonoBehaviour
{
    #region ESERCIZI
    public void CaricaEsercizi()
    {
        DataTable dtTable;
        List<byte[]> arrayList = new List<byte[]>();

        List<Transform> tChildren = new List<Transform>();

        SvuotaPaginaEsercizi();

        var tuple = AppManager.Instance.sql.SelectAllEsercizi("", Globals.COL_ESE_NOME);
        dtTable = tuple.Item1;
        arrayList = tuple.Item2;
        DataTable dtMuscoli;
        Button[] Duplicato = new Button[dtTable.Rows.Count];
        if (!gameObject.GetComponent<Image>())
            gameObject.AddComponent<Image>();
        Image immagine = gameObject.GetComponent<Image>();

        Globals.BTN_ADD_ESERCIZIO.transform.Find("txtSelect").GetComponent<TextMeshProUGUI>().gameObject.SetActive(false);
        Globals.BTN_ADD_ESERCIZIO.transform.Find("Canvas Button").GetComponent<Canvas>().gameObject.SetActive(false);
        for (int row = 0; row < dtTable.Rows.Count; row++)
        {
            string sMuscoli = "";
            int iMuscoli = 0;
            dtMuscoli = AppManager.Instance.sql.SelectAllME(" WHERE me." + Globals.COL_MxE_IDESERCIZIO + " = " + int.Parse(dtTable.Rows[row][Globals.COL_ESE_ID].ToString()), "mu." + Globals.COL_MUS_NOME);
            while (iMuscoli < dtMuscoli.Rows.Count)
            {
                if (iMuscoli != 0)
                    sMuscoli += ", ";
                sMuscoli += dtMuscoli.Rows[iMuscoli][Globals.COL_MUS_NOME].ToString();
                iMuscoli += 1;
            }

            CaricaBottone(immagine, dtTable, Duplicato, arrayList, row, Globals.BTN_ADD_ESERCIZIO, sMuscoli);

            int IDesercizio = int.Parse(dtTable.Rows[row][Globals.COL_ESE_ID].ToString());
            Duplicato[row].onClick.RemoveAllListeners();
            Duplicato[row].onClick.AddListener(() => AppManager.Instance.navManager.NavigateTo(() => ClickEsercizio(IDesercizio)));

            tChildren.Add(Duplicato[row].transform);
        }
        Globals.BTN_ADD_ESERCIZIO.onClick.RemoveAllListeners();
        Globals.BTN_ADD_ESERCIZIO.onClick.AddListener(() => AppManager.Instance.navManager.NavigateTo(() => AppManager.Instance.objects.ExercisesSwitchState("Add")));

        //Button press
        for (int i = 0; i < Globals.BTN_ADD_ESERCIZIO.transform.parent.childCount; i++)
        {
            ButtonPressEvent bpe = Globals.BTN_ADD_ESERCIZIO.transform.parent.transform.GetChild(i).gameObject.AddComponent<ButtonPressEvent>();
            bpe.onPressed.AddListener(() => AppManager.Instance.shakeObject.Shake(tChildren));
            bpe.onPressed.AddListener(ModificaEsercizi);
        }
    }
    private void ClickEsercizio(int IDesercizio)
    {
        AppManager.Instance.objects.TabsSwitchState("View&C");
        AppManager.Instance.schede.CaricaSchedeEsercizio(IDesercizio);
    }
    private void ModificaEsercizi()
    {
        GameObject Parent = Globals.BTN_ADD_ESERCIZIO.transform.parent.gameObject;
        Parent.GetComponent<VerticalLayoutGroup>().spacing = 20;
        Parent.GetComponent<VerticalLayoutGroup>().padding.top = 30;

        //per prendere la lista di child
        List<Transform> tChildren = new List<Transform>();
        for (int i = 0; i < Parent.transform.childCount; i++)
        {
            Transform child = Parent.transform.GetChild(i);
            tChildren.Add(child.transform);
        }

        Globals.BTN_ADD_ESERCIZIO.gameObject.SetActive(false);
        for (int i = 0; i < Parent.transform.childCount; i++)
        {
            Parent.transform.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
            int ChildNum = i;
            Parent.transform.GetChild(ChildNum).transform.Find("CanvasBtnEditDel").GetComponent<Canvas>().gameObject.SetActive(true);
            Parent.transform.GetChild(ChildNum).transform.Find("CanvasBtnEditDel/btnDelete").GetComponent<Button>().onClick.RemoveAllListeners();
            Parent.transform.GetChild(ChildNum).transform.Find("CanvasBtnEditDel/btnDelete").GetComponent<Button>().onClick.AddListener(() => btn_DeleteEsercizi(ChildNum - 1));
            Parent.transform.GetChild(ChildNum).transform.Find("CanvasBtnEditDel/btnUpdate").GetComponent<Button>().onClick.RemoveAllListeners();
            Parent.transform.GetChild(ChildNum).transform.Find("CanvasBtnEditDel/btnUpdate").GetComponent<Button>().onClick.AddListener(() => btn_UpdateEsercizi(ChildNum - 1));
            //Button press
            ButtonPressEvent bpe = Parent.transform.GetChild(ChildNum).gameObject.AddComponent<ButtonPressEvent>();
            bpe.onPressed.AddListener(AppManager.Instance.shakeObject.StopAllShakes);
            bpe.onPressed.AddListener(CaricaEsercizi);
        }
    }
    private void btn_DeleteEsercizi(int Num)
    {
        bool Deleted = true;
        var tuple = AppManager.Instance.sql.SelectAllEsercizi("", Globals.COL_ESE_NOME);
        DataTable dtTable = tuple.Item1;
        int ID = int.Parse(dtTable.Rows[Num][Globals.COL_ESE_ID].ToString());
        DataTable dtMxEDel = AppManager.Instance.sql.SelectAllME(" WHERE me." + Globals.COL_MxE_IDESERCIZIO + " = " + ID, "me." + Globals.COL_MxE_ID);
        for (int row = 0; row < dtMxEDel.Rows.Count; row++)
        {
            if (AppManager.Instance.sql.DeleteME(int.Parse(dtMxEDel.Rows[row][Globals.COL_MxE_ID].ToString())) == false)
                Deleted = false;
        }
        tuple = AppManager.Instance.sql.SelectAllDettagli(" WHERE det." + Globals.COL_DET_ID_ESERCIZIO + " = " + ID, Globals.COL_DET_ID);
        DataTable dtDelDet = tuple.Item1;
        for (int row = 0; row < dtDelDet.Rows.Count; row++)
        {
            if (DeleteDettagli(int.Parse(dtDelDet.Rows[row][Globals.COL_DET_ID].ToString())) == false)
                Deleted = false;
        }
        if (Deleted == true)
            if (AppManager.Instance.sql.DeleteEsercizio(ID) == false)
                Deleted = false;
        if (Deleted == true)
        {
            AppManager.Instance.navManager.NavigateTo(CaricaEsercizi);
        }
    }
    private void btn_UpdateEsercizi(int Num)
    {
        var tuple = AppManager.Instance.sql.SelectAllEsercizi("", Globals.COL_ESE_NOME);
        DataTable dtTable = tuple.Item1;
        int ID = int.Parse(dtTable.Rows[Num][Globals.COL_ESE_ID].ToString());
        Insert insert = gameObject.AddComponent<Insert>();
        AppManager.Instance.navManager.NavigateTo(() => AppManager.Instance.objects.ExercisesSwitchState("Modify"));
        insert.Init_InsertEsercizio(ID);
    }
    #region Seleziona esercizi
    //0 = no
    //1 = yes
    //2 = alredy selected
    static int[,] iEsSelected;
    static Button[] Duplicato;

    public void SelezionaEsercizi(int IDscheda)
    {
        Globals.INTERO = IDscheda;

        DataTable dtTable;
        List<byte[]> arrayList = new List<byte[]>();
        Button btnAddEsercizio = Globals.PAGE_EXER_VIEW.transform.Find("Scroll View/Viewport/Content/Aggiungi").GetComponent<Button>();

        AppManager.Instance.objects.ExercisesSwitchState("View");
        AppManager.Instance.objects.Button_Area("Answer");
        Globals.BTN_CONFIRM.onClick.RemoveAllListeners();
        Globals.BTN_CONFIRM.onClick.AddListener(() => InserisciRelazioneSchedaEsercizi(Globals.INTERO));
        Globals.BTN_CANCEL.onClick.RemoveAllListeners();
        Globals.BTN_CANCEL.onClick.AddListener(() => AppManager.Instance.navManager.NavigateBack());

        SvuotaPaginaEsercizi();

        var tuple = AppManager.Instance.sql.SelectAllEsercizi("", Globals.COL_ESE_NOME);
        dtTable = tuple.Item1;
        arrayList = tuple.Item2;
        Duplicato = new Button[dtTable.Rows.Count];

        if (!gameObject.GetComponent<Image>())
            gameObject.AddComponent<Image>();
        Image immagine = gameObject.GetComponent<Image>();

        iEsSelected = new int[dtTable.Rows.Count, 3];
        Array.Clear(iEsSelected, 0, iEsSelected.Length);

        tuple = AppManager.Instance.sql.SelectAllDettagli(" WHERE det." + Globals.COL_DET_ID_SCHEDA + " = " + Globals.INTERO, Globals.COL_ESE_ID);
        DataTable dtGiaCollegati = tuple.Item1;
        DataTable dtMuscoli;

        btnAddEsercizio.transform.Find("txtSelect").gameObject.SetActive(false);
        btnAddEsercizio.transform.Find("Canvas Button").gameObject.SetActive(false);
        for (int row = 0; row < dtTable.Rows.Count; row++)
        {
            string sMuscoli = "";
            int iMuscoli = 0;
            dtMuscoli = AppManager.Instance.sql.SelectAllME(" WHERE me." + Globals.COL_MxE_IDESERCIZIO + " = " + int.Parse(dtTable.Rows[row][Globals.COL_ESE_ID].ToString()), "mu." + Globals.COL_MUS_NOME);
            while (iMuscoli < dtMuscoli.Rows.Count)
            {
                if (iMuscoli != 0)
                    sMuscoli += ", ";
                sMuscoli += dtMuscoli.Rows[iMuscoli][Globals.COL_MUS_NOME].ToString();
                iMuscoli += 1;
            }
            CaricaBottone(immagine, dtTable, Duplicato, arrayList, row, btnAddEsercizio, sMuscoli);

            iEsSelected[row, 0] = int.Parse(dtTable.Rows[row][Globals.COL_ESE_ID].ToString());
            iEsSelected[row, 1] = 0;
            iEsSelected[row, 2] = 0;
            int iRiga = row;

            Duplicato[row].onClick.RemoveAllListeners();
            Duplicato[row].onClick.AddListener(() => MoreSelect(iRiga));

            for (int riga = 0; riga < dtGiaCollegati.Rows.Count; riga++)
            {
                if (int.Parse(dtTable.Rows[row][Globals.COL_ESE_ID].ToString()) == int.Parse(dtGiaCollegati.Rows[riga][Globals.COL_ESE_ID].ToString()))
                {
                    iEsSelected[row, 2] = 1;
                    //color
                    ColorBlock theColor;
                    theColor = Duplicato[row].colors;
                    theColor.normalColor = Color.yellow;
                    Duplicato[row].colors = theColor;
                    break;
                }
            }
        }
        btnAddEsercizio.gameObject.SetActive(false);
    }
    private void MoreSelect(int row)
    {
        if (iEsSelected[row, 1] < 9)
        {
            if (iEsSelected[row, 1] == 0)
            {
                SwitchSelectEsercizio(row);
                Duplicato[row].transform.Find("txtSelect").GetComponent<TextMeshProUGUI>().text = "x1";
            }
            else
            {
                iEsSelected[row, 1] += 1;
                Duplicato[row].transform.Find("txtSelect").GetComponent<TextMeshProUGUI>().text = "x" + iEsSelected[row, 1];
            }
        }
    }
    private void LessSelect(int row)
    {
        if (iEsSelected[row, 1] == 1)
        {
            SwitchSelectEsercizio(row);
        }
        else
        {
            iEsSelected[row, 1] -= 1;
            Duplicato[row].transform.Find("txtSelect").GetComponent<TextMeshProUGUI>().text = "x" + iEsSelected[row, 1];
        }
    }
    private void SwitchSelectEsercizio(int row)
    {
        if (iEsSelected[row, 1] == 0)
        {
            iEsSelected[row, 1] = 1;

            //color
            ColorBlock theColor;
            theColor = Duplicato[row].colors;
            theColor.normalColor = Color.green;
            theColor.selectedColor = Color.green;
            theColor.highlightedColor = Color.green;
            Duplicato[row].colors = theColor;

            Duplicato[row].transform.Find("txtSelect").GetComponent<TextMeshProUGUI>().gameObject.SetActive(true);
            Button btnDelSelect = Duplicato[row].transform.Find("Canvas Button/btnDelSelect").GetComponent<Button>();
            Duplicato[row].transform.Find("Canvas Button").GetComponent<Canvas>().gameObject.SetActive(true);

            btnDelSelect.onClick.RemoveAllListeners();
            btnDelSelect.onClick.AddListener(() => LessSelect(row));
        }
        else
        {
            iEsSelected[row, 1] = 0;

            if (iEsSelected[row, 2] == 1)
            {
                //color
                ColorBlock theColor;
                theColor = Duplicato[row].colors;
                theColor.normalColor = Color.yellow;
                theColor.selectedColor = Color.yellow;
                theColor.highlightedColor = Color.yellow;
                Duplicato[row].colors = theColor;
            }
            else
            {
                //color
                ColorBlock theColor;
                theColor = Duplicato[row].colors;
                theColor.normalColor = Color.white;
                theColor.selectedColor = Color.white;
                theColor.highlightedColor = Color.white;
                Duplicato[row].colors = theColor;
            }
            Duplicato[row].transform.Find("txtSelect").GetComponent<TextMeshProUGUI>().gameObject.SetActive(false);
            Duplicato[row].transform.Find("Canvas Button").GetComponent<Canvas>().gameObject.SetActive(false);
        }
    }

    private void InserisciRelazioneSchedaEsercizi(int IDscheda)
    {
        Insert insert = gameObject.AddComponent<Insert>();

        for (int row = 0; row < iEsSelected.GetLength(0); row++)
        {
            int Inserted = 0;
            while (Inserted < iEsSelected[row, 1])
            {
                insert.InsertRelazioneES(iEsSelected[row, 0], IDscheda);
                Inserted += 1;
            }
        }
        AppManager.Instance.navManager.NavigateBack();
        ModificaDettagli();
    }
    public void SvuotaPaginaEsercizi()
    {
        Globals.BTN_ADD_ESERCIZIO.GetComponent<Button>().gameObject.SetActive(true);

        GameObject Parent = Globals.BTN_ADD_ESERCIZIO.transform.parent.gameObject;
        while (Parent.transform.childCount > 1)
        {
            DestroyImmediate(Parent.transform.GetChild(Parent.transform.childCount - 1).gameObject);
        }
        Globals.BTN_ADD_ESERCIZIO.GetComponent<Button>().transform.Find("CanvasBtnEditDel").GetComponent<Canvas>().gameObject.SetActive(false);
        Parent.GetComponent<VerticalLayoutGroup>().spacing = 0;
        Parent.GetComponent<VerticalLayoutGroup>().padding.top = 0;
    }
    #endregion
    #endregion
    #region DETTAGLI
    public void CaricaDettaglidaScheda(int ID)
    {
        Globals.INTERO = ID;
        DataTable dtTable;
        List<byte[]> arrayList = new List<byte[]>();

        Button btnAddEsercizioDet = Globals.PAGE_EXER_VIEW_DET.transform.Find("Scroll View/Viewport/Content/Aggiungi").GetComponent<Button>();

        SvuotaPaginaDettagliEsercizi();
        ControlloPosizioni(ID);

        string sCondition = " WHERE sc." + Globals.COL_SCH_ID + " = " + ID + " ";
        var tuple = AppManager.Instance.sql.SelectAllDettagli(sCondition, "det." + Globals.COL_DET_POSIZIONE);
        dtTable = tuple.Item1;
        arrayList = tuple.Item2;
        Button[] Duplicato = new Button[dtTable.Rows.Count];

        if (!gameObject.GetComponent<Image>())
            gameObject.AddComponent<Image>();
        Image immagine = gameObject.GetComponent<Image>();
        btnAddEsercizioDet.gameObject.transform.Find("Canvas Details").gameObject.SetActive(true);

        //per prendere la lista di child
        List<Transform> tChildren = new List<Transform>();
        GameObject parent = btnAddEsercizioDet.transform.parent.gameObject;
        parent.GetComponent<VerticalLayoutGroup>().spacing = 0;
        parent.GetComponent<VerticalLayoutGroup>().padding.top = 0;
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            Transform child = parent.transform.GetChild(i);
            tChildren.Add(child.transform);
        }

        for (int row = 0; row < dtTable.Rows.Count; row++)
        {
            CaricaBottone(immagine, dtTable, Duplicato, arrayList, row, btnAddEsercizioDet, "");
            //canvas details
            RectTransform rt = GameObject.Find(Duplicato[row].gameObject.name).transform.Find("Canvas Details").GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(1, 0);
            rt.anchorMax = new Vector2(1, 0);
            rt.pivot = new Vector2(1, 0);
            rt.sizeDelta = new Vector2(500, 50);
            rt.anchoredPosition = new Vector2(0, 0);
            //dettagli
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
            //Button press
            ButtonPressEvent bpe = Duplicato[row].gameObject.AddComponent<ButtonPressEvent>();
            bpe.onPressed.AddListener(() => AppManager.Instance.shakeObject.Shake(tChildren));
            bpe.onPressed.AddListener(ModificaDettagli);
        }
        btnAddEsercizioDet.gameObject.transform.Find("Canvas Details").GetComponent<Canvas>().gameObject.SetActive(false);
        RectTransform butrt = btnAddEsercizioDet.gameObject.transform.Find("Text").GetComponent<RectTransform>();
        butrt.anchorMin = new Vector2(0, 0.5f);
        butrt.anchorMax = new Vector2(0, 0.5f);
        butrt.pivot = new Vector2(0, 0.5f);
        butrt.sizeDelta = new Vector2(butrt.sizeDelta.x, 150);
        butrt.anchoredPosition = new Vector2(butrt.anchoredPosition.x, 0);
        btnAddEsercizioDet.gameObject.transform.Find("Text").GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.MidlineLeft;
        btnAddEsercizioDet.GetComponent<Button>().onClick.RemoveAllListeners();
        btnAddEsercizioDet.GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.navManager.NavigateTo(() => SelezionaEsercizi(ID)));
    }
    public void SvuotaPaginaDettagliEsercizi()
    {
        GameObject Parent = Globals.BTN_ADD_ESERCIZIO_DET.transform.parent.gameObject;
        Globals.BTN_ADD_ESERCIZIO_DET.gameObject.SetActive(false);
        while (Parent.transform.childCount > 1)
        {
            if (Parent.transform.GetChild(Parent.transform.childCount - 1).gameObject.activeSelf == true)
                DestroyImmediate(Parent.transform.GetChild(Parent.transform.childCount - 1).gameObject);
        }
        Globals.BTN_ADD_ESERCIZIO_DET.gameObject.SetActive(true);
        Globals.BTN_ADD_ESERCIZIO_DET.transform.Find("Canvas Button").GetComponent<Canvas>().gameObject.SetActive(false);
        Parent.GetComponent<VerticalLayoutGroup>().spacing = 0;
        Parent.GetComponent<VerticalLayoutGroup>().padding.top = 0;
    }
    private void ModificaDettagli()
    {
        GameObject Parent = Globals.BTN_ADD_ESERCIZIO_DET.transform.parent.gameObject;
        Parent.GetComponent<VerticalLayoutGroup>().spacing = 20;
        Parent.GetComponent<VerticalLayoutGroup>().padding.top = 30;
        Globals.BTN_ADD_ESERCIZIO_DET.gameObject.SetActive(false);

        //per prendere la lista di child
        List<Transform> tChildren = new List<Transform>();
        GameObject parent = Globals.BTN_ADD_ESERCIZIO_DET.transform.parent.gameObject;
        parent.GetComponent<VerticalLayoutGroup>().spacing = 0;
        parent.GetComponent<VerticalLayoutGroup>().padding.top = 0;
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            Transform child = parent.transform.GetChild(i);
            tChildren.Add(child.transform);
        }

        for (int i = 0; i < Parent.transform.childCount; i++)
        {
            int ChildNum = i;
            Parent.transform.GetChild(ChildNum).Find("Canvas Button").GetComponent<Canvas>().gameObject.SetActive(true);
            Parent.transform.GetChild(ChildNum).Find("Canvas Button/btnDelete").GetComponent<Button>().onClick.RemoveAllListeners();
            Parent.transform.GetChild(ChildNum).Find("Canvas Button/btnDelete").GetComponent<Button>().onClick.AddListener(() => btn_DeleteDettagli(ChildNum));
            Parent.transform.GetChild(ChildNum).Find("Canvas Button/btnUpdate").GetComponent<Button>().onClick.RemoveAllListeners();
            Parent.transform.GetChild(ChildNum).Find("Canvas Button/btnUpdate").GetComponent<Button>().onClick.AddListener(() => btn_EditDettagli(ChildNum));
            Parent.transform.GetChild(ChildNum).Find("Canvas Button/btnDown").GetComponent<Button>().onClick.RemoveAllListeners();
            Parent.transform.GetChild(ChildNum).Find("Canvas Button/btnDown").GetComponent<Button>().onClick.AddListener(() => btn_PosDownDettagli(ChildNum));
            Parent.transform.GetChild(ChildNum).Find("Canvas Button/btnUp").GetComponent<Button>().onClick.RemoveAllListeners();
            Parent.transform.GetChild(ChildNum).Find("Canvas Button/btnUp").GetComponent<Button>().onClick.AddListener(() => btn_PosUpDettagli(ChildNum));
            //Button press
            ButtonPressEvent bpe = Parent.transform.GetChild(ChildNum).gameObject.AddComponent<ButtonPressEvent>();
            bpe.onPressed.AddListener(AppManager.Instance.shakeObject.StopAllShakes);
            bpe.onPressed.AddListener(() => CaricaDettaglidaScheda(Globals.INTERO));
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
    #region Button Canvas
    private void btn_DeleteDettagli(int Pos)
    {
        bool Deleted = false;
        var tuple = AppManager.Instance.sql.SelectAllDettagli(" WHERE det." + Globals.COL_DET_ID_SCHEDA + " = " + Globals.INTERO + " AND det." + Globals.COL_DET_POSIZIONE + " = " + Pos, "det." + Globals.COL_DET_ID);
        DataTable dtDelDet = tuple.Item1;
        if (dtDelDet.Rows.Count == 1)
        {
            int ID = int.Parse(dtDelDet.Rows[0][Globals.COL_DET_ID].ToString());
            Deleted = DeleteDettagli(ID);
        }
        if (Deleted == true)
        {
            CaricaDettaglidaScheda(Globals.INTERO);
        }
    }
    private bool DeleteDettagli(int ID)
    {
        var tuple = AppManager.Instance.sql.SelectAllDettagli(" WHERE det." + Globals.COL_DET_ID + " = " + ID, "det." + Globals.COL_DET_ID);
        DataTable dtDelDet = tuple.Item1;
        tuple = AppManager.Instance.sql.SelectAllDettagli(" WHERE det." + Globals.COL_DET_ID_SCHEDA + " = " + Globals.INTERO + " AND det." + Globals.COL_DET_POSIZIONE + " > " + int.Parse(dtDelDet.Rows[0][Globals.COL_DET_POSIZIONE].ToString()), "det." + Globals.COL_DET_POSIZIONE);
        DataTable dtEditPos = tuple.Item1;
        bool Error = false;
        DateTime RipT;
        DateTime Recupero;
        DateTime Attesa;
        for (int row = 0; row < dtEditPos.Rows.Count; row++)
        {
            DateTime.TryParse(dtEditPos.Rows[row][Globals.COL_DET_RIPETIZIONIt].ToString(), out RipT);
            DateTime.TryParse(dtEditPos.Rows[row][Globals.COL_DET_RECUPERO].ToString(), out Recupero);
            DateTime.TryParse(dtEditPos.Rows[row][Globals.COL_DET_ATTESA].ToString(), out Attesa);
            if (AppManager.Instance.sql.UpdateDettagli(int.Parse(dtEditPos.Rows[row][Globals.COL_DET_ID].ToString()), int.Parse(dtEditPos.Rows[row][Globals.COL_DET_SERIE].ToString()), dtEditPos.Rows[row][Globals.COL_DET_RIPnOt].ToString(), int.Parse(dtEditPos.Rows[row][Globals.COL_DET_RIPETIZIONIn].ToString()), RipT.ToString("mm:ss"), int.Parse(dtEditPos.Rows[row][Globals.COL_DET_POSIZIONE].ToString()) - 1, Recupero.ToString("mm:ss"), Attesa.ToString("mm:ss"), int.Parse(dtEditPos.Rows[row][Globals.COL_ESE_ID].ToString()), int.Parse(dtEditPos.Rows[row][Globals.COL_SCH_ID].ToString())) == false)
                Error = true;
        }
        if (Error == false)
            if (AppManager.Instance.sql.DeleteDettagli(ID) == true)
                return true;
        return false;
    }
    private void btn_PosDownDettagli(int ChildNum)
    {
        var tuple = AppManager.Instance.sql.SelectAllDettagli(" WHERE det." + Globals.COL_DET_ID_SCHEDA + " = " + Globals.INTERO + " AND det." + Globals.COL_DET_POSIZIONE + " = " + ChildNum, "det." + Globals.COL_DET_ID);
        DataTable dtEditDet = tuple.Item1;
        if (dtEditDet.Rows.Count == 1)
        {
            int ID = int.Parse(dtEditDet.Rows[0][Globals.COL_DET_ID].ToString());

            tuple = AppManager.Instance.sql.SelectAllDettagli(" WHERE det." + Globals.COL_DET_ID + " = " + ID, "det." + Globals.COL_DET_ID);
            DataTable dtDet = tuple.Item1;
            int Pos = int.Parse(dtDet.Rows[0][Globals.COL_DET_POSIZIONE].ToString());

            tuple = AppManager.Instance.sql.SelectAllDettagli(" WHERE det." + Globals.COL_DET_ID_SCHEDA + " = " + Globals.INTERO + " AND det." + Globals.COL_DET_POSIZIONE + " = " + (Pos + 1), "det." + Globals.COL_DET_POSIZIONE);
            DataTable dtEditPos = tuple.Item1;
            if (dtEditPos.Rows.Count == 1)
            {
                if (AppManager.Instance.sql.UpdateDettagli(int.Parse(dtEditPos.Rows[0][Globals.COL_DET_ID].ToString()), int.Parse(dtEditPos.Rows[0][Globals.COL_DET_SERIE].ToString()), dtEditPos.Rows[0][Globals.COL_DET_RIPnOt].ToString(), int.Parse(dtEditPos.Rows[0][Globals.COL_DET_RIPETIZIONIn].ToString()), dtEditPos.Rows[0][Globals.COL_DET_RIPETIZIONIt].ToString(), int.Parse(dtEditPos.Rows[0][Globals.COL_DET_POSIZIONE].ToString()) - 1, dtEditPos.Rows[0][Globals.COL_DET_RECUPERO].ToString(), dtEditPos.Rows[0][Globals.COL_DET_ATTESA].ToString(), int.Parse(dtEditPos.Rows[0][Globals.COL_ESE_ID].ToString()), int.Parse(dtEditPos.Rows[0][Globals.COL_SCH_ID].ToString())))
                    if (AppManager.Instance.sql.UpdateDettagli(int.Parse(dtDet.Rows[0][Globals.COL_DET_ID].ToString()), int.Parse(dtDet.Rows[0][Globals.COL_DET_SERIE].ToString()), dtDet.Rows[0][Globals.COL_DET_RIPnOt].ToString(), int.Parse(dtDet.Rows[0][Globals.COL_DET_RIPETIZIONIn].ToString()), dtDet.Rows[0][Globals.COL_DET_RIPETIZIONIt].ToString(), int.Parse(dtDet.Rows[0][Globals.COL_DET_POSIZIONE].ToString()) + 1, dtDet.Rows[0][Globals.COL_DET_RECUPERO].ToString(), dtDet.Rows[0][Globals.COL_DET_ATTESA].ToString(), int.Parse(dtDet.Rows[0][Globals.COL_ESE_ID].ToString()), int.Parse(dtDet.Rows[0][Globals.COL_SCH_ID].ToString())))
                    {
                        CaricaDettaglidaScheda(Globals.INTERO);
                        ModificaDettagli();
                    }
            }
        }
    }
    private void btn_PosUpDettagli(int ChildNum)
    {
        var tuple = AppManager.Instance.sql.SelectAllDettagli(" WHERE det." + Globals.COL_DET_ID_SCHEDA + " = " + Globals.INTERO + " AND det." + Globals.COL_DET_POSIZIONE + " = " + ChildNum, "det." + Globals.COL_DET_ID);
        DataTable dtEditDet = tuple.Item1;
        if (dtEditDet.Rows.Count == 1)
        {
            int ID = int.Parse(dtEditDet.Rows[0][Globals.COL_DET_ID].ToString());

            tuple = AppManager.Instance.sql.SelectAllDettagli(" WHERE det." + Globals.COL_DET_ID + " = " + ID, "det." + Globals.COL_DET_ID);
            DataTable dtDet = tuple.Item1;
            int Pos = int.Parse(dtDet.Rows[0][Globals.COL_DET_POSIZIONE].ToString());

            tuple = AppManager.Instance.sql.SelectAllDettagli(" WHERE det." + Globals.COL_DET_ID_SCHEDA + " = " + Globals.INTERO + " AND det." + Globals.COL_DET_POSIZIONE + " = " + (Pos - 1), "det." + Globals.COL_DET_POSIZIONE);
            DataTable dtEditPos = tuple.Item1;
            if (dtEditPos.Rows.Count == 1)
            {
                if (AppManager.Instance.sql.UpdateDettagli(int.Parse(dtEditPos.Rows[0][Globals.COL_DET_ID].ToString()), int.Parse(dtEditPos.Rows[0][Globals.COL_DET_SERIE].ToString()), dtEditPos.Rows[0][Globals.COL_DET_RIPnOt].ToString(), int.Parse(dtEditPos.Rows[0][Globals.COL_DET_RIPETIZIONIn].ToString()), dtEditPos.Rows[0][Globals.COL_DET_RIPETIZIONIt].ToString(), int.Parse(dtEditPos.Rows[0][Globals.COL_DET_POSIZIONE].ToString()) + 1, dtEditPos.Rows[0][Globals.COL_DET_RECUPERO].ToString(), dtEditPos.Rows[0][Globals.COL_DET_ATTESA].ToString(), int.Parse(dtEditPos.Rows[0][Globals.COL_ESE_ID].ToString()), int.Parse(dtEditPos.Rows[0][Globals.COL_SCH_ID].ToString())))
                    if (AppManager.Instance.sql.UpdateDettagli(int.Parse(dtDet.Rows[0][Globals.COL_DET_ID].ToString()), int.Parse(dtDet.Rows[0][Globals.COL_DET_SERIE].ToString()), dtDet.Rows[0][Globals.COL_DET_RIPnOt].ToString(), int.Parse(dtDet.Rows[0][Globals.COL_DET_RIPETIZIONIn].ToString()), dtDet.Rows[0][Globals.COL_DET_RIPETIZIONIt].ToString(), int.Parse(dtDet.Rows[0][Globals.COL_DET_POSIZIONE].ToString()) - 1, dtDet.Rows[0][Globals.COL_DET_RECUPERO].ToString(), dtDet.Rows[0][Globals.COL_DET_ATTESA].ToString(), int.Parse(dtDet.Rows[0][Globals.COL_ESE_ID].ToString()), int.Parse(dtDet.Rows[0][Globals.COL_SCH_ID].ToString())))
                    {
                        CaricaDettaglidaScheda(Globals.INTERO);
                        ModificaDettagli();
                    }
            }
        }
    }
    private void btn_EditDettagli(int Pos)
    {
        var tuple = AppManager.Instance.sql.SelectAllDettagli(" WHERE det." + Globals.COL_DET_ID_SCHEDA + " = " + Globals.INTERO + " AND det." + Globals.COL_DET_POSIZIONE + " = " + Pos, "det." + Globals.COL_DET_ID);
        DataTable dtEditDet = tuple.Item1;
        if (dtEditDet.Rows.Count == 1)
        {
            int ID = int.Parse(dtEditDet.Rows[0][Globals.COL_DET_ID].ToString());
            string RipT = dtEditDet.Rows[0][Globals.COL_DET_RIPETIZIONIt].ToString();
            string Recupero = dtEditDet.Rows[0][Globals.COL_DET_RECUPERO].ToString();
            string Attesa = dtEditDet.Rows[0][Globals.COL_DET_ATTESA].ToString();
            AppManager.Instance.navManager.NavigateTo(() => AppManager.Instance.objects.ExercisesSwitchState("AddDetails"));
            AppManager.Instance.dettagliEsercizi.CaricaPaginaDettagli(ID, dtEditDet.Rows[0][Globals.COL_ESE_NOME].ToString(), int.Parse(dtEditDet.Rows[0][Globals.COL_DET_SERIE].ToString()), Recupero, Attesa, RipT, int.Parse(dtEditDet.Rows[0][Globals.COL_DET_RIPETIZIONIn].ToString()), dtEditDet.Rows[0][Globals.COL_DET_RIPnOt].ToString(), Pos, int.Parse(dtEditDet.Rows[0][Globals.COL_ESE_ID].ToString()), Globals.INTERO);
        }
    }
    #endregion
    #endregion

    private void CaricaBottone(Image immagine, DataTable dtTable, Button[] Duplicato, List<byte[]> arrayList, int row, Button button, string Muscoli)
    {
        #region Creazione bottoni
        //Duplica bottone
        Duplicato[row] = Instantiate(button);
        //Rinominalo
        Duplicato[row].name = "EsN" + row;
        //Assegnagli come padre "Content"
        Duplicato[row].transform.SetParent((GameObject.Find("Content")).transform);
        //Assegnazione indice (posizione)
        Duplicato[row].transform.SetSiblingIndex(row + 1);
        //Cambia la stringa del testo figlio
        Duplicato[row].transform.Find("Text").GetComponent<TextMeshProUGUI>().text = dtTable.Rows[row][Globals.COL_ESE_NOME].ToString();

        if (Muscoli != "")
            Duplicato[row].transform.Find("txtMuscles").GetComponent<TextMeshProUGUI>().text = Muscoli;
        #endregion
        #region Immagine nel bottone
        try
        {
            //Converto in un array di byte la riga n della lista contenente gli array di byte
            byte[] ByteImmagine = arrayList[row];
            if (ByteImmagine != null)
            {
                //Ottengo l'immagine dai byte
                immagine = AppManager.Instance.uploadImage.ConvertByteArrayToImage(ByteImmagine);
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
        //RectTransform rtCnv = Duplicato[row].transform.Find("cnvSelect").GetComponent<Canvas>().GetComponent<RectTransform>();
        //rtCnv.anchoredPosition = new Vector2(1, 1)
    }
}