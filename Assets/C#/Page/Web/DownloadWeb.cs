using SimpleJSON;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

public class DownloadWeb : MonoBehaviour
{
    #region Costanti
    private const string WEB_INS_IMG = "https://your-server.example/gym-api/InsertImage.php";
    private const string WEB_GET_IMG = "https://your-server.example/gym-api/GetImage.php";

    private const string WEB_GETALL_PAC = "https://your-server.example/gym-api/GetPacchetti.php";

    private const string WEB_GET_PAC = "https://your-server.example/gym-api/GetPacchetto.php";

    private const string WEB_GET_MUS = "https://your-server.example/gym-api/GetMuscoli.php";


    private const string WEB_GET_ES = "https://your-server.example/gym-api/GetEsercizio.php";
    private const string WEB_GET_SC = "https://your-server.example/gym-api/GetScheda.php";
    private const string WEB_GET_DET = "https://your-server.example/gym-api/GetDettaglio.php";

    private const string WEB_GET_ALL_ES = "https://your-server.example/gym-api/GetEsercizi.php";
    private const string WEB_GET_ALL_SC = "https://your-server.example/gym-api/GetSchede.php";
    private const string WEB_GET_ALL_DET = "https://your-server.example/gym-api/GetDettagli.php";
    #endregion
    #region Variabili Globali
    int[,] mtrEsercizi;
    int[,] mtrSchede;

    static bool bDownloadAllEs;
    static bool bDownloadAllSc;
    static bool bDownloadEs;
    static bool bDownloadSc;

    static bool bMBchecked;
    static int iMBchecked;

    static bool bPpLoad;
    #endregion
    public JsonWeb jsl;
    static string AllOrSchede;
    private void Start()
    {
        jsl = new JsonWeb();
    }
    #region Download All
    public void DownloadAll(int IDpac)
    {
        ppTesto("Caricamento...");
        ppPerc(0, 10);
        ppShow();

        AllOrSchede = "All";
        StartCoroutine(AllEsercizi(IDpac));
    }
    private async void FinitoDownloadAllJson()
    {
        if (AllOrSchede == "All")
        {
            JsonEsercizio[] jEsercizi = jsl.LoadJsonEsercizi();
            JsonScheda[] jSchede = jsl.LoadJsonSchede();
            JsonDettaglio[] jDettagli = jsl.LoadJsonDettagli();
            JsonM_E[] jMuscoli = jsl.LoadJsonM_E();

            mtrEsercizi = new int[jEsercizi.Length, 2];
            mtrSchede = new int[jSchede.Length, 2];

            ppTesto("Dowload Main.Instance.esercizi...");

            bMBchecked = false;
            int num = 0;
            foreach (JsonEsercizio jesercizio in jEsercizi)
            {
                ppPerc(num, mtrEsercizi.Length);

                mtrEsercizi[num, 0] = jesercizio.IDesercizio;
                mtrEsercizi[num, 1] = await InsertEsercizio(jesercizio.IDesercizio, jesercizio.NomeEsercizio, jesercizio.Note, jesercizio.Consigli, jMuscoli);
                num += 1;
            }

            ppTesto("Donwload Main.Instance.schede...");

            bMBchecked = false;
            num = 0;
            foreach (JsonScheda scheda in jSchede)
            {
                ppPerc(num, mtrSchede.Length);

                mtrSchede[num, 0] = scheda.IDscheda;
                mtrSchede[num, 1] = await InsertScheda(num, scheda.NomeScheda);
                num += 1;
            }

            ppTesto("Donwload dettaglio Main.Instance.schede...");

            int IDes;
            int IDsc;
            foreach (JsonDettaglio dettaglio in jDettagli)
            {
                ppPerc(num, jDettagli.Length);

                num = 0;
                while (1 == 1)
                {
                    if (mtrEsercizi[num, 0] == dettaglio.IDesercizio)
                    {
                        IDes = mtrEsercizi[num, 1];
                        break;
                    }
                    num += 1;
                }
                num = 0;
                while (1 == 1)
                {
                    if (mtrSchede[num, 0] == dettaglio.IDscheda)
                    {
                        IDsc = mtrSchede[num, 1];
                        break;
                    }
                    num += 1;
                }
                if (IDsc != 0)
                    AppManager.Instance.sql.InsertDettagli(dettaglio.Serie, dettaglio.Rip_NoT, dettaglio.RipetizioniN, dettaglio.RipetizioniT, dettaglio.Recupero, dettaglio.Attesa,
                        dettaglio.Posizione, IDes, IDsc);
            }
            ppClose();
            await AppManager.Instance.messagebox.ShowMsgBox(1, "Dowload finito!", null, "OK", null);
        }
        else
        {
            FinitoDownloadSchede();
        }
    }
    #endregion
    #region Download
    #region Esercizi
    public void DownloadEsercizi(int IDpac)
    {
        ppTesto("Caricamento...");
        ppPerc(0, 10);
        ppShow();
        StartCoroutine(Esercizi(IDpac));
    }
    private async void FinitoDownloadEsercizi()
    {
        JsonEsercizio[] jEsercizi = jsl.LoadJsonEsercizi();
        JsonM_E[] jMuscoli = jsl.LoadJsonM_E();

        mtrEsercizi = new int[jEsercizi.Length, 2];

        ppTesto("Dowload Main.Instance.esercizi...");

        bMBchecked = false;
        int num = 0;
        foreach (JsonEsercizio jEsercizio in jEsercizi)
        {
            ppPerc(num, mtrEsercizi.Length);

            mtrEsercizi[num, 0] = jEsercizio.IDesercizio;
            mtrEsercizi[num, 1] = await InsertEsercizio(jEsercizio.IDesercizio, jEsercizio.NomeEsercizio, jEsercizio.Note, jEsercizio.Consigli, jMuscoli);
            num += 1;
        }

        ppClose();
        await AppManager.Instance.messagebox.ShowMsgBox(1, "Dowload finito!", null, "OK", null);
    }
    #endregion
    #region Schede
    public void DownloadSchede(int IDpac)
    {
        ppTesto("Caricamento...");
        ppPerc(0, 10);
        ppShow();

        AllOrSchede = "Schede";
        StartCoroutine(AllEsercizi(IDpac));
    }
    private async void FinitoDownloadSchede()
    {
        JsonEsercizio[] jEsercizi = jsl.LoadJsonEsercizi();
        JsonScheda[] jSchede = jsl.LoadJsonSchede();
        JsonDettaglio[] jDettagli = jsl.LoadJsonDettagli();
        JsonM_E[] jMuscoli = jsl.LoadJsonM_E();

        mtrEsercizi = new int[jEsercizi.Length, 2];
        mtrSchede = new int[jSchede.Length, 2];

        ppTesto("Donwload Main.Instance.schede...");

        bMBchecked = false;
        int num = 0;
        foreach (JsonScheda scheda in jSchede)
        {
            ppPerc(num, mtrSchede.Length);

            mtrSchede[num, 0] = scheda.IDscheda;
            mtrSchede[num, 1] = await InsertScheda(num, scheda.NomeScheda);
            num += 1;
        }

        ppTesto("Donwload dettaglio Main.Instance.schede...");

        int IDes;
        int IDsc;
        int numDet = 0;
        List<int[]> listEsercizi = new List<int[]>();
        foreach (JsonDettaglio jDettaglio in jDettagli)
        {
            ppPerc(num, jDettagli.Length);

            num = 0;
            while (1 == 1)
            {
                if (mtrSchede[num, 0] == jDettaglio.IDscheda)
                {
                    IDsc = mtrSchede[num, 1];
                    break;
                }
                num += 1;
            }
            if (IDsc != 0)
            {
                bool Trovato = false;
                for (int row = 0; row < listEsercizi.Count; row++)
                {
                    if (listEsercizi[row][0] == jDettaglio.IDesercizio)
                    {
                        AppManager.Instance.sql.InsertDettagli(jDettaglio.Serie, jDettaglio.Rip_NoT, jDettaglio.RipetizioniN, jDettaglio.RipetizioniT, jDettaglio.Recupero, jDettaglio.Attesa,
                                jDettaglio.Posizione, listEsercizi[row][1], IDsc);
                        Trovato = true;
                    }
                }

                if (Trovato == false)
                {
                    foreach (JsonEsercizio jEsercizio in jEsercizi)
                    {
                        if (jDettaglio.IDesercizio == jEsercizio.IDesercizio)
                        {
                            IDes = await InsertEsercizio(jEsercizio.IDesercizio, jEsercizio.NomeEsercizio, jEsercizio.Note, jEsercizio.Consigli, jMuscoli);

                            int[] es = { jEsercizio.IDesercizio, IDes };
                            listEsercizi.Add(es);

                            AppManager.Instance.sql.InsertDettagli(jDettaglio.Serie, jDettaglio.Rip_NoT, jDettaglio.RipetizioniN, jDettaglio.RipetizioniT, jDettaglio.Recupero, jDettaglio.Attesa,
                                jDettaglio.Posizione, IDes, IDsc);
                            numDet += 1;
                            break;
                        }
                    }
                }
            }
        }
        ppClose();
        await AppManager.Instance.messagebox.ShowMsgBox(1, "Dowload finito!", null, "OK", null);
    }
    #endregion
    #region Scheda
    public void DownloadScheda(int IDscheda)
    {
        ppTesto("Caricamento...");
        ppPerc(0, 10);
        ppShow();

        StartCoroutine(Scheda(IDscheda));
    }
    #endregion
    #region Esercizio
    public void DownloadEsercizio(int IDes)
    {
        ppTesto("Caricamento...");
        ppPerc(0, 10);
        ppShow();
        StartCoroutine(Esercizio(IDes));
    }
    #endregion
    #endregion

    #region IEnumerator All
    IEnumerator AllEsercizi(int IDpac)
    {
        WWWForm form = new WWWForm();
        form.AddField("IDpacchetto", IDpac);

        using (UnityWebRequest www = UnityWebRequest.Post(WEB_GET_ALL_ES, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                string sjsonArray = www.downloadHandler.text;
                JSONArray jsonArray = JSON.Parse(sjsonArray) as JSONArray;

                int IDes;
                string Nome;
                string Note;
                string Consigli;

                JsonEsercizio[] es = new JsonEsercizio[jsonArray.Count];

                int IDme;
                int IDmus;

                List<JsonM_E> me_List = new List<JsonM_E>();

                for (int i = 0; i < jsonArray.Count; i++)
                {
                    IDes = jsonArray[i].AsObject["IDesercizio"];
                    Nome = jsonArray[i].AsObject["NomeEsercizio"];
                    Note = jsonArray[i].AsObject["Note"];
                    Consigli = jsonArray[i].AsObject["Consigli"];

                    es[i] = new JsonEsercizio(IDes, Nome, Note, Consigli, null);

                    IDme = jsonArray[i].AsObject["IDme"];
                    IDmus = jsonArray[i].AsObject["IDmuscolo"];

                    bool Trovato = false;
                    for (int a = 0; a < me_List.Count; a++)
                    {
                        if (me_List[a].IDme == IDme)
                        {
                            Trovato = true;
                            break;
                        }
                    }
                    if (Trovato == false)
                        me_List.Add(new JsonM_E(IDme, IDmus, IDes));
                }
                jsl.SaveEsercizi(es);

                JsonM_E[] m_e = new JsonM_E[me_List.Count];
                for (int a = 0; a < me_List.Count; a++)
                {
                    m_e[a] = me_List[a];
                }

                jsl.SaveM_E(m_e);
            }
        }
        StartCoroutine(AllSchede(IDpac));
    }
    IEnumerator AllSchede(int IDpac)
    {
        WWWForm form = new WWWForm();
        form.AddField("IDpacchetto", IDpac);

        using (UnityWebRequest www = UnityWebRequest.Post(WEB_GET_ALL_SC, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                string sjsonArray = www.downloadHandler.text;
                JSONArray jsonArray = JSON.Parse(sjsonArray) as JSONArray;

                int ID;
                string Nome;

                JsonScheda[] sc = new JsonScheda[jsonArray.Count];

                for (int i = 0; i < jsonArray.Count; i++)
                {
                    ID = jsonArray[i].AsObject["IDscheda"];
                    Nome = jsonArray[i].AsObject["NomeScheda"];

                    sc[i] = new JsonScheda(ID, Nome);
                }
                jsl.SaveSchede(sc);
            }
        }
        StartCoroutine(AllDettagli(IDpac));
    }
    IEnumerator AllDettagli(int IDpacchetto)
    {
        WWWForm form = new WWWForm();
        form.AddField("IDpacchetto", IDpacchetto);

        using (UnityWebRequest www = UnityWebRequest.Post(WEB_GET_ALL_DET, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                string sjsonArray = www.downloadHandler.text;
                JSONArray jsonArray = JSON.Parse(sjsonArray) as JSONArray;

                int IDdet;
                int Serie;
                string NoT;
                int RepN;
                string RepT;
                string Rec;
                string Att;
                int Pos;
                int IDes;
                int IDsc;

                JsonDettaglio[] det = new JsonDettaglio[jsonArray.Count];

                for (int i = 0; i < jsonArray.Count; i++)
                {
                    IDdet = jsonArray[i].AsObject["IDdettagli"];
                    Serie = jsonArray[i].AsObject["Serie"];
                    NoT = jsonArray[i].AsObject["Rip_NoT"];
                    RepN = jsonArray[i].AsObject["RipetizioniN"];
                    RepT = jsonArray[i].AsObject["RipetizioniT"];
                    Rec = jsonArray[i].AsObject["Recupero"];
                    Att = jsonArray[i].AsObject["Attesa"];
                    Pos = jsonArray[i].AsObject["Posizione"];
                    IDes = jsonArray[i].AsObject["IDesercizio"];
                    IDsc = jsonArray[i].AsObject["IDscheda"];

                    det[i] = new JsonDettaglio(IDdet, Serie, NoT, RepN, RepT, Rec, Att, Pos, IDes, IDsc);
                }
                jsl.SaveDettagli(det);
            }
        }
        FinitoDownloadAllJson();
    }
    #endregion
    #region IEnumerator Esercizi
    IEnumerator Esercizi(int IDpac)
    {
        WWWForm form = new WWWForm();
        form.AddField("IDpacchetto", IDpac);

        using (UnityWebRequest www = UnityWebRequest.Post(WEB_GET_ALL_ES, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                string sjsonArray = www.downloadHandler.text;
                JSONArray jsonArray = JSON.Parse(sjsonArray) as JSONArray;

                int IDes;
                string Nome;
                string Note;
                string Consigli;

                JsonEsercizio[] es = new JsonEsercizio[jsonArray.Count];

                int IDme;
                int IDmus;

                List<JsonM_E> me_List = new List<JsonM_E>();

                for (int i = 0; i < jsonArray.Count; i++)
                {
                    IDes = jsonArray[i].AsObject["IDesercizio"];
                    Nome = jsonArray[i].AsObject["NomeEsercizio"];
                    Note = jsonArray[i].AsObject["Note"];
                    Consigli = jsonArray[i].AsObject["Consigli"];

                    es[i] = new JsonEsercizio(IDes, Nome, Note, Consigli, null);

                    IDme = jsonArray[i].AsObject["IDme"];
                    IDmus = jsonArray[i].AsObject["IDmuscolo"];

                    bool Trovato = false;
                    for (int a = 0; a < me_List.Count; a++)
                    {
                        if (me_List[a].IDme == IDme)
                        {
                            Trovato = true;
                            break;
                        }
                    }
                    if (Trovato == false)
                        me_List.Add(new JsonM_E(IDme, IDmus, IDes));
                }
                jsl.SaveEsercizi(es);

                JsonM_E[] m_e = new JsonM_E[me_List.Count];
                for (int a = 0; a < me_List.Count; a++)
                {
                    m_e[a] = me_List[a];
                }

                jsl.SaveM_E(m_e);
            }
        }
        FinitoDownloadEsercizi();
    }
    #endregion
    #region IEnumerator Scheda
    IEnumerator Scheda(int IDsch)
    {
        WWWForm form = new WWWForm();
        form.AddField("IDscheda", IDsch);

        using (UnityWebRequest www = UnityWebRequest.Post(WEB_GET_SC, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                string sjsonArray = www.downloadHandler.text;
                JSONArray jsonArray = JSON.Parse(sjsonArray) as JSONArray;

                int ID;
                string Nome;

                JsonScheda[] sc = new JsonScheda[jsonArray.Count];

                for (int i = 0; i < jsonArray.Count; i++)
                {
                    ID = jsonArray[i].AsObject["IDscheda"];
                    Nome = jsonArray[i].AsObject["NomeScheda"];

                    sc[i] = new JsonScheda(ID, Nome);
                }
                jsl.SaveSchede(sc);
            }
        }
        StartCoroutine(Scheda_DettagliEsercizi(IDsch));
    }
    IEnumerator Scheda_DettagliEsercizi(int IDscheda)
    {
        WWWForm form = new WWWForm();
        form.AddField("IDscheda", IDscheda);

        using (UnityWebRequest www = UnityWebRequest.Post(WEB_GET_DET, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                string sjsonArray = www.downloadHandler.text;
                JSONArray jsonArray = JSON.Parse(sjsonArray) as JSONArray;

                int IDdet;
                int Serie;
                string NoT;
                int RepN;
                string RepT;
                string Rec;
                string Att;
                int Pos;
                int IDes;
                int IDsc;

                JsonDettaglio[] det = new JsonDettaglio[jsonArray.Count];

                string Nome;
                string Note;
                string Consigli;

                JsonEsercizio[] es = new JsonEsercizio[jsonArray.Count];

                int IDme;
                int IDmus;
                List<JsonM_E> me_List = new List<JsonM_E>();


                for (int i = 0; i < jsonArray.Count; i++)
                {
                    IDdet = jsonArray[i].AsObject["IDdettagli"];
                    Serie = jsonArray[i].AsObject["Serie"];
                    NoT = jsonArray[i].AsObject["Rip_NoT"];
                    RepN = jsonArray[i].AsObject["RipetizioniN"];
                    RepT = jsonArray[i].AsObject["RipetizioniT"];
                    Rec = jsonArray[i].AsObject["Recupero"];
                    Att = jsonArray[i].AsObject["Attesa"];
                    Pos = jsonArray[i].AsObject["Posizione"];
                    IDes = jsonArray[i].AsObject["IDesercizio"];
                    IDsc = jsonArray[i].AsObject["IDscheda"];

                    det[i] = new JsonDettaglio(IDdet, Serie, NoT, RepN, RepT, Rec, Att, Pos, IDes, IDsc);

                    Nome = jsonArray[i].AsObject["NomeEsercizio"];
                    Note = jsonArray[i].AsObject["Note"];
                    Consigli = jsonArray[i].AsObject["Consigli"];

                    es[i] = new JsonEsercizio(IDes, Nome, Note, Consigli, null);

                    IDme = jsonArray[i].AsObject["IDme"];
                    IDmus = jsonArray[i].AsObject["IDmuscolo"];

                    bool Trovato = false;
                    for (int a = 0; a < me_List.Count; a++)
                    {
                        if (me_List[a].IDme == IDme)
                        {
                            Trovato = true;
                            break;
                        }
                    }
                    if (Trovato == false)
                        me_List.Add(new JsonM_E(IDme, IDmus, IDes));
                }
                jsl.SaveEsercizi(es);
                jsl.SaveDettagli(det);

                JsonM_E[] m_e = new JsonM_E[me_List.Count];
                for (int a = 0; a < me_List.Count; a++)
                {
                    m_e[a] = me_List[a];
                }

                jsl.SaveM_E(m_e);
            }
        }
        FinitoDownloadSchede();
    }
    #endregion
    #region IEnumerator Esercizio
    IEnumerator Esercizio(int IDes)
    {
        WWWForm form = new WWWForm();
        form.AddField("IDesercizio", IDes);

        using (UnityWebRequest www = UnityWebRequest.Post(WEB_GET_ES, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                string sjsonArray = www.downloadHandler.text;
                JSONArray jsonArray = JSON.Parse(sjsonArray) as JSONArray;

                int ID;
                string Nome;
                string Note;
                string Consigli;

                JsonEsercizio[] es = new JsonEsercizio[1];

                int IDme;
                int IDmus;
                List<JsonM_E> me_List = new List<JsonM_E>();

                for (int i = 0; i < jsonArray.Count; i++)
                {
                    ID = jsonArray[i].AsObject["IDesercizio"];
                    Nome = jsonArray[i].AsObject["NomeEsercizio"];
                    Note = jsonArray[i].AsObject["Note"];
                    Consigli = jsonArray[i].AsObject["Consigli"];

                    es[0] = new JsonEsercizio(ID, Nome, Note, Consigli, null);

                    IDme = jsonArray[i].AsObject["IDme"];
                    IDmus = jsonArray[i].AsObject["IDmuscolo"];

                    bool Trovato = false;
                    for (int a = 0; a < me_List.Count; a++)
                    {
                        if (me_List[a].IDme == IDme)
                        {
                            Trovato = true;
                            break;
                        }
                    }
                    if (Trovato == false)
                        me_List.Add(new JsonM_E(IDme, IDmus, IDes));
                }
                jsl.SaveEsercizi(es);

                JsonM_E[] m_e = new JsonM_E[me_List.Count];
                for (int a = 0; a < me_List.Count; a++)
                {
                    m_e[a] = me_List[a];
                }

                jsl.SaveM_E(m_e);
            }
        }
        FinitoDownloadEsercizi();
    }
    #endregion

    #region Insert
    private async Task<int> InsertEsercizio(int IDes, string Nome, string Note, string Consigli, JsonM_E[] muscoli)
    {
        DataTable dtEs = AppManager.Instance.sql.SelectAllEsercizi(" WHERE " + Globals.COL_ESE_NOME + " = '" + Nome + "'", Globals.COL_ESE_NOME).Item1;

        int ID;

        if (dtEs.Rows.Count == 0)
        {
            ID = AppManager.Instance.sql.InsertEsercizio(Nome, Note, Consigli, null);
            foreach (JsonM_E muscolo in muscoli)
            {
                if (muscolo.IDesercizio == ID)
                    AppManager.Instance.sql.InsertM_E(IDes, muscolo.IDmuscolo);
            }
        }
        else
        {
            int Risultato;
            if (bMBchecked == false)
            {
                ppPos(false);
                var DialogResult = await AppManager.Instance.messagebox.ShowMsgBox(1, "Hai già un esercizio con la seguente denominazione:\n\r" + Nome + "\n\rCome vuoi procedere?", "è lo stesso esercizio", "sovrascrivilo", "è un altro esercizio", true);
                ppPos(true);
                Risultato = DialogResult;
                if (AppManager.Instance.messagebox.tglCheck.isOn)
                {
                    bMBchecked = true;
                    iMBchecked = Risultato;
                }
            }
            else
            {
                Risultato = iMBchecked;
            }
            if (Risultato == 1)
            {
                ID = int.Parse(dtEs.Rows[0][Globals.COL_ESE_ID].ToString());
            }
            else
            {
                if (Risultato == 2)
                {
                    ID = int.Parse(dtEs.Rows[0][Globals.COL_ESE_ID].ToString());
                    AppManager.Instance.sql.UpdateEsercizio(ID, Nome, Note, Consigli, null);
                }
                else
                {
                    ID = AppManager.Instance.sql.InsertEsercizio(Nome, Note, Consigli, null);
                    foreach (JsonM_E muscolo in muscoli)
                    {
                        if (muscolo.IDesercizio == IDes)
                            AppManager.Instance.sql.InsertM_E(ID, muscolo.IDmuscolo);
                    }
                }
            }
        }
        return ID;
    }
    private async Task<int> InsertScheda(int Row, string Nome)
    {
        DataTable dtSc = AppManager.Instance.sql.SelectAllSchede(" WHERE " + Globals.COL_SCH_NOME + " = '" + Nome + "'", Globals.COL_SCH_NOME);

        int ID;

        if (dtSc.Rows.Count == 0)
            ID = AppManager.Instance.sql.InsertScheda(Nome);
        else
        {
            int Risultato;
            if (bMBchecked == false)
            {
                ppPos(false);
                var DialogResult = await AppManager.Instance.messagebox.ShowMsgBox(1, "Hai già una scheda con la seguente denominazione:\n\r" + Nome + "\n\rCome vuoi procedere?", "Inserisci comunque", null, "Non inserire");
                ppPos(true);
                Risultato = DialogResult;
                if (AppManager.Instance.messagebox.tglCheck.isOn)
                {
                    bMBchecked = true;
                    iMBchecked = Risultato;
                }
            }
            else
            {
                Risultato = iMBchecked;
            }

            if (Risultato == 1)
            {
                ID = AppManager.Instance.sql.InsertScheda(Nome);
            }
            else
            {
                ID = 0;
            }
        }
        return ID;
    }
    #endregion

    #region Popup Load
    private void ppShow()
    {
        Globals.CANVAS_WEB.transform.Find("ppLoad").GetComponent<Canvas>().gameObject.SetActive(true);
        bPpLoad = false;
    }
    private void ppPerc(int n, int Total)
    {
        float perc = ((n) * 100) / Total;
        if (perc > 100)
            perc = 100;

        Globals.CANVAS_WEB.transform.Find("ppLoad").GetComponent<Canvas>().gameObject.SetActive(true);

        Image bar = Globals.CANVAS_WEB.transform.Find("ppLoad/RadialProgressBar").GetComponent<Image>().
            transform.Find("Fill").GetComponent<Image>();
        bar.fillAmount = perc / 100;

        TextMeshProUGUI txt = Globals.CANVAS_WEB.transform.Find("ppLoad/txtPerc").GetComponent<TextMeshProUGUI>();
        txt.text = Math.Round(perc, 0) + "%";

        bPpLoad = true;
        StartCoroutine(ppLoad(n, Total, bar, txt));
    }
    private void ppPos(bool Center)
    {
        if (Center == true)
        {
            Globals.CANVAS_WEB.transform.Find("ppLoad").GetComponent<Canvas>().GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            Globals.CANVAS_WEB.transform.Find("ppLoad/RadialProgressBar").GetComponent<Image>().GetComponent<RectTransform>().sizeDelta = new Vector2(400, 400);
            Globals.CANVAS_WEB.transform.Find("ppLoad/txtPerc").GetComponent<TextMeshProUGUI>().fontSize = 150;
            Globals.CANVAS_WEB.transform.Find("ppLoad").GetComponent<Canvas>().transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            Globals.CANVAS_WEB.transform.Find("ppLoad").GetComponent<Canvas>().GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 330);
            Globals.CANVAS_WEB.transform.Find("ppLoad&RadialProgressBar").GetComponent<Image>().GetComponent<RectTransform>().sizeDelta = new Vector2(250, 250);
            Globals.CANVAS_WEB.transform.Find("ppLoad/txtPerc").GetComponent<TextMeshProUGUI>().fontSize = 80;
            Globals.CANVAS_WEB.transform.Find("ppLoad").GetComponent<Canvas>().transform.GetChild(0).gameObject.SetActive(false);
            bPpLoad = false;
        }
    }
    private void ppClose()
    {
        Globals.CANVAS_WEB.transform.Find("ppLoad").GetComponent<Canvas>().gameObject.SetActive(false);
        bPpLoad = false;
    }
    private void ppTesto(string descrizione)
    {
        Globals.CANVAS_WEB.transform.Find("ppLoad/txtDesc").GetComponent<TextMeshProUGUI>().text = descrizione;
    }
    IEnumerator ppLoad(int n, int Total, Image bar, TextMeshProUGUI txt)
    {
        float Now = ((n + 1) * 100) / Total;
        float Next;
        if (bMBchecked == false)
            Next = ((n + 2) * 100) / Total;
        else
            Next = 100;

        float Distance = Next - Now;
        while (bPpLoad == true)
        {
            Thread.Sleep(250);
            Distance /= 2;
            Now += Distance;
            if (Now > 100)
                Now = 100;
            bar.fillAmount = Now / 100;
            txt.text = Math.Round(Now, 0) + "%";
            yield return null;
        }
    }
    #endregion
}