using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class dbWeb : MonoBehaviour
{
    private const string WEB_INS_IMG = "https://your-server.example/gym-api/InsertImage.php";
    private const string WEB_GET_IMG = "https://your-server.example/gym-api/GetImage.php";

    private const string WEB_GETALL_PAC = "https://your-server.example/gym-api/GetPacchetti.php";

    private const string WEB_GETFIL_PAC = "https://your-server.example/gym-api/GetPacchettiFiltrati.php";

    private const string WEB_GET_PAC = "https://your-server.example/gym-api/GetPacchetto.php";
    private const string WEB_GET_ES = "https://your-server.example/gym-api/GetEsercizi.php";
    private const string WEB_GET_SC = "https://your-server.example/gym-api/GetSchede.php";
    private const string WEB_GET_MUS = "https://your-server.example/gym-api/GetMuscoli.php";
    private const string WEB_GET_DET = "https://your-server.example/gym-api/GetDettaglio.php";

    private const string WEB_COUNT_SC = "https://your-server.example/gym-api/CountSchede.php";
    private const string WEB_COUNT_ES = "https://your-server.example/gym-api/CountEsercizi.php";

    private bool CheckInternetConnection()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            // Non c'è una connessione a Internet, gestisci l'errore qui
            Debug.Log("Errore: nessuna connessione a Internet.");
            return false;
        }
        return true;
    }
    private async void NoInternetConnection()
    {
        var DialogResult = await AppManager.Instance.messagebox.ShowMsgBox(1, "Nessuna connessione a internet", "Riprova", null, "Torna al menù principale");
        if (DialogResult == 1)
        {
            AppManager.Instance.navManager.RetryNavigation();
        }
        else
        {
            AppManager.Instance.objects.BtnHome();
        }
    }
    public IEnumerator SelezionaPacchetti(int iLimit, int iOffSet)
    {
        if (!CheckInternetConnection())
        {
            NoInternetConnection();
            yield break;
        }

        int retryCount = 0;

        WWWForm form = new WWWForm();
        form.AddField("Limit", iLimit);
        form.AddField("OffSet", iOffSet);

        while (retryCount < 2)
        {
            using (UnityWebRequest www = UnityWebRequest.Post(WEB_GETALL_PAC, form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    retryCount++;
                    Debug.Log(www.error + " - Tentativo #" + retryCount);
                    continue;
                }
                else
                {
                    string sjsonArray = www.downloadHandler.text;
                    JSONArray jsonArray = JSON.Parse(sjsonArray) as JSONArray;

                    if (jsonArray == null)
                    {
                        AppManager.Instance.web.DisattivaBtnCarica();
                    }
                    else
                    {
                        for (int i = 0; i < jsonArray.Count; i++)
                        {
                            AppManager.Instance.web.InstanziaPacchetto(jsonArray[i].AsObject["IDpacchetto"], jsonArray[i].AsObject["NomePacchetto"], jsonArray[i].AsObject["NomeUtente"], jsonArray[i].AsObject["Versione"]);
                        }

                        if (jsonArray.Count < iLimit)
                            AppManager.Instance.web.DisattivaBtnCarica();
                    }
                    AppManager.Instance.web.PosLastRow();
                    //break che esce da retryCount
                    break;
                }
            }
        }
        if (retryCount == 2) // se il secondo tentativo ha fallito
        {
            Debug.Log("TENTATIVO #2 FALLITO.");
        }
    }

    public IEnumerator FiltraPacchetti(string sText, int iLimit, int iOffSet)
    {
        if (!CheckInternetConnection())
        {
            NoInternetConnection();
            yield break;
        }

        int retryCount = 0;

        sText = sText.Replace("\u200B", "");

        WWWForm form = new WWWForm();
        form.AddField("Limit", iLimit);
        form.AddField("OffSet", iOffSet);
        form.AddField("Descrizione", sText);
        form.AddField("Gigi", "o");

        while (retryCount < 2)
        {
            using (UnityWebRequest www = UnityWebRequest.Post(WEB_GETFIL_PAC, form))
            {
                yield return www.SendWebRequest();
                string sStampa = www.downloadHandler.text;
                if (www.result != UnityWebRequest.Result.Success)
                {
                    retryCount++;
                    Debug.Log(www.error + " - Tentativo #" + retryCount);
                    continue;
                }
                else
                {

                    string sjsonArray = www.downloadHandler.text;
                    JSONArray jsonArray = JSON.Parse(sjsonArray) as JSONArray;

                    if (jsonArray == null)
                    {
                        AppManager.Instance.web.DisattivaBtnCarica();
                    }
                    else
                    {
                        for (int i = 0; i < jsonArray.Count; i++)
                        {
                            AppManager.Instance.web.InstanziaPacchetto(jsonArray[i].AsObject["IDpacchetto"], jsonArray[i].AsObject["NomePacchetto"], jsonArray[i].AsObject["NomeUtente"], jsonArray[i].AsObject["Versione"]);
                        }

                        if (jsonArray.Count < iLimit)
                            AppManager.Instance.web.DisattivaBtnCarica();
                    }
                    AppManager.Instance.web.PosLastRow();
                    //break che esce da retryCount
                    break;
                }
            }
        }
        if (retryCount == 2) // se il secondo tentativo ha fallito
        {
            Debug.Log("TENTATIVO #2 FALLITO.");
        }
    }
    public IEnumerator CountSchede(int ID, int CountEs)
    {
        if (!CheckInternetConnection())
        {
            NoInternetConnection();
            yield break;
        }

        int retryCount = 0;

        WWWForm form = new WWWForm();
        form.AddField("IDpacchetto", ID);

        while (retryCount < 2)
        {
            using (UnityWebRequest www = UnityWebRequest.Post(WEB_COUNT_SC, form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    retryCount++;
                    Debug.Log(www.error + " - Tentativo #" + retryCount);
                    continue;
                }
                else
                {
                    string sjsonArray = www.downloadHandler.text;
                    JSONArray jsonArray = JSON.Parse(sjsonArray) as JSONArray;

                    int Count = 0;

                    for (int i = 0; i < jsonArray.Count; i++)
                    {
                        Count = jsonArray[i].AsObject[0];
                    }
                    StartCoroutine(DatiPacchetto(ID));
                    //break che esce da retryCount
                    break;
                }
            }
        }
        if (retryCount == 2) // se il secondo tentativo ha fallito
        {
            Debug.Log("TENTATIVO #2 FALLITO.");
        }
    }
    public IEnumerator CountEsercizi(int ID)
    {
        if (!CheckInternetConnection())
        {
            NoInternetConnection();
            yield break;
        }

        int retryCount = 0;

        WWWForm form = new WWWForm();
        form.AddField("IDpacchetto", ID);

        while (retryCount < 2)
        {
            using (UnityWebRequest www = UnityWebRequest.Post(WEB_COUNT_ES, form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    retryCount++;
                    Debug.Log(www.error + " - Tentativo #" + retryCount);
                    continue;
                }
                else
                {
                    string sjsonArray = www.downloadHandler.text;
                    JSONArray jsonArray = JSON.Parse(sjsonArray) as JSONArray;

                    int Count = 0;

                    for (int i = 0; i < jsonArray.Count; i++)
                    {
                        Count = jsonArray[i].AsObject[0];
                    }
                    StartCoroutine(CountSchede(ID, Count));
                    //break che esce da retryCount
                    break;
                }
            }
        }
        if (retryCount == 2) // se il secondo tentativo ha fallito
        {
            Debug.Log("TENTATIVO #2 FALLITO.");
        }
    }
    public IEnumerator DatiPacchetto(int ID, Action<Tuple<string, int, string>> onComplete = null)
    {
        if (!CheckInternetConnection())
        {
            NoInternetConnection();
            yield break;
        }

        int retryCount = 0;

        WWWForm form = new WWWForm();
        form.AddField("IDpacchetto", ID);

        while (retryCount < 2)
        {
            using (UnityWebRequest www = UnityWebRequest.Post(WEB_GET_PAC, form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    retryCount++;
                    Debug.Log(www.error + " - Tentativo #" + retryCount);
                    continue;
                }
                else
                {
                    string sjsonArray = www.downloadHandler.text;
                    JSONArray jsonArray = JSON.Parse(sjsonArray) as JSONArray;

                    string Nome = "";
                    int Versione = 0;
                    string Utente = "";

                    for (int i = 0; i < jsonArray.Count; i++)
                    {
                        Nome = jsonArray[i].AsObject["NomePacchetto"];
                        Versione = jsonArray[i].AsObject["Versione"];
                        Utente = jsonArray[i].AsObject["NomeUtente"];
                    }
                    if (onComplete != null)
                    {
                        onComplete(new Tuple<string, int, string>(Nome, Versione, Utente));
                    }
                    //break che esce da retryCount
                    break;
                }
            }
        }
        if (retryCount == 2) // se il secondo tentativo ha fallito
        {
            Debug.Log("TENTATIVO #2 FALLITO.");
        }
    }

    public IEnumerator SelezionaEsercizi(int ID, Action<JSONArray> onComplete = null)
    {
        if (!CheckInternetConnection())
        {
            NoInternetConnection();
            yield break;
        }

        int retryCount = 0;

        WWWForm form = new WWWForm();
        form.AddField("IDpacchetto", ID);

        while (retryCount < 2)
        {
            using (UnityWebRequest www = UnityWebRequest.Post(WEB_GET_ES, form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    retryCount++;
                    Debug.Log(www.error + " - Tentativo #" + retryCount);
                    continue;
                }
                else
                {
                    string sjsonArray = www.downloadHandler.text;
                    JSONArray jsonArray = JSON.Parse(sjsonArray) as JSONArray;

                    onComplete?.Invoke(jsonArray);
                    //break che esce da retryCount
                    break;
                }
            }
        }
        if (retryCount == 2) // se il secondo tentativo ha fallito
        {
            Debug.Log("TENTATIVO #2 FALLITO.");
        }
    }
    public IEnumerator SelezionaMuscoli(int ID, Action<JSONArray> onComplete = null)
    {
        if (!CheckInternetConnection())
        {
            NoInternetConnection();
            yield break;
        }

        int retryCount = 0;

        WWWForm form = new WWWForm();
        form.AddField("IDesercizio", ID);

        while (retryCount < 2)
        {
            using (UnityWebRequest www = UnityWebRequest.Post(WEB_GET_MUS, form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    retryCount++;
                    Debug.Log(www.error + " - Tentativo #" + retryCount);
                    continue;
                }
                else
                {
                    JSONArray jsonArray = new JSONArray();
                    string sjsonArray = www.downloadHandler.text;
                    if (sjsonArray == "0 results")
                    {
                        Debug.Log("non ce ne sono di muscoli");
                    }
                    else
                    {
                        jsonArray = JSON.Parse(sjsonArray) as JSONArray;
                    }
                    onComplete?.Invoke(jsonArray);
                    //break che esce da retryCount
                    break;
                }
            }
        }
        if (retryCount == 2) // se il secondo tentativo ha fallito
        {
            Debug.Log("TENTATIVO #2 FALLITO.");
        }
    }
    public IEnumerator SelezionaDettagli(int ID, Action<JSONArray> onComplete = null)
    {
        if (!CheckInternetConnection())
        {
            NoInternetConnection();
            yield break;
        }

        int retryCount = 0;

        WWWForm form = new WWWForm();
        form.AddField("IDscheda", ID);

        while (retryCount < 2)
        {
            using (UnityWebRequest www = UnityWebRequest.Post(WEB_GET_DET, form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    retryCount++;
                    Debug.Log(www.error + " - Tentativo #" + retryCount);
                    continue;
                }
                else
                {
                    string sjsonArray = www.downloadHandler.text;
                    JSONArray jsonArray = new JSONArray();
                    if (sjsonArray != "0 results")
                    {
                        jsonArray = JSON.Parse(sjsonArray) as JSONArray;
                    }
                    if (onComplete != null)
                    {
                        onComplete(jsonArray);
                    }
                    //break che esce da retryCount
                    break;
                }
            }
        }
        if (retryCount == 2) // se il secondo tentativo ha fallito
        {
            Debug.Log("TENTATIVO #2 FALLITO.");
        }
    }
    public IEnumerator SelezionaSchede(int ID, Action<JSONArray> onComplete = null)
    {
        if (!CheckInternetConnection())
        {
            NoInternetConnection();
            yield break;
        }

        int retryCount = 0;

        WWWForm form = new WWWForm();
        form.AddField("IDpacchetto", ID);

        while (retryCount < 2)
        {
            using (UnityWebRequest www = UnityWebRequest.Post(WEB_GET_SC, form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    retryCount++;
                    Debug.Log(www.error + " - Tentativo #" + retryCount);
                    continue;
                }
                else
                {
                    string sjsonArray = www.downloadHandler.text;
                    JSONArray jsonArray = JSON.Parse(sjsonArray) as JSONArray;

                    onComplete?.Invoke(jsonArray);
                    //break che esce da retryCount
                    break;
                }
            }
        }
        if (retryCount == 2) // se il secondo tentativo ha fallito
        {
            Debug.Log("TENTATIVO #2 FALLITO.");
        }
    }
    public IEnumerator cSelezionaPacchetto(int IDpacchetto)
    {
        if (!CheckInternetConnection())
        {
            NoInternetConnection();
            yield break;
        }

        int retryCount = 0;

        WWWForm form = new WWWForm();
        form.AddField("IDpacchetto", IDpacchetto);

        while (retryCount < 2)
        {
            using (UnityWebRequest www = UnityWebRequest.Post(WEB_GET_PAC, form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    retryCount++;
                    Debug.Log(www.error + " - Tentativo #" + retryCount);
                    continue;
                }
                else
                {
                    Debug.Log(www.downloadHandler.text);
                    string sjsonArray = www.downloadHandler.text;
                    JSONArray jsonArray = JSON.Parse(sjsonArray) as JSONArray;

                    for (int i = 0; i < jsonArray.Count; i++)
                    {
                        string versione = jsonArray[i].AsObject["Versione"];
                    }
                    //break che esce da retryCount
                    break;
                }
            }
        }
        if (retryCount == 2) // se il secondo tentativo ha fallito
        {
            Debug.Log("TENTATIVO #2 FALLITO.");
        }
    }
    public IEnumerator SelezionaImmagine(int IDes)
    {
        if (!CheckInternetConnection())
        {
            NoInternetConnection();
            yield break;
        }

        int retryCount = 0;

        WWWForm form = new WWWForm();
        form.AddField("IDesercizio", IDes);

        string ImgString = "";
        while (ImgString == "")
        {
            while (retryCount < 2)
            {
                using (UnityWebRequest www = UnityWebRequest.Post(WEB_GET_IMG, form))
                {
                    yield return www.SendWebRequest();

                    if (www.result != UnityWebRequest.Result.Success)
                    {
                        retryCount++;
                        Debug.Log(www.error + " - Tentativo #" + retryCount);
                        continue;
                    }
                    else
                    {
                        ImgString = www.downloadHandler.text;


                        byte[] culo = www.downloadHandler.data;

                        if (!gameObject.GetComponent<Image>())
                            gameObject.AddComponent<Image>();
                        Image immagine = gameObject.GetComponent<Image>();
                        immagine = AppManager.Instance.uploadImage.ConvertByteArrayToImage(culo);

                        Globals.PAGE_MENU_TRAINING.transform.Find("Image").GetComponent<Image>().sprite = immagine.sprite;

                        //StartCoroutine(InserisciImmagine(immagine));
                        //break che esce da retryCount
                        break;
                    }
                }
            }
            if (retryCount == 2) // se il secondo tentativo ha fallito
            {
                Debug.Log("TENTATIVO #2 FALLITO.");
            }
        }
    }

    //public IEnumerator InserisciImmagine(Image img)
    //{
    //    int retryCount = 0;

    //    WWWForm form = new WWWForm();
    //    byte[] imgByte = null;

    //    imgByte = img.sprite.texture.EncodeToPNG();

    //    string NomeImg = "yt2.png";

    //    form.AddBinaryData("myimage", imgByte, "imageFromUnity.png", "image/png");
    //    form.AddField("NomeImg", NomeImg);


    //    while (retryCount < 2)
    //    {
    //        using (UnityWebRequest www = UnityWebRequest.Post(WEB_INS_IMG, form))
    //        {

    //            yield return www.SendWebRequest();

    //            if (www.error != null)
    //            {
    //                Debug.Log(www.downloadHandler.text);
    //                Debug.Log(www.error);
    //            }
    //            else
    //            {
    //                string a = www.downloadHandler.text;
    //            }
    //            www.Dispose();
    //        }
    //    }
    //    if (retryCount == 2) // se il secondo tentativo ha fallito
    //    {
    //        Debug.Log("TENTATIVO #2 FALLITO.");
    //    }
    //}
}
