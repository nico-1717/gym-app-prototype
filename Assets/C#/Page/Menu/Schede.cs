using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Schede : MonoBehaviour
{
    static Button btnAddSchede;
    private void Start()
    {
        btnAddSchede = Globals.PAGE_TAB_VIEW.transform.Find("Scroll View/Viewport/Content/Aggiungi").GetComponent<Button>();
    }
    public void Carica()
    {
        SvuotaPaginaSchede();

        DataTable dtTable = AppManager.Instance.sql.SelectAllSchede("", Globals.COL_SCH_NOME);

        List<Transform> tChildren = new List<Transform>();
        Button[] Duplicato = new Button[dtTable.Rows.Count];
        for (int row = 0; row < dtTable.Rows.Count; row++)
        {
            int IDscheda = int.Parse(dtTable.Rows[row][Globals.COL_SCH_ID].ToString());

            InstanziaBottone(dtTable.Rows[row][Globals.COL_SCH_NOME].ToString(), Duplicato, row, IDscheda);
        }

        Globals.BTN_ADD_SCHEDA.onClick.RemoveAllListeners();
        Globals.BTN_ADD_SCHEDA.onClick.AddListener(() => btnAggiungi());

        ShowSchede();
    }
    private void ShowSchede()
    {
        Globals.BTN_ADD_SCHEDA.gameObject.SetActive(true);

        List<Transform> tChildren = new List<Transform>();

        GameObject parent = Globals.BTN_ADD_SCHEDA.transform.parent.gameObject;

        parent.GetComponent<VerticalLayoutGroup>().spacing = 0;
        parent.GetComponent<VerticalLayoutGroup>().padding.top = 0;

        //per prendere la lista di child
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            Transform child = parent.transform.GetChild(i);
            tChildren.Add(child.transform);

            parent.transform.GetChild(i).Find("CanvasBtnEditDel").GetComponent<Canvas>().gameObject.SetActive(false);
        }

        foreach (Transform t in tChildren)
        {
            GameObject btn = t.gameObject;

            if (btn != Globals.BTN_ADD_SCHEDA.gameObject)
            {
                btn.GetComponent<Button>().onClick.RemoveAllListeners();
                btn.GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.navManager.NavigateTo(() => Cliccato(int.Parse(btn.transform.Find("txtID").GetComponent<TextMeshProUGUI>().text))));
            }

            ButtonPressEvent bpe = btn.AddComponent<ButtonPressEvent>();
            bpe.onPressed.AddListener(() => AppManager.Instance.shakeObject.Shake(tChildren));
            bpe.onPressed.AddListener(ModificaSchede);

        }
    }
    public void Cliccato(int ID)
    {
        AppManager.Instance.objects.ExercisesSwitchState("ViewDetails");
        AppManager.Instance.esercizi.CaricaDettaglidaScheda(ID);
    }
    private void btnAggiungi()
    {
        Globals.BTN_ADD_SCHEDA.GetComponentInChildren<TextMeshProUGUI>().text = "";
        Globals.BTN_ADD_SCHEDA.transform.Find("ImgCircle").GetComponent<Image>().gameObject.SetActive(false);

        Globals.BTN_ADD_SCHEDA.transform.Find("InputField").GetComponent<TMP_InputField>().gameObject.SetActive(true);
        Globals.BTN_ADD_SCHEDA.transform.Find("InputField").GetComponent<TMP_InputField>().text = "";
        Globals.BTN_ADD_SCHEDA.transform.Find("InputField").GetComponent<TMP_InputField>().ActivateInputField();
        Globals.BTN_ADD_SCHEDA.transform.Find("CanvasButton").GetComponent<Canvas>().gameObject.SetActive(true);

        Globals.BTN_ADD_SCHEDA.transform.Find("CanvasButton/Confirm").GetComponent<Button>().onClick.RemoveAllListeners();
        Globals.BTN_ADD_SCHEDA.transform.Find("CanvasButton/Cancel").GetComponent<Button>().onClick.RemoveAllListeners();

        Globals.BTN_ADD_SCHEDA.transform.Find("CanvasButton/Confirm").GetComponent<Button>().onClick.AddListener(() => Inserisci());
        Globals.BTN_ADD_SCHEDA.transform.Find("CanvasButton/Cancel").GetComponent<Button>().onClick.AddListener(() => Carica());
    }
    private void Inserisci()
    {
        AppManager.Instance.sql.InsertScheda(Globals.BTN_ADD_SCHEDA.GetComponent<Button>().transform.Find("InputField").GetComponent<TMP_InputField>().text);
        Carica();
    }
    public void CaricaSchedeEsercizio(int ID)
    {
        Globals.INTERO = ID;
        DataTable dtTable;

        SvuotaPaginaSchede();

        string sCondition = " WHERE es." + Globals.COL_ESE_ID + " = " + ID + " ";
        var tuple = AppManager.Instance.sql.SelectAllDettagli(sCondition, "det." + Globals.COL_DET_ID);
        dtTable = tuple.Item1;
        Button[] Duplicato = new Button[dtTable.Rows.Count];

        List<KeyValuePair<int, string>> ListaSchede = new List<KeyValuePair<int, string>>();
        for (int row = 0; row < dtTable.Rows.Count; row++)
        {
            KeyValuePair<int, string> x = new KeyValuePair<int, string>(int.Parse(dtTable.Rows[row][Globals.COL_SCH_ID].ToString()), dtTable.Rows[row][Globals.COL_SCH_NOME].ToString());
            ListaSchede.Add(x);
        }
        ListaSchede = ListaSchede.Distinct().ToList();
        int nScheda = 0;
        foreach (KeyValuePair<int, string> scheda in ListaSchede)
        {
            InstanziaBottone(scheda.Value, Duplicato, nScheda, scheda.Key);
            nScheda += 1;
        }
        Globals.BTN_ADD_SCHEDA.onClick.RemoveAllListeners();
        Globals.BTN_ADD_SCHEDA.onClick.AddListener(() => AppManager.Instance.navManager.NavigateTo(() => SelezionaSchede(ID)));
    }
    #region Seleziona schede
    //0 = no
    //1 = yes
    //2 = alredy selected
    static int[,] iScSelected;
    static Button[] Duplicato;

    public void SelezionaSchede(int IDesercizio)
    {
        DataTable dtTable;
        List<byte[]> arrayList = new List<byte[]>();

        AppManager.Instance.objects.Button_Area("Answer");
        Globals.BTN_CONFIRM.onClick.RemoveAllListeners();
        Globals.BTN_CONFIRM.onClick.AddListener(() => InserisciRelazioneSchedaEsercizi(IDesercizio));
        Globals.BTN_CANCEL.onClick.RemoveAllListeners();
        Globals.BTN_CANCEL.onClick.AddListener(() => AppManager.Instance.navManager.NavigateBack());

        SvuotaPaginaSchede();

        dtTable = AppManager.Instance.sql.SelectAllSchede("", Globals.COL_SCH_NOME);

        Duplicato = new Button[dtTable.Rows.Count];
        iScSelected = new int[dtTable.Rows.Count, dtTable.Rows.Count];
        Array.Clear(iScSelected, 0, iScSelected.Length);

        var tuple = AppManager.Instance.sql.SelectAllDettagli(" WHERE det." + Globals.COL_DET_ID_ESERCIZIO + " = " + IDesercizio, Globals.COL_SCH_ID);
        DataTable dtGiaCollegati = tuple.Item1;

        for (int row = 0; row < dtTable.Rows.Count; row++)
        {
            InstanziaBottone(dtTable.Rows[row][Globals.COL_SCH_NOME].ToString(), Duplicato, row, int.Parse(dtTable.Rows[row][Globals.COL_SCH_NOME].ToString()));
            iScSelected[row, 0] = int.Parse(dtTable.Rows[row][Globals.COL_SCH_ID].ToString());
            iScSelected[row, 1] = 0;
            int iRiga = row;

            for (int riga = 0; riga < dtGiaCollegati.Rows.Count; riga++)
            {
                if (int.Parse(dtTable.Rows[row][Globals.COL_SCH_ID].ToString()) == int.Parse(dtGiaCollegati.Rows[riga][Globals.COL_SCH_ID].ToString()))
                {
                    Duplicato[row].enabled = false;
                    iScSelected[row, 1] = 2;

                    //color
                    ColorBlock theColor;
                    theColor = Duplicato[row].colors;
                    theColor.normalColor = Color.gray;
                    Duplicato[row].colors = theColor;
                    break;
                }
                Duplicato[row].onClick.RemoveAllListeners();
                Duplicato[row].onClick.AddListener(() => SwitchSelectScheda(iRiga));
            }
            if (dtGiaCollegati.Rows.Count == 0)
                Duplicato[row].onClick.AddListener(() => SwitchSelectScheda(iRiga));
        }
        Globals.BTN_ADD_SCHEDA.GetComponent<Button>().gameObject.SetActive(false);
    }
    private void SwitchSelectScheda(int row)
    {
        if (iScSelected[row, 1] == 0)
        {
            iScSelected[row, 1] = 1;

            //color
            ColorBlock theColor;
            theColor = Duplicato[row].colors;
            theColor.normalColor = Color.green;
            theColor.selectedColor = Color.green;
            theColor.highlightedColor = Color.green;
            Duplicato[row].colors = theColor;
        }
        else
        {
            iScSelected[row, 1] = 0;

            //color
            ColorBlock theColor;
            theColor = Duplicato[row].colors;
            theColor.normalColor = Color.white;
            theColor.selectedColor = Color.white;
            theColor.highlightedColor = Color.white;
            Duplicato[row].colors = theColor;
        }
    }

    private void InserisciRelazioneSchedaEsercizi(int IDesercizio)
    {
        Insert insert = gameObject.AddComponent<Insert>();

        for (int row = 0; row < Math.Sqrt(iScSelected.Length); row++)
        {
            if (iScSelected[row, 1] == 1)
            {
                insert.InsertRelazioneES(IDesercizio, iScSelected[row, 0]);
            }
        }
        AppManager.Instance.objects.Scenes("Esercizi");
        AppManager.Instance.navManager.NavigateTo(() => AppManager.Instance.objects.ExercisesSwitchState("View&C"));
    }
    #endregion
    private void ModificaSchede()
    {
        GameObject Parent = Globals.BTN_ADD_SCHEDA.transform.parent.gameObject;

        Globals.BTN_ADD_SCHEDA.gameObject.SetActive(false);

        Parent.GetComponent<VerticalLayoutGroup>().spacing = 20;
        Parent.GetComponent<VerticalLayoutGroup>().padding.top = 30;
        for (int row = 0; row < Parent.transform.childCount; row++)
        {
            Parent.transform.GetChild(row).GetComponent<Button>().onClick.RemoveAllListeners();

            int ChildNum = row;
            Parent.transform.GetChild(ChildNum).Find("CanvasBtnEditDel").GetComponent<Canvas>().gameObject.SetActive(true);
            Parent.transform.GetChild(ChildNum).Find("CanvasBtnEditDel/btnDelete").GetComponent<Button>().onClick.RemoveAllListeners();
            Parent.transform.GetChild(ChildNum).Find("CanvasBtnEditDel/btnDelete").GetComponent<Button>().onClick.AddListener(() => btn_DeleteScheda(ChildNum - 1));
            Parent.transform.GetChild(ChildNum).Find("CanvasBtnEditDel/btnUpdate").GetComponent<Button>().onClick.RemoveAllListeners();
            Parent.transform.GetChild(ChildNum).Find("CanvasBtnEditDel/btnUpdate").GetComponent<Button>().onClick.AddListener(() => btn_UpdateScheda(ChildNum));

            ButtonPressEvent bpe = Parent.transform.GetChild(ChildNum).gameObject.AddComponent<ButtonPressEvent>();
            bpe.onPressed.AddListener(() => AppManager.Instance.shakeObject.StopAllShakes());
            bpe.onPressed.AddListener(ShowSchede);
        }
    }
    private void btn_DeleteScheda(int Num)
    {
        bool Deleted = true;
        DataTable dtTable = AppManager.Instance.sql.SelectAllSchede("", Globals.COL_SCH_NOME);
        int ID = int.Parse(dtTable.Rows[Num][Globals.COL_SCH_ID].ToString());
        var tuple = AppManager.Instance.sql.SelectAllDettagli(" WHERE det." + Globals.COL_DET_ID_SCHEDA + " = " + ID, Globals.COL_DET_ID);
        DataTable dtDelDet = tuple.Item1;
        for (int row = 0; row < dtDelDet.Rows.Count; row++)
        {
            if (AppManager.Instance.sql.DeleteDettagli(int.Parse(dtDelDet.Rows[row][Globals.COL_DET_ID].ToString())) == false)
                Deleted = false;
        }
        if (Deleted == true)
            if (AppManager.Instance.sql.DeleteScheda(ID) == false)
                Deleted = false;
        if (Deleted == true)
        {
            Carica();
        }
    }
    private void btn_UpdateScheda(int Num)
    {
        GameObject Parent = Globals.BTN_ADD_SCHEDA.transform.parent.gameObject;
        GameObject EditingButton = Parent.transform.GetChild(Num).gameObject;

        for (int row = 0; row < Parent.transform.childCount; row++)
        {
            Parent.transform.GetChild(row).gameObject.transform.Find("CanvasBtnEditDel").GetComponent<Canvas>().gameObject.SetActive(false);
        }

        EditingButton.GetComponent<Button>().transform.Find("CanvasButton").GetComponent<Canvas>().gameObject.SetActive(true);

        EditingButton.GetComponent<Button>().transform.Find("CanvasButton/Confirm").GetComponent<Button>().onClick.RemoveAllListeners();
        EditingButton.GetComponent<Button>().transform.Find("CanvasButton/Cancel").GetComponent<Button>().onClick.RemoveAllListeners();

        EditingButton.GetComponent<Button>().transform.Find("CanvasButton/Confirm").GetComponent<Button>().onClick.AddListener(() => UpdateScheda(Num - 1, EditingButton));
        EditingButton.GetComponent<Button>().transform.Find("CanvasButton/Cancel").GetComponent<Button>().onClick.AddListener(() => Carica());

        EditingButton.GetComponent<Button>().transform.Find("InputField").GetComponent<TMP_InputField>().gameObject.SetActive(true);
        EditingButton.GetComponent<Button>().transform.Find("InputField").GetComponent<TMP_InputField>().text = EditingButton.GetComponentInChildren<TextMeshProUGUI>().text;
        EditingButton.GetComponent<Button>().transform.Find("InputField").GetComponent<TMP_InputField>().ActivateInputField();
    }
    private void UpdateScheda(int Num, GameObject EditingButton)
    {
        bool Edited = false;
        DataTable dtTable = AppManager.Instance.sql.SelectAllSchede("", Globals.COL_SCH_NOME);
        int ID = int.Parse(dtTable.Rows[Num][Globals.COL_SCH_ID].ToString());
        string a = EditingButton.GetComponent<Button>().transform.Find("InputField").GetComponent<TMP_InputField>().text;
        if (AppManager.Instance.sql.UpdateScheda(ID, EditingButton.GetComponent<Button>().transform.Find("InputField").GetComponent<TMP_InputField>().text) == true)
            Edited = true;
        if (Edited == true)
        {
            Carica();
        }
    }
    private void InstanziaBottone(string Nome, Button[] Duplicato, int row, int ID)
    {
        Duplicato[row] = Instantiate(Globals.BTN_ADD_SCHEDA);
        Duplicato[row].name = "SchedaN" + row;
        Duplicato[row].transform.SetParent((GameObject.Find("Content")).transform);
        Duplicato[row].transform.SetSiblingIndex(row + 1);
        Destroy(GameObject.Find(Duplicato[row].gameObject.name).transform.Find("ImgCircle").gameObject);
        Duplicato[row].transform.Find("Text").GetComponent<TextMeshProUGUI>().rectTransform.anchoredPosition = new Vector2(0, 0);
        Duplicato[row].transform.Find("Text").GetComponent<TextMeshProUGUI>().text = Nome;
        Duplicato[row].transform.Find("txtID").GetComponent<TextMeshProUGUI>().text = ID.ToString();
    }

    public void SvuotaPaginaSchede()
    {
        Globals.BTN_ADD_SCHEDA.GetComponent<Button>().gameObject.SetActive(true);
        Globals.BTN_ADD_SCHEDA.GetComponentInChildren<TextMeshProUGUI>().text = "AGGIUNGI";
        Globals.BTN_ADD_SCHEDA.GetComponent<Button>().transform.Find("ImgCircle").GetComponent<Image>().gameObject.SetActive(true);
        Globals.BTN_ADD_SCHEDA.GetComponent<Button>().transform.Find("InputField").GetComponent<TMP_InputField>().gameObject.SetActive(false);
        Globals.BTN_ADD_SCHEDA.GetComponent<Button>().transform.Find("CanvasButton").GetComponent<Canvas>().gameObject.SetActive(false);
        Globals.BTN_ADD_SCHEDA.GetComponent<Button>().transform.Find("CanvasBtnEditDel").GetComponent<Canvas>().gameObject.SetActive(false);

        GameObject Parent = Globals.BTN_ADD_SCHEDA.transform.parent.gameObject;
        Parent.GetComponent<VerticalLayoutGroup>().spacing = 0;
        Parent.GetComponent<VerticalLayoutGroup>().padding.top = 0;

        while (Parent.transform.childCount > 1)
        {
            DestroyImmediate(Parent.transform.GetChild(Parent.transform.childCount - 1).gameObject);
        }
    }
}
