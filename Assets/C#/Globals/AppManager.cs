using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppManager : MonoBehaviour
{
    public static AppManager Instance { get; private set; }

    public Web web;
    public Objects objects;
    public SQLiteEvents sql;
    public MessageBox messagebox;
    public DownloadWeb download;
    public NavigationManager navManager;
    public Esercizi esercizi;
    public Schede schede;
    public DettagliEsercizi dettagliEsercizi;
    public Allenamento allenamento;
    public AllenamentoBottoni allenamentoBottoni;
    public AllenamentoEsercizio allenamentoEsercizio;
    public Finito finito;
    public UploadImage uploadImage;
    public EserciziAllenamento eserciziAllenamento;
    public dbWeb dbweb;

    public ShakeObject shakeObject;


    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad()
    {
        // Crea l'istanza della classe AppManager
        GameObject mngGameObject = new GameObject("AppMng");
        Instance = mngGameObject.AddComponent<AppManager>();

        // Crea i componenti necessari
        Instance.web = mngGameObject.AddComponent<Web>();
        Instance.objects = mngGameObject.AddComponent<Objects>();
        Instance.sql = mngGameObject.AddComponent<SQLiteEvents>();
        Instance.messagebox = mngGameObject.AddComponent<MessageBox>();
        Instance.download = mngGameObject.AddComponent<DownloadWeb>();
        Instance.navManager = mngGameObject.AddComponent<NavigationManager>();
        Instance.esercizi = mngGameObject.AddComponent<Esercizi>();
        Instance.schede = mngGameObject.AddComponent<Schede>();
        Instance.dettagliEsercizi = mngGameObject.AddComponent<DettagliEsercizi>();
        Instance.allenamento = mngGameObject.AddComponent<Allenamento>();
        Instance.allenamentoBottoni = mngGameObject.AddComponent<AllenamentoBottoni>();
        Instance.allenamentoEsercizio = mngGameObject.AddComponent<AllenamentoEsercizio>();
        Instance.finito = mngGameObject.AddComponent<Finito>();
        Instance.uploadImage = mngGameObject.AddComponent<UploadImage>();
        Instance.eserciziAllenamento = mngGameObject.AddComponent<EserciziAllenamento>();
        Instance.dbweb = mngGameObject.AddComponent<dbWeb>();

        Instance.shakeObject = mngGameObject.AddComponent<ShakeObject>();


        DontDestroyOnLoad(mngGameObject);
    }

}
