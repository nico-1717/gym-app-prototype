using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonWeb
{
    public string path;
    public string mobilePath;
    public void CreateFile(string FileName, string json)
    {
        path = Application.persistentDataPath;
        FileStream fileStream = new FileStream(path + "/" + FileName, FileMode.Create);

        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.Write(json);
        }
    }
    #region Load
    public JsonEsercizio[] LoadJsonEsercizi()
    {
        path = Application.persistentDataPath;
        FileStream fileStream = new FileStream(path + "/Esercizi", FileMode.Open);
        string sjsonArray;
        using (StreamReader reader = new StreamReader(fileStream))
        {
            sjsonArray = reader.ReadToEnd();
        }
        JSONArray jsonArray = JSON.Parse(sjsonArray) as JSONArray;
        JsonEsercizio[] es = new JsonEsercizio[jsonArray.Count];

        int ID;
        string Nome;
        string Note;
        string Consigli;
        for (int i = 0; i < jsonArray.Count; i++)
        {
            ID = jsonArray[i].AsObject["IDesercizio"];
            Nome = jsonArray[i].AsObject["NomeEsercizio"];
            Note = jsonArray[i].AsObject["Note"];
            Consigli = jsonArray[i].AsObject["Consigli"];

            es[i] = new JsonEsercizio(ID, Nome, Note, Consigli, null);
        }
        return es;
    }
    public JsonScheda[] LoadJsonSchede()
    {
        path = Application.persistentDataPath;
        FileStream fileStream = new FileStream(path + "/Schede", FileMode.Open);
        string sjsonArray;
        using (StreamReader reader = new StreamReader(fileStream))
        {
            sjsonArray = reader.ReadToEnd();
        }
        JSONArray jsonArray = JSON.Parse(sjsonArray) as JSONArray;
        JsonScheda[] sc = new JsonScheda[jsonArray.Count];

        int ID;
        string Nome;

        for (int i = 0; i < jsonArray.Count; i++)
        {
            ID = jsonArray[i].AsObject["IDscheda"];
            Nome = jsonArray[i].AsObject["NomeScheda"];

            sc[i] = new JsonScheda(ID, Nome);
        }
        return sc;
    }
    public JsonDettaglio[] LoadJsonDettagli()
    {
        path = Application.persistentDataPath;
        FileStream fileStream = new FileStream(path + "/Dettagli", FileMode.Open);
        string sjsonArray;
        using (StreamReader reader = new StreamReader(fileStream))
        {
            sjsonArray = reader.ReadToEnd();
        }
        JSONArray jsonArray = JSON.Parse(sjsonArray) as JSONArray;
        JsonDettaglio[] det = new JsonDettaglio[jsonArray.Count];

        int ID;
        int Serie;
        string NoT;
        int RipN;
        string RipsT;
        string Recupero;
        string Attesa;
        int Posizione;
        int IDes;
        int IDsc;

        for (int i = 0; i < jsonArray.Count; i++)
        {
            ID = jsonArray[i].AsObject["IDdettagli"];
            Serie = jsonArray[i].AsObject["Serie"];
            NoT = jsonArray[i].AsObject["Rip_NoT"];
            RipN = jsonArray[i].AsObject["RipetizioniN"];
            RipsT = jsonArray[i].AsObject["RipetizioniT"];
            Recupero = jsonArray[i].AsObject["Recupero"];
            Attesa = jsonArray[i].AsObject["Attesa"];
            Posizione = jsonArray[i].AsObject["Posizione"];
            IDes = jsonArray[i].AsObject["IDesercizio"];
            IDsc = jsonArray[i].AsObject["IDscheda"];

            det[i] = new JsonDettaglio(ID, Serie, NoT, RipN, RipsT, Recupero, Attesa, Posizione, IDes, IDsc);
        }
        return det;
    }
    public JsonM_E[] LoadJsonM_E()
    {
        path = Application.persistentDataPath;
        FileStream fileStream = new FileStream(path + "/Muscoli", FileMode.Open);
        string sjsonArray;
        using (StreamReader reader = new StreamReader(fileStream))
        {
            sjsonArray = reader.ReadToEnd();
        }
        JSONArray jsonArray = JSON.Parse(sjsonArray) as JSONArray;
        JsonM_E[] mus = new JsonM_E[jsonArray.Count];

        int ID;
        int IDes;
        int IDmu;

        for (int i = 0; i < jsonArray.Count; i++)
        {
            ID = jsonArray[i].AsObject["IDme"];
            IDmu = jsonArray[i].AsObject["IDmuscolo"];
            IDes = jsonArray[i].AsObject["IDesercizio"];

            mus[i] = new JsonM_E(ID, IDmu, IDes);
        }
        return mus;
    }
    #endregion
    #region Save
    public void SaveEsercizi(JsonEsercizio[] es)
    {
        string json = "[";
        foreach (JsonEsercizio esercizio in es)
        {
            json += JsonUtility.ToJson(esercizio);
        }
        json += "]";
        CreateFile("Esercizi", json);
    }
    public void SaveSchede(JsonScheda[] es)
    {
        string json = "[";
        foreach (JsonScheda Scheda in es)
        {
            json += JsonUtility.ToJson(Scheda);
        }
        json += "]";
        CreateFile("Schede", json);
    }
    public void SaveDettagli(JsonDettaglio[] es)
    {
        string json = "[";
        foreach (JsonDettaglio Dettaglio in es)
        {
            json += JsonUtility.ToJson(Dettaglio);
        }
        json += "]";
        CreateFile("Dettagli", json);
    }
    public void SaveM_E(JsonM_E[] es)
    {
        string json = "[";
        foreach (JsonM_E M_E in es)
        {
            json += JsonUtility.ToJson(M_E);
        }
        json += "]";
        CreateFile("Muscoli", json);
    }
    #endregion
}
public class JsonEsercizio
{
    public int IDesercizio;
    public string NomeEsercizio;
    public string Consigli;
    public string Note;
    public string Immagine;
    public JsonEsercizio(int id, string nome, string consigli, string note, string immagine)
    {
        IDesercizio = id;
        NomeEsercizio = nome;
        Consigli = consigli;
        Note = note;
        Immagine = immagine;
    }
    public JsonEsercizio()
    {

    }
}
public class JsonScheda
{
    public int IDscheda;
    public string NomeScheda;
    public JsonScheda(int id, string nome)
    {
        IDscheda = id;
        NomeScheda = nome;
    }
    public JsonScheda()
    {

    }
}
public class JsonDettaglio
{
    public int IDdettagli;
    public int Serie;
    public string Rip_NoT;
    public int RipetizioniN;
    public string RipetizioniT;
    public string Recupero;
    public string Attesa;
    public int Posizione;
    public int IDesercizio;
    public int IDscheda;
    public JsonDettaglio(int id, int serie, string not, int ripn, string ript, string recupero, string attesa, int posizione, int ides, int idsc)
    {
        IDdettagli = id;
        Serie = serie;
        Rip_NoT = not;
        RipetizioniN = ripn;
        RipetizioniT = ript;
        Recupero = recupero;
        Attesa = attesa;
        Posizione = posizione;
        IDesercizio = ides;
        IDscheda = idsc;
    }
    public JsonDettaglio()
    {

    }
}
public class JsonM_E
{
    public int IDme;
    public int IDmuscolo;
    public int IDesercizio;
    public JsonM_E(int id, int idmus, int ides)
    {
        IDme = id;
        IDmuscolo = idmus;
        IDesercizio = ides;
    }
    public JsonM_E()
    {

    }
}