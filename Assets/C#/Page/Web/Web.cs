using SimpleJSON;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Linq;

public class Web : MonoBehaviour
{
    static private Button btnScheda;
    static private Button btnDettaglio;
    static private Button btnEsercizio;
    static private Canvas cnvCell;

    static private int IDpacchetto;
    //null = no filter, "..." = text filter
    static private string sFilter;

    private const int iNumPacCar = 2;
    private const int iNumPacAltrCar = 1;

    private void Start()
    {
        btnScheda = Globals.CANVAS_WEB.transform.Find("cnvSchede/Scroll View/Viewport/Content/btnScheda").GetComponent<Button>();
        btnDettaglio = Globals.CANVAS_WEB.transform.Find("cnvDettagli/cnvSV/Scroll View/Viewport/Content/btnDettaglio").GetComponent<Button>();
        btnEsercizio = Globals.CANVAS_WEB.transform.Find("cnvEsercizi/Scroll View/Viewport/Content/btnEsercizio").GetComponent<Button>();
        cnvCell = Globals.CANVAS_WEB.transform.Find("cnvPacchetti/svPacchetti/Viewport/Content/cnvCell").GetComponent<Canvas>();

        Globals.CANVAS_WEB.transform.Find("cnvPacchetti/svPacchetti/Viewport/Content/cnvLastRow/btnCarica").GetComponent<Button>().onClick.RemoveAllListeners();
        Globals.CANVAS_WEB.transform.Find("cnvPacchetti/svPacchetti/Viewport/Content/cnvLastRow/btnCarica").GetComponent<Button>().onClick.AddListener(CaricaAltriPacchetti);

        Globals.CANVAS_WEB.transform.Find("cnvPacchetti/cnvSearchBtn/btnSearch").GetComponent<Button>().onClick.RemoveAllListeners();
        Globals.CANVAS_WEB.transform.Find("cnvPacchetti/cnvSearchBtn/btnSearch").GetComponent<Button>().onClick.AddListener(FilterPacchetto);

        Globals.CANVAS_WEB.transform.Find("cnvPacchetti/cnvSearchBtn/InputField/btnReset").GetComponent<Button>().onClick.RemoveAllListeners();
        Globals.CANVAS_WEB.transform.Find("cnvPacchetti/cnvSearchBtn/InputField/btnReset").GetComponent<Button>().onClick.AddListener(ResetTextSearch);

        Globals.CANVAS_WEB.transform.Find("btnUpload").GetComponent<Button>().onClick.RemoveAllListeners();
        Globals.CANVAS_WEB.transform.Find("btnUpload").GetComponent<Button>().onClick.AddListener(CaricaUploadPacchetto);

        Globals.CANVAS_WEB.transform.Find("cnvDettagli/Background/invisibleBtnBack").GetComponent<Button>().onClick.RemoveAllListeners();
        Globals.CANVAS_WEB.transform.Find("cnvDettagli/Background/invisibleBtnBack").GetComponent<Button>().onClick.AddListener(AppManager.Instance.navManager.NavigateBack);

        Globals.CANVAS_WEB.transform.Find("cnvEsercizio/Background/invisibleBtnBack").GetComponent<Button>().onClick.RemoveAllListeners();
        Globals.CANVAS_WEB.transform.Find("cnvEsercizio/Background/invisibleBtnBack").GetComponent<Button>().onClick.AddListener(AppManager.Instance.navManager.NavigateBack);
    }
    public void Apri()
    {
        AppManager.Instance.objects.SelectWeb();

        btnScheda.gameObject.SetActive(false);
        btnDettaglio.gameObject.SetActive(false);
        btnEsercizio.gameObject.SetActive(false);

        Globals.CANVAS_WEB.transform.Find("ppLoad").GetComponent<Canvas>().gameObject.SetActive(false);


        Globals.CANVAS_WEB.transform.Find("cnvPacchetti/svPacchetti/Viewport/Content/cnvLastRow/Text").gameObject.SetActive(false);
        Globals.CANVAS_WEB.transform.Find("cnvPacchetti/svPacchetti/Viewport/Content/cnvLastRow/btnCarica").gameObject.SetActive(true);

        ResetTextSearch();

        CaricaPacchetti();
    }
    private void ResetTextSearch()
    {
        Globals.CANVAS_WEB.transform.Find("cnvPacchetti/cnvSearchBtn/InputField").GetComponent<TMP_InputField>().text = "";
    }
    #region Switch
    private Dictionary<string, Canvas> canvasMap = new Dictionary<string, Canvas>();

    private void InitializeCanvasMap()
    {
        canvasMap["Pacchetti"] = Globals.CANVAS_WEB.transform.Find("cnvPacchetti").GetComponent<Canvas>();
        canvasMap["Pacchetto"] = Globals.CANVAS_WEB.transform.Find("cnvPacchetto").GetComponent<Canvas>();
        canvasMap["Schede"] = Globals.CANVAS_WEB.transform.Find("cnvSchede").GetComponent<Canvas>();
        canvasMap["Dettagli"] = Globals.CANVAS_WEB.transform.Find("cnvDettagli").GetComponent<Canvas>();
        canvasMap["Esercizi"] = Globals.CANVAS_WEB.transform.Find("cnvEsercizi").GetComponent<Canvas>();
        canvasMap["Esercizio"] = Globals.CANVAS_WEB.transform.Find("cnvEsercizio").GetComponent<Canvas>();
    }

    private void SwitchCanvas(string canvasName)
    {
        if (canvasMap.Count == 0)
        {
            InitializeCanvasMap();
        }

        foreach (var oggetto in canvasMap)
        {
            var canvas = oggetto.Value;
            canvas.gameObject.SetActive(false);
        }

        Button btnUpload = Globals.CANVAS_WEB.transform.Find("btnUpload").GetComponent<Button>();

        if (canvasMap.ContainsKey(canvasName))
        {
            var targetCanvas = canvasMap[canvasName];
            targetCanvas.gameObject.SetActive(true);

            if (canvasName == "Dettagli")
            {
                targetCanvas = canvasMap["Schede"];
                targetCanvas.gameObject.SetActive(true);
            }
            if (canvasName == "Esercizio")
            {
                targetCanvas = canvasMap["Esercizi"];
                targetCanvas.gameObject.SetActive(true);
            }

            if (canvasName == "Pacchetti")
            {
                btnUpload.gameObject.SetActive(true);
            }
            else
            {
                btnUpload.gameObject.SetActive(false);
            }
        }
    }
    #endregion
    #region btnDownload
    private void btnDownload(string Click, int ID, bool bDownload = true)
    {
        Button button = Globals.CANVAS_WEB.transform.Find("btnDownload").GetComponent<Button>();
        if (bDownload == true)
        {
            button.transform.Find("Text").GetComponentInChildren<TextMeshProUGUI>().text = "DOWNLOAD";
            button.onClick.RemoveAllListeners();
            switch (Click)
            {
                case "All":
                    button.onClick.AddListener(() => AppManager.Instance.download.DownloadAll(ID));
                    button.gameObject.SetActive(true);
                    break;
                case "Schede":
                    button.onClick.AddListener(() => AppManager.Instance.download.DownloadSchede(ID));
                    button.gameObject.SetActive(true);
                    break;
                case "Esercizi":
                    button.onClick.AddListener(() => AppManager.Instance.download.DownloadEsercizi(ID));
                    button.gameObject.SetActive(true);
                    break;
                case "Scheda":
                    button.onClick.AddListener(() => AppManager.Instance.download.DownloadScheda(ID));
                    button.gameObject.SetActive(true);
                    break;
                case "Esercizio":
                    button.onClick.AddListener(() => AppManager.Instance.download.DownloadEsercizio(ID));
                    button.gameObject.SetActive(true);
                    break;
                case null:
                    button.gameObject.SetActive(false);
                    break;
            }
        }
        else
        {
            button.transform.Find("Text").GetComponentInChildren<TextMeshProUGUI>().text = "UPLOAD";
        }
    }
    #endregion 


    #region Pacchetti
    public void CaricaPacchetti()
    {
        Svuota(Globals.CANVAS_WEB.transform.Find("cnvPacchetti/svPacchetti/Viewport/Content").gameObject, 2);

        btnDownload(null, 0);

        sFilter = null;

        StartCoroutine(AppManager.Instance.dbweb.SelezionaPacchetti(iNumPacCar, 0));

        SwitchCanvas("Pacchetti");
    }
    private void CaricaAltriPacchetti()
    {
        int iChild = cnvCell.transform.parent.childCount;
        if (sFilter == null)
            StartCoroutine(AppManager.Instance.dbweb.SelezionaPacchetti(iNumPacAltrCar, iChild - 2));
        else
            StartCoroutine(AppManager.Instance.dbweb.FiltraPacchetti(sFilter, iNumPacAltrCar, iChild - 2));
    }
    private void FilterPacchetto()
    {
        string sText = Globals.CANVAS_WEB.transform.Find("cnvPacchetti/cnvSearchBtn/InputField/Text").GetComponent<TextMeshProUGUI>().text;

        Svuota(Globals.CANVAS_WEB.transform.Find("cnvPacchetti/svPacchetti/Viewport/Content").gameObject, 2);

        Globals.CANVAS_WEB.transform.Find("cnvPacchetti/svPacchetti/Viewport/Content/cnvLastRow/Text").gameObject.SetActive(false);
        Globals.CANVAS_WEB.transform.Find("cnvPacchetti/svPacchetti/Viewport/Content/cnvLastRow/btnCarica").gameObject.SetActive(true);

        if (sText == "")
        {
            CaricaPacchetti();
        }
        else
        {
            sFilter = sText;
            StartCoroutine(AppManager.Instance.dbweb.FiltraPacchetti(sText, iNumPacCar, 0));
        }
    }
    public void PosLastRow()
    {
        const int PosUnityBoh = -874;
        double iChild = cnvCell.transform.parent.childCount - 2;

        Globals.CANVAS_WEB.transform.Find("cnvPacchetti/svPacchetti/Viewport/Content/cnvLastRow").gameObject.GetComponent<RectTransform>().position = new Vector2(0, ((float)Math.Ceiling(iChild / 3) * -cnvCell.GetComponent<RectTransform>().rect.height) - 50 - PosUnityBoh);
        Globals.CANVAS_WEB.transform.Find("cnvPacchetti/svPacchetti/Viewport/Content").gameObject.GetComponent<LayoutElement>().minHeight =
            ((float)Math.Ceiling(iChild / 3) * cnvCell.GetComponent<RectTransform>().rect.height) +
            Globals.CANVAS_WEB.transform.Find("cnvPacchetti/svPacchetti/Viewport/Content/cnvLastRow").gameObject.GetComponent<RectTransform>().rect.height + 10;
    }
    public void DisattivaBtnCarica()
    {
        Globals.CANVAS_WEB.transform.Find("cnvPacchetti/svPacchetti/Viewport/Content/cnvLastRow/Text").gameObject.SetActive(true);
        Globals.CANVAS_WEB.transform.Find("cnvPacchetti/svPacchetti/Viewport/Content/cnvLastRow/btnCarica").gameObject.SetActive(false);
    }


    public void InstanziaPacchetto(int ID, string Nome, string Utente, int Versione)
    {
        Button bottone;
        Canvas cnvDuplicato;
        GameObject parent = cnvCell.transform.parent.gameObject;

        int num = parent.transform.childCount;

        cnvDuplicato = Instantiate(cnvCell);
        cnvDuplicato.name = "Cell" + num;
        cnvDuplicato.transform.SetParent(parent.transform);
        cnvDuplicato.transform.SetSiblingIndex(num + 2);

        cnvDuplicato.GetComponent<RectTransform>().sizeDelta = new Vector2(cnvCell.GetComponent<RectTransform>().rect.width, cnvCell.GetComponent<RectTransform>().rect.height);

        cnvDuplicato.gameObject.SetActive(true);

        bottone = cnvDuplicato.transform.Find("btnPacchetto").GetComponent<Button>();

        bottone.transform.Find("txtPacchetto").GetComponent<TextMeshProUGUI>().text = Nome;
        bottone.transform.Find("txtUtente").GetComponent<TextMeshProUGUI>().text = Utente;

        bottone.onClick.AddListener(() => AppManager.Instance.navManager.NavigateTo(() => StartCoroutine(CaricaPacchetto(ID))));
    }
    #endregion
    #region Pacchetto
    IEnumerator CaricaPacchetto(int ID)
    {
        IDpacchetto = ID;

        Canvas Pacchetto = Globals.CANVAS_WEB.transform.Find("cnvPacchetto").GetComponent<Canvas>();

        JSONArray jsEsercizi = new JSONArray();
        int numEsercizi = 0;
        JSONArray jsSchede = new JSONArray();
        int numSchede = 0;
        Tuple<string, int, string> tPacchetto = new Tuple<string, int, string>("", 0, "");
        string NomePacchetto;
        int VersionePacchetto;
        string Utente;

        yield return StartCoroutine(AppManager.Instance.dbweb.SelezionaEsercizi(ID, result =>
        {
            jsEsercizi = result;
        }));
        if (jsEsercizi != null)
            numEsercizi = jsEsercizi.Count;
        else
            numEsercizi = 0;
        yield return StartCoroutine(AppManager.Instance.dbweb.SelezionaSchede(ID, result =>
        {
            jsSchede = result;
        }));
        if (jsSchede != null)
            numSchede = jsSchede.Count;
        else
            numSchede = 0;
        yield return StartCoroutine(AppManager.Instance.dbweb.DatiPacchetto(ID, result =>
        {
            tPacchetto = result;
        }));

        NomePacchetto = tPacchetto.Item1;
        VersionePacchetto = tPacchetto.Item2;
        Utente = tPacchetto.Item3;

        Pacchetto.transform.Find("txtPacchetto").GetComponent<TextMeshProUGUI>().text = NomePacchetto + "." + VersionePacchetto;
        Pacchetto.transform.Find("txtUtente").GetComponent<TextMeshProUGUI>().text = Utente;

        Pacchetto.transform.Find("txtPacchetto").gameObject.SetActive(true);
        Pacchetto.transform.Find("txtUpload").gameObject.SetActive(false);
        Pacchetto.transform.Find("ifNomePacchetto").gameObject.SetActive(false);

        Pacchetto.transform.Find("cnvEsercizi/btnEsercizi").GetComponent<Button>().onClick.RemoveAllListeners();
        Pacchetto.transform.Find("cnvSchede/btnSchede").GetComponent<Button>().onClick.RemoveAllListeners();

        Svuota(Globals.CANVAS_WEB.transform.Find("cnvPacchetto/cnvEsercizi/svEsercizi/Viewport/Content").gameObject, 1);
        Svuota(Globals.CANVAS_WEB.transform.Find("cnvEsercizi/Scroll View/Viewport/Content").gameObject, 1);
        if (numEsercizi > 0)
        {
            List<int> liEsercizi = new List<int>();
            if (jsEsercizi != null)
            {
                for (int i = 0; i < jsEsercizi.Count; i++)
                {
                    bool trovato = false;
                    foreach (int es in liEsercizi)
                    {
                        if (es == jsEsercizi[i].AsObject["IDesercizio"])
                            trovato = true;
                    }
                    if (trovato == false)
                    {
                        InstanziaAnteprima(i, jsEsercizi[i].AsObject["NomeEsercizio"], false, false);

                        StartCoroutine(Download_InstanziaEsercizio(i, jsEsercizi[i].AsObject["IDesercizio"], jsEsercizi[i].AsObject["Immagine"],
                            jsEsercizi[i].AsObject["NomeEsercizio"], jsEsercizi[i].AsObject["Consigli"], jsEsercizi[i].AsObject["Note"]));

                        liEsercizi.Add(jsEsercizi[i].AsObject["IDesercizio"]);
                    }
                }
            }

            Pacchetto.transform.Find("cnvEsercizi/btnEsercizi").GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.navManager.NavigateTo(() => Download_MostraEsercizi(ID)));
        }

        Svuota(Globals.CANVAS_WEB.transform.Find("cnvPacchetto/cnvSchede/svSchede/Viewport/Content").gameObject, 1);
        Svuota(Globals.CANVAS_WEB.transform.Find("cnvSchede/Scroll View/Viewport/Content").gameObject, 1);
        if (numSchede > 0)
        {
            if (jsSchede != null)
            {
                for (int i = 0; i < jsSchede.Count; i++)
                {
                    InstanziaAnteprima(i, jsSchede[i].AsObject["NomeScheda"], true, false);

                    Download_InstanziaScheda(i, jsSchede[i].AsObject["IDscheda"], jsSchede[i].AsObject["NomeScheda"]);
                }
            }

            Pacchetto.transform.Find("cnvSchede/btnSchede").GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.navManager.NavigateTo(() => Download_MostraSchede(ID)));
        }

        SwitchCanvas("Pacchetto");

        btnDownload("All", ID);
    }

    private void CaricaUploadPacchetto()
    {
        Canvas Pacchetto = Globals.CANVAS_WEB.transform.Find("cnvPacchetto").GetComponent<Canvas>();

        Pacchetto.transform.Find("txtPacchetto").gameObject.SetActive(false);
        Pacchetto.transform.Find("txtUpload").gameObject.SetActive(true);
        Pacchetto.transform.Find("ifNomePacchetto").gameObject.SetActive(true);
        Pacchetto.transform.Find("txtUtente").GetComponent<TextMeshProUGUI>().text = "";

        Upload_Carica_ScEs();

        Pacchetto.transform.Find("cnvEsercizi/btnEsercizi").GetComponent<Button>().onClick.RemoveAllListeners();
        Pacchetto.transform.Find("cnvSchede/btnSchede").GetComponent<Button>().onClick.RemoveAllListeners();
        Pacchetto.transform.Find("cnvEsercizi/btnEsercizi").GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.navManager.NavigateTo(Upload_ClickEsercizi));
        Pacchetto.transform.Find("cnvSchede/btnSchede").GetComponent<Button>().onClick.AddListener(() => AppManager.Instance.navManager.NavigateTo(Upload_ClickSchede));

        AppManager.Instance.navManager.NavigateTo(MostraUploadPacchetto);
    }
    //funzione che viene chiamata anche quando si torna indietro
    private void MostraUploadPacchetto()
    {
        Upload_CaricaAnteprima();

        SwitchCanvas("Pacchetto");

        Globals.CANVAS_MENU.gameObject.SetActive(false);
        Globals.CANVAS_WEB.gameObject.SetActive(true);

        btnDownload("All", 0, false);
    }
    private void Upload_CaricaAnteprima()
    {
        Svuota(Globals.CANVAS_WEB.transform.Find("cnvPacchetto/cnvSchede/svSchede/Viewport/Content").gameObject, 1);
        Svuota(Globals.CANVAS_WEB.transform.Find("cnvPacchetto/cnvEsercizi/svEsercizi/Viewport/Content").gameObject, 1);

        for (int i = 0; i < iScSel.GetLength(0); i++)
        {
            if (iScSel[i, 1] == 1)
                InstanziaAnteprima(i, btnScheda.transform.parent.GetChild(iScSel[i, 3]).Find("Text").GetComponent<TextMeshProUGUI>().text, true, true, iScSel[i, 0]);
        }
        for (int i = 0; i < iEsSel.GetLength(0); i++)
        {
            if (iEsSel[i, 1] == 1)
            {
                bool bbtnDel = true;
                for (int j = 0; j < iS_E.GetLength(0); j++)
                {
                    if (iS_E[j, 1] == iEsSel[i, 0])
                    {
                        for (int y = 0; y < iScSel.GetLength(0); y++)
                        {
                            if (iS_E[j, 0] == iScSel[y, 0])
                            {
                                if (iScSel[y, 1] == 1)
                                {
                                    bbtnDel = false;
                                    goto ExitCicliBtnDel;
                                }
                                break;
                            }
                        }
                    }
                }
            ExitCicliBtnDel:
                InstanziaAnteprima(i, btnEsercizio.transform.parent.GetChild(iEsSel[i, 3]).Find("Text").GetComponent<TextMeshProUGUI>().text, false, bbtnDel, iEsSel[i, 0], true);
            }
        }
    }
    //0 = IDscheda
    //1 = Official Select (0 = no, 1 = sì)
    //2 = Temporal Select (0 = no, 1 = sì)
    //3 = Posizione
    static int[,] iScSel;
    //0 = IDesercizio
    //1 = Official Select (0 = no, 1 = sì)
    //2 = Temporal Select (0 = no, 1 = sì)
    //3 = Posizione
    static int[,] iEsSel;
    static int[,] iS_E;
    static List<byte[]> lbImmaginiEs;
    private void Upload_Carica_ScEs()
    {
        //Esercizi
        DataTable dtTableEs;
        Button btnAddEsercizio = btnEsercizio;
        Svuota(btnAddEsercizio.transform.parent.gameObject, 1);
        var tuple = AppManager.Instance.sql.SelectAllEsercizi("", Globals.COL_ESE_ID);
        dtTableEs = tuple.Item1;
        lbImmaginiEs = tuple.Item2;
        iEsSel = new int[dtTableEs.Rows.Count, 4];
        btnAddEsercizio.gameObject.SetActive(true);
        btnAddEsercizio.onClick.RemoveAllListeners();
        for (int row = 0; row < dtTableEs.Rows.Count; row++)
        {
            DataTable dtMxE = AppManager.Instance.sql.SelectAllME(" WHERE me." + Globals.COL_MxE_IDESERCIZIO + " = " + (int)dtTableEs.Rows[row][Globals.COL_ESE_ID], "mu." + Globals.COL_MUS_ID);
            List<string> liMuscoli = new List<string>();
            for (int i = 0; i < dtMxE.Rows.Count; i++)
            {
                liMuscoli.Add(dtMxE.Rows[i][Globals.COL_MUS_NOME].ToString());
            }

            Upload_InstanziaBottoneEsercizi((int)dtTableEs.Rows[row][Globals.COL_ESE_ID], row, btnAddEsercizio, dtTableEs.Rows[row][Globals.COL_ESE_NOME].ToString(), dtTableEs.Rows[row][Globals.COL_ESE_CONSIGLI].ToString(),
            dtTableEs.Rows[row][Globals.COL_ESE_NOTE].ToString(), /*dtTableEs.Rows[row][Globals.COL_ESE_IMMAGINE].ToString()*/ null, liMuscoli);

            iEsSel[row, 0] = int.Parse(dtTableEs.Rows[row][Globals.COL_ESE_ID].ToString());
            iEsSel[row, 1] = 0;
            iEsSel[row, 2] = 0;
            iEsSel[row, 3] = row + 1;
        }
        btnAddEsercizio.gameObject.SetActive(false);
        //Schede
        DataTable dtTableSc;
        Button btnAddScheda = btnScheda;
        Svuota(btnAddScheda.transform.parent.gameObject, 1);
        dtTableSc = AppManager.Instance.sql.SelectAllSchede("", Globals.COL_SCH_ID);
        iScSel = new int[dtTableSc.Rows.Count, 4];
        btnAddScheda.gameObject.SetActive(true);
        btnAddScheda.onClick.RemoveAllListeners();
        for (int row = 0; row < dtTableSc.Rows.Count; row++)
        {
            Upload_InstanziaBottoneSchede((int)dtTableSc.Rows[row][Globals.COL_SCH_ID], dtTableSc.Rows[row][Globals.COL_SCH_NOME].ToString(), row, btnAddScheda);

            iScSel[row, 0] = int.Parse(dtTableSc.Rows[row][Globals.COL_SCH_ID].ToString());
            iScSel[row, 1] = 0;
            iScSel[row, 2] = 0;
            iScSel[row, 3] = row + 1;
        }
        btnAddScheda.gameObject.SetActive(false);
        //Dettagli
        DataTable dtTableDet = AppManager.Instance.sql.SelectAllDettagli("", Globals.COL_DET_ID).Item1;
        iS_E = new int[dtTableDet.Rows.Count, 2];
        for (int row = 0; row < dtTableDet.Rows.Count; row++)
        {
            iS_E[row, 0] = int.Parse(dtTableDet.Rows[row][Globals.COL_SCH_ID].ToString());
            iS_E[row, 1] = int.Parse(dtTableDet.Rows[row][Globals.COL_ESE_ID].ToString());
        }
    }
    private void UploadSelectClick(int row, bool bSchede)
    {
        GameObject goParent;
        Button btn;
        ColorBlock theColor;
        Color newColor;
        int[,] Matrix;
        if (bSchede)
        {
            goParent = btnScheda.transform.parent.gameObject;
            Matrix = iScSel;
        }
        else
        {
            goParent = btnEsercizio.transform.parent.gameObject;
            Matrix = iEsSel;
        }
        btn = goParent.transform.GetChild(Matrix[row, 3]).GetComponent<Button>();
        if (Matrix[row, 2] == 0)
        {
            Matrix[row, 2] = 1;

            newColor = Color.green;
        }
        else
        {
            //se si sta tentando di deselezionare un esercizio che compare in una scheda attiva, si esce mostrando il messaggio
            if (!bSchede)
            {
                for (int i = 0; i < iS_E.GetLength(0); i++)
                {
                    if (iS_E[i, 1] == iEsSel[row, 0])
                    {
                        for (int j = 0; j < iScSel.GetLength(0); j++)
                        {
                            if (iS_E[i, 0] == iScSel[j, 0])
                            {
                                if (iScSel[j, 1] == 1)
                                {
                                    StartCoroutine(AppManager.Instance.messagebox.ShowMsg(2, "non puoi eliminare questo esercizio finché è presente una scheda che lo utilizza", 5));
                                    return;
                                }
                            }
                        }
                    }
                }
            }
            Matrix[row, 2] = 0;

            newColor = goParent.transform.GetChild(0).GetComponent<Button>().colors.normalColor;
        }
        theColor = btn.colors;
        theColor.normalColor = newColor;
        theColor.selectedColor = newColor;
        theColor.highlightedColor = newColor;
        btn.colors = theColor;
    }
    private void Upload_InstanziaBottoneEsercizi(int IDes, int row, Button btnToDuplicate, string Nome, string Consigli, string Note, string Immagine, List<string> liMuscoli)
    {
        Button btnDuplicato = Instantiate(btnToDuplicate);
        btnDuplicato.name = "EsN" + row;
        btnDuplicato.transform.SetParent(btnToDuplicate.transform.parent);
        btnDuplicato.transform.SetSiblingIndex(row + 1);
        btnDuplicato.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = Nome;
        btnDuplicato.onClick.AddListener(() => UploadSelectClick(row, false));

        TextMeshProUGUI txtMuscoli = btnDuplicato.transform.Find("txtMuscles").GetComponent<TextMeshProUGUI>();
        txtMuscoli.text = "";

        if (liMuscoli.Count > 0)
        {
            for (int i = 0; i < liMuscoli.Count; i++)
            {
                txtMuscoli.text += liMuscoli[i] + ", ";
            }
            txtMuscoli.text = txtMuscoli.text.Substring(0, txtMuscoli.text.Length - 2);
        }
        else
        {
            txtMuscoli.text = "";
        }

        ButtonPressEvent bpe = btnDuplicato.gameObject.AddComponent<ButtonPressEvent>();
        bpe.onPressed.AddListener(() => AppManager.Instance.navManager.NavigateTo(() => MostraEsercizio(IDes, Nome, Consigli, Note, Immagine, liMuscoli)));
    }
    private void CaricaImmaginiEsercizi(byte[] bImmagine, Button btn)
    {
        try
        {
            byte[] ByteImmagine = bImmagine;
            if (ByteImmagine != null)
            {
                Image immagine = AppManager.Instance.uploadImage.ConvertByteArrayToImage(ByteImmagine);
                btn.transform.Find("ImgCircle/Image").GetComponent<Image>().sprite = immagine.sprite;
                btn.transform.Find("ImgCircle/Image").GetComponent<Image>().preserveAspect = true;
                btn.transform.Find("ImgCircle/Image").GetComponent<Image>().SetNativeSize();
                RectTransform rt = btn.transform.Find("ImgCircle/Image").GetComponent<Image>().GetComponent<RectTransform>();
                double width = rt.sizeDelta.x * rt.localScale.x;
                double height = rt.sizeDelta.y * rt.localScale.y;
                if (width > height)
                {
                    btn.transform.Find("ImgCircle/Image").GetComponent<Image>().rectTransform.sizeDelta = new Vector3(4000, 100, 1);
                }
                else
                {
                    btn.transform.Find("ImgCircle/Image").GetComponent<Image>().rectTransform.sizeDelta = new Vector3(100, 4000, 1);
                }
                btn.transform.Find("ImgCircle/Image").GetComponent<Image>().rectTransform.anchoredPosition = new Vector3(0, 0, 1);
            }
            else
            {
                Destroy(btn.transform.Find("ImgCircle").gameObject);
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }
    private void Upload_InstanziaBottoneSchede(int IDsc, string Nome, int row, Button btnToDuplicate)
    {
        Button btnDuplicato = Instantiate(btnToDuplicate);
        btnDuplicato.name = "SchedaN" + row;
        btnDuplicato.transform.SetParent(btnToDuplicate.transform.parent);
        btnDuplicato.transform.SetSiblingIndex(row + 1);
        Destroy(btnDuplicato.transform.Find("ImgCircle").gameObject);
        btnDuplicato.transform.Find("Text").GetComponent<TextMeshProUGUI>().rectTransform.anchoredPosition = new Vector2(0, 0);
        btnDuplicato.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = Nome;
        btnDuplicato.onClick.AddListener(() => UploadSelectClick(row, true));

        ButtonPressEvent bpe = btnDuplicato.gameObject.AddComponent<ButtonPressEvent>();
        bpe.onPressed.AddListener(() => AppManager.Instance.navManager.NavigateTo(() => StartCoroutine(CaricaDettagli(IDsc, false))));
    }
    private void Upload_ClickSchede()
    {
        int[,] Matrix = iScSel;
        for (int row = 0; row < Matrix.GetLength(0); row++)
        {
            //imposto tutti i valori temporal sbagliati perché poi chiamo la funzione che li cambia e imposta il colore
            if (Matrix[row, 1] == 0)
                Matrix[row, 2] = 1;
            else
                Matrix[row, 2] = 0;
            UploadSelectClick(row, true);
        }

        SwitchCanvas("Schede");
        AppManager.Instance.objects.Button_Area("Answer");
        Globals.BTN_CONFIRM.onClick.RemoveAllListeners();
        Globals.BTN_CONFIRM.onClick.AddListener(() => Upload_ConfirmSelect(true));
        Globals.BTN_CANCEL.onClick.RemoveAllListeners();
        Globals.BTN_CANCEL.onClick.AddListener(AppManager.Instance.navManager.NavigateBack);
    }
    private void Upload_ClickEsercizi()
    {
        int[,] Matrix = iEsSel;
        for (int row = 0; row < Matrix.GetLength(0); row++)
        {
            //imposto tutti i valori temporal sbagliati perché poi chiamo la funzione che li cambia e imposta il colore
            if (Matrix[row, 1] == 0)
                Matrix[row, 2] = 1;
            else
                Matrix[row, 2] = 0;
            UploadSelectClick(row, false);
        }

        GameObject goParent = btnEsercizio.transform.parent.gameObject;
        for (int row = 1; row < goParent.transform.childCount; row++)
        {
            //Reimposto l'altezza dei bottoni, che può essere stata variata dal filter search, come quella del bottone base
            goParent.transform.GetChild(row).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(goParent.transform.GetChild(row).gameObject.GetComponent<RectTransform>().rect.width, goParent.transform.GetChild(0).gameObject.GetComponent<RectTransform>().rect.height);
            //Ricarico le immagini
            CaricaImmaginiEsercizi(lbImmaginiEs[row - 1], goParent.transform.GetChild(row).GetComponent<Button>());
        }
        SwitchCanvas("Esercizi");
        AppManager.Instance.objects.Button_Area("Answer");
        Globals.BTN_CONFIRM.onClick.RemoveAllListeners();
        Globals.BTN_CONFIRM.onClick.AddListener(() => Upload_ConfirmSelect(false));
        Globals.BTN_CANCEL.onClick.RemoveAllListeners();
        Globals.BTN_CANCEL.onClick.AddListener(AppManager.Instance.navManager.NavigateBack);
    }
    private void Upload_ConfirmSelect(bool bSchede)
    {
        int[,] Matrix;
        if (bSchede)
            Matrix = iScSel;
        else
            Matrix = iEsSel;
        for (int row = 0; row < Matrix.GetLength(0); row++)
            //il cambio avviene in questa funzione
            Upload_ControlloSelect(Matrix, row, bSchede);

        AppManager.Instance.navManager.NavigateBack();
    }
    private void Upload_ControlloSelect(int[,] Matrix, int row, bool bScheda)
    {
        List<int> liEsercizi = new List<int>(); ;
        //se la scheda viene attivata, tutti i suoi esercizi vengono attivati
        if (bScheda & Matrix[row, 1] == 0 & Matrix[row, 2] == 1)
        {
            for (int i = 0; i < iS_E.GetLength(0); i++)
            {
                if (iS_E[i, 0] == Matrix[row, 0])
                {
                    bool trovato = false;
                    foreach (int es in liEsercizi)
                    {
                        if (es == iS_E[i, 1])
                            trovato = true;
                    }
                    if (trovato == false)
                    {
                        for (int j = 0; j < iEsSel.GetLength(0); j++)
                        {
                            if (iEsSel[j, 0] == iS_E[i, 1])
                            {
                                iEsSel[j, 1] = 1;
                                break;
                            }
                        }
                        liEsercizi.Add(iS_E[i, 1]);
                    }
                }
            }
            Matrix[row, 1] = Matrix[row, 2];
        }
        //in tutti gli altri casi
        else
        {
            Matrix[row, 1] = Matrix[row, 2];
        }
    }
    private async void Upload_btnDel(int iID, string btnName, bool bScheda)
    {
        int[,] Matrix;
        if (bScheda)
        {
            Matrix = iScSel;
            for (int row = 0; row < Matrix.GetLength(0); row++)
            {
                if (Matrix[row, 0] == iID)
                {
                    Matrix[row, 1] = 0;
                    DestroyImmediate(Globals.CANVAS_WEB.transform.Find("cnvPacchetto/cnvSchede/svSchede/Viewport/Content").transform.Find(btnName).gameObject);
                    break;
                }

            }
            List<int> liIDes = Upload_EliminaEserciziDaScheda(iID);
            if (liIDes.Count > 0)
            {
                //per evitare di mettere tutta la funzione async, però fa uscire il codice dal main thread, devo ancora trovare la soluzione
                //Task.Run(async () =>
                //{
                var DialogResult = await AppManager.Instance.messagebox.ShowMsgBox(1, "Vuoi eliminare anche tutti gli esercizi che compaiono SOLO nella scheda eliminata?\n\rGli esercizi in questione sono: " + liIDes.Count, "Elimina " + liIDes.Count + " esercizi", null, "NON eliminare alcun esercizio");
                if (DialogResult == 1)
                {
                    for (int i = 0; i < liIDes.Count; i++)
                    {
                        for (int j = 0; j < iEsSel.GetLength(0); j++)
                        {
                            if (iEsSel[j, 0] == liIDes[i])
                            {
                                iEsSel[j, 1] = 0;
                                btnName = "Esercizio" + (iEsSel[j, 3] - 1);
                                DestroyImmediate(Globals.CANVAS_WEB.transform.Find("cnvPacchetto/cnvEsercizi/svEsercizi/Viewport/Content").transform.Find(btnName).gameObject);
                                break;
                            }
                        }
                    }
                }
                else
                    Upload_CaricaAnteprima();
                //});
            }
            else
                //mi ricarica l'anteprima per il btnDel degli esercizi
                Upload_CaricaAnteprima();
        }
        else
        {
            Matrix = iEsSel;
            for (int row = 0; row < Matrix.GetLength(0); row++)
            {
                if (Matrix[row, 0] == iID)
                {
                    Matrix[row, 1] = 0;
                    DestroyImmediate(Globals.CANVAS_WEB.transform.Find("cnvPacchetto/cnvEsercizi/svEsercizi/Viewport/Content").transform.Find(btnName).gameObject);
                    break;
                }

            }
        }
    }
    private List<int> Upload_EliminaEserciziDaScheda(int iID)
    {
        //cicli per ottenere una lista degli esercizi che sono contenuti solo nella scheda eliminata (lista con tutti quelli controllati, così evito di cercare più volte lo stesso esercizio)
        List<int> liIDes = new List<int>();
        List<int> liIDesIneliminabili = new List<int>();
        //scorre i dettagli per trovare tutte le volte che compare la scheda
        for (int i = 0; i < iS_E.GetLength(0); i++)
        {
            if (iS_E[i, 0] == iID)
            {
                //controllo che non sia un esercizio già esaminato
                int index = liIDesIneliminabili.IndexOf(iS_E[i, 1]);
                if (index == -1)
                {
                    //sapendo che gli esercizi che hanno una relazione con questa scheda sono già per forza attivi, ciclo nuovamente i dettagli per trovare le relazioni di questi esercizi che non siano con quella scheda (se non ne hanno allora è eliminabile)
                    bool bTrovato = false;
                    for (int j = 0; j < iS_E.GetLength(0); j++)
                    {
                        if (iS_E[j, 1] == iS_E[i, 1] && iS_E[j, 0] != iID)
                        {
                            bTrovato = true;
                            //ciclo le schede per trovare quella che ha la relazione con l'esercizio per controllare se è attiva o no, se lo è l'esercizio non può essere eliminato, altrimenti sì, quindi lo aggiungo alla lista
                            for (int y = 0; y < iScSel.GetLength(0); y++)
                            {
                                if (iScSel[y, 0] == iS_E[j, 0])
                                {
                                    if (iScSel[y, 1] == 1)
                                    {
                                        liIDesIneliminabili.Add(iS_E[i, 1]);
                                        goto RelazioneSuccessiva;
                                    }
                                    else
                                    {
                                        liIDes.Add(iS_E[j, 1]);
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    if (!bTrovato)
                        liIDes.Add(iS_E[i, 1]);
                }
            RelazioneSuccessiva:
                continue;
            }
        }
        //rimuovo doppioni e quelli che non si possono eliminare
        liIDes = liIDes.Distinct().ToList();
        liIDesIneliminabili = liIDesIneliminabili.Distinct().ToList();
        liIDes.RemoveAll(n => liIDesIneliminabili.Contains(n));
        return liIDes;
    }
    #endregion
    #region Schede
    public void Download_MostraSchede(int ID)
    {
        SwitchCanvas("Schede");

        btnDownload("Schede", IDpacchetto);
    }


    public void Download_InstanziaScheda(int num, int ID, string Nome)
    {
        Button btnToDuplicate = btnScheda;
        Button bottone;
        GameObject parent = btnToDuplicate.transform.parent.gameObject;

        bottone = Instantiate(btnToDuplicate);
        bottone.name = "Scheda" + num;
        bottone.transform.SetParent(parent.transform);
        bottone.transform.SetSiblingIndex(num + 1);
        Destroy(bottone.transform.Find("ImgCircle").gameObject);

        bottone.gameObject.SetActive(true);

        bottone.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = Nome;

        bottone.onClick.AddListener(() => AppManager.Instance.navManager.NavigateTo(() => StartCoroutine(CaricaDettagli(ID, true))));
    }
    #endregion
    #region Dettagli
    public IEnumerator CaricaDettagli(int IDsc, bool bWeb)
    {
        Svuota(btnDettaglio.transform.parent.gameObject, 1);

        if (bWeb)
        {
            JSONArray jsonArray = new JSONArray();
            yield return StartCoroutine(AppManager.Instance.dbweb.SelezionaDettagli(IDsc, result =>
            {
                jsonArray = result;
            }));
            for (int i = 0; i < jsonArray.Count; i++)
            {
                AppManager.Instance.web.InstanziaDettaglio(i, jsonArray[i].AsObject["IDdettagli"], /*jsonArray[i].AsObject["Immagine"]*/ null, jsonArray[i].AsObject["NomeEsercizio"],
                    jsonArray[i].AsObject["Serie"], jsonArray[i].AsObject["Rip_NoT"], jsonArray[i].AsObject["RipetizioniN"], jsonArray[i].AsObject["RipetizioniT"],
                    jsonArray[i].AsObject["Recupero"], jsonArray[i].AsObject["Attesa"]);
            }

            btnDownload("Scheda", IDsc);
        }
        else
        {
            var Tuple = AppManager.Instance.sql.SelectAllDettagli(" WHERE det." + Globals.COL_DET_ID_SCHEDA + " = " + IDsc, Globals.COL_DET_POSIZIONE);
            DataTable dtDettagli = Tuple.Item1;
            List<byte[]> arrayList = Tuple.Item2;
            for (int i = 0; i < dtDettagli.Rows.Count; i++)
            {
                AppManager.Instance.web.InstanziaDettaglio(i, (int)dtDettagli.Rows[0][Globals.COL_DET_ID], arrayList[i], dtDettagli.Rows[0][Globals.COL_ESE_NOME].ToString(), (int)dtDettagli.Rows[0][Globals.COL_DET_SERIE], dtDettagli.Rows[0][Globals.COL_DET_RIPnOt].ToString(), (int)dtDettagli.Rows[0][Globals.COL_DET_RIPETIZIONIn], dtDettagli.Rows[0][Globals.COL_DET_RIPETIZIONIt].ToString(), dtDettagli.Rows[0][Globals.COL_DET_RECUPERO].ToString(), dtDettagli.Rows[0][Globals.COL_DET_ATTESA].ToString());
            }
        }

        SwitchCanvas("Dettagli");

    }

    private void InstanziaDettaglio(int num, int ID, byte[] byImmagine, string Nome, int Serie, string NoT, int RepN, string RepT, string Rec, string Attesa)
    {
        Button bottone;
        GameObject parent = btnDettaglio.transform.parent.gameObject;

        bottone = Instantiate(btnDettaglio);
        bottone.name = "Dett" + num;
        bottone.transform.SetParent(parent.transform);
        bottone.transform.SetSiblingIndex(num + 1);

        bottone.gameObject.SetActive(true);

        bottone.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = Nome;
        bottone.transform.Find("cnvDetails/txtSeriesNum").GetComponent<TextMeshProUGUI>().text = Serie.ToString();

        string Rep;
        if (NoT == "N")
            Rep = RepN.ToString();
        else
            Rep = RepT;
        bottone.transform.Find("cnvDetails/txtRepsNum").GetComponent<TextMeshProUGUI>().text = Rep;

        string Recupero;
        if (Serie > 1)
            Recupero = Rec;
        else
            Recupero = Attesa;
        bottone.transform.Find("cnvDetails/txtRecNum").GetComponent<TextMeshProUGUI>().text = Recupero;

        #region Immagine nel bottone
        if (byImmagine != null)
        {
            try
            {
                //Converto in un array di byte la riga n della lista contenente gli array di byte
                byte[] ByteImmagine = byImmagine;
                if (ByteImmagine != null)
                {
                    //Ottengo l'immagine dai byte
                    Image immagine = AppManager.Instance.uploadImage.ConvertByteArrayToImage(ByteImmagine);
                    //Assegno la suddetta immagine al figlio (immagine) del figlio (ImgCircle = immagine circolare per creare la maschera rotonda) del bottone
                    bottone.transform.Find("ImgCircle/Image").GetComponent<Image>().sprite = immagine.sprite;
                    //Gli impogno di preservare il suo aspetto in modo da non ottere l'iimagine strecciata
                    bottone.transform.Find("ImgCircle/Image").GetComponent<Image>().preserveAspect = true;
                    //Riporto l'immagine alle sue dimensioni originali (ma solo per calcolare l'altezza e la laeghezza originale)
                    bottone.transform.Find("ImgCircle/Image").GetComponent<Image>().SetNativeSize();
                    //Ottengo i dati del "rect transform" dell'immagine
                    RectTransform rt = bottone.transform.Find("ImgCircle/Image").GetComponent<Image>().GetComponent<RectTransform>();
                    //Prendo la larghezza e la larghezza
                    double width = rt.sizeDelta.x * rt.localScale.x;
                    double height = rt.sizeDelta.y * rt.localScale.y;
                    //Se la larghezza è maggiore dall'altezza, imposto l'altezza a 100 pixels precisi (quelli del riquadro dell'immagine) mentre la larghezza al massimo che gli possa occorre
                    if (width > height)
                    {
                        bottone.transform.Find("ImgCircle/Image").GetComponent<Image>().rectTransform.sizeDelta = new Vector3(4000, 100, 1);
                    }
                    else
                    {
                        bottone.transform.Find("ImgCircle/Image").GetComponent<Image>().rectTransform.sizeDelta = new Vector3(100, 4000, 1);
                    }
                    //Porto l'immagine nella posizione x = 0, y = 0
                    bottone.transform.Find("ImgCircle/Image").GetComponent<Image>().rectTransform.anchoredPosition = new Vector3(0, 0, 1);
                }
                //Se è nulla distruggo l'immagine
                else
                {
                    Destroy(bottone.transform.Find("ImgCircle").gameObject);
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
        }
        #endregion
    }
    #endregion
    #region Esercizi
    public void Download_MostraEsercizi(int ID)
    {
        SwitchCanvas("Esercizi");

        btnDownload("Esercizi", IDpacchetto);
    }


    public void InstanziaAnteprima(int num, string Nome, bool bScheda, bool btnDel, int iID = 0, bool bAdviceEs = false)
    {
        GameObject parent;
        Button bottone;
        TextMeshProUGUI testo;

        if (bScheda)
        {
            parent = Globals.CANVAS_WEB.transform.Find("cnvPacchetto/cnvSchede/svSchede/Viewport/Content/btnScheda").GetComponent<Button>().transform.parent.gameObject;
            bottone = Instantiate(Globals.CANVAS_WEB.transform.Find("cnvPacchetto/cnvSchede/svSchede/Viewport/Content/btnScheda").GetComponent<Button>());
            testo = bottone.transform.Find("txtScheda").GetComponent<TextMeshProUGUI>();
            bottone.name = "Scheda" + num;

            if (btnDel)
                bottone.transform.Find("btnDel").GetComponent<Button>().onClick.AddListener(() => Upload_btnDel(iID, bottone.name, true));
        }
        else
        {
            parent = Globals.CANVAS_WEB.transform.Find("cnvPacchetto/cnvEsercizi/svEsercizi/Viewport/Content/btnEsercizio").GetComponent<Button>().transform.parent.gameObject;
            bottone = Instantiate(Globals.CANVAS_WEB.transform.Find("cnvPacchetto/cnvEsercizi/svEsercizi/Viewport/Content/btnEsercizio").GetComponent<Button>());
            testo = bottone.transform.Find("txtEsercizio").GetComponent<TextMeshProUGUI>();
            bottone.name = "Esercizio" + num;

            if (btnDel)
                bottone.transform.Find("btnDel").GetComponent<Button>().onClick.AddListener(() => Upload_btnDel(iID, bottone.name, false));
            else if (bAdviceEs)
                bottone.onClick.AddListener(() => StartCoroutine(AppManager.Instance.messagebox.ShowMsg(2, "non puoi eliminare questo esercizio finché è presente una scheda che lo utilizza", 5)));
        }

        bottone.transform.SetParent(parent.transform);
        bottone.transform.SetSiblingIndex(num + 1);
        bottone.gameObject.SetActive(true);
        testo.text = Nome;

        bottone.transform.Find("btnDel").gameObject.SetActive(btnDel);
    }
    IEnumerator Download_InstanziaEsercizio(int num, int ID, string Immagine, string Nome, string Consigli, string Note)
    {
        Button btnToDuplicate = btnEsercizio;
        Button bottone;
        GameObject parent = btnToDuplicate.transform.parent.gameObject;

        bottone = Instantiate(btnToDuplicate);
        bottone.name = "Esercizio" + num;
        bottone.transform.SetParent(parent.transform);
        bottone.transform.SetSiblingIndex(num + 1);

        bottone.gameObject.SetActive(true);

        bottone.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = Nome;

        JSONArray jsMuscoli = new JSONArray();
        yield return StartCoroutine(AppManager.Instance.dbweb.SelezionaMuscoli(ID, result =>
        {
            jsMuscoli = result;
        }));

        TextMeshProUGUI txtMuscoli = bottone.transform.Find("txtMuscles").GetComponent<TextMeshProUGUI>();
        List<string> liMuscoli = new List<string>();

        txtMuscoli.text = "";

        if (jsMuscoli != null && jsMuscoli.Count > 0)
        {
            for (int i = 0; i < jsMuscoli.Count; i++)
            {
                string sMuscolo;
                try
                {
                    sMuscolo = AppManager.Instance.sql.SelectAllMuscoli(" WHERE " + Globals.COL_MUS_ID + " = " + jsMuscoli[i].AsObject["IDmuscolo"], Globals.COL_MUS_ID).Rows[0][Globals.COL_MUS_NOME].ToString();
                }
                catch
                {
                    sMuscolo = "ID muscolo: " + jsMuscoli[i].AsObject["IDmuscolo"] + " non trovato";
                }
                txtMuscoli.text += sMuscolo + ", ";
                liMuscoli.Add(sMuscolo);
            }
            txtMuscoli.text = txtMuscoli.text.Substring(0, txtMuscoli.text.Length - 2);
        }
        else
        {
            txtMuscoli.text = "";
        }

        bottone.onClick.AddListener(() => AppManager.Instance.navManager.NavigateTo(() => MostraEsercizio(ID, Nome, Consigli, Note, Immagine, liMuscoli)));
    }

    private void Svuota(GameObject goParent, int iSave)
    {
        {
            while (goParent.transform.childCount > iSave)
            {
                DestroyImmediate(goParent.transform.GetChild(goParent.transform.childCount - 1).gameObject);
            }
        }
    }
    #endregion
    #region Esercizio
    private void MostraEsercizio(int ID, string Nome, string Consigli, string Note, string Immagine, List<string> liMuscoli)
    {
        Canvas Esercizio = Globals.CANVAS_WEB.transform.Find("cnvEsercizio/cnvEs").GetComponent<Canvas>();

        Esercizio.transform.Find("txtName").GetComponent<TextMeshProUGUI>().text = Nome;
        Esercizio.transform.Find("txtAdvice").GetComponent<TextMeshProUGUI>().text = Consigli;
        Esercizio.transform.Find("txtNote").GetComponent<TextMeshProUGUI>().text = Note;

        TextMeshProUGUI txtMuscoli = Esercizio.transform.Find("txtMuscles").GetComponent<TextMeshProUGUI>();
        txtMuscoli.text = "";
        if (liMuscoli.Count > 0)
        {
            for (int i = 0; i < liMuscoli.Count; i++)
            {
                txtMuscoli.text += liMuscoli[i] + ", ";
            }
            txtMuscoli.text = txtMuscoli.text.Substring(0, txtMuscoli.text.Length - 2);
        }
        else
        {
            txtMuscoli.text = "";
        }

        SwitchCanvas("Esercizio");

        btnDownload("Esercizio", ID);
    }
    #endregion



}
