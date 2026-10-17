using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Finito : MonoBehaviour
{
    static private int Pos;
    private void Start()
    {
        //se funziona:
        //if (FindObjectOfType<Finito>() != null)
        //{
        //    Destroy(this);
        //}

        Globals.COUNT_FINITO += 1;
        if (Globals.COUNT_FINITO > 1)
            Destroy(this);
    }
    public void Init()
    {
        AppManager.Instance.objects.PagesTraining("Finito");
        Globals.PAGE_FINITO.transform.Find("cnvRiepilogo").GetComponent<Canvas>().gameObject.SetActive(false);
        Globals.PAGE_FINITO.transform.Find("cnvResoconto").GetComponent<Canvas>().gameObject.SetActive(true);

        Globals.PAGE_FINITO.transform.Find("cnvResoconto/txtDurata").GetComponent<TextMeshProUGUI>().gameObject.SetActive(false);
        Globals.PAGE_FINITO.transform.Find("cnvResoconto/btnRiepilogo").GetComponent<Button>().gameObject.SetActive(false);

        Globals.PAGE_FINITO.transform.Find("cnvResoconto/Load").GetComponent<Image>().fillAmount = 0;

        Globals.PAGE_FINITO.transform.Find("cnvResoconto/btnRiepilogo").GetComponent<Button>().onClick.RemoveAllListeners();
        Globals.PAGE_FINITO.transform.Find("cnvRiepilogo/btnConfirm").GetComponent<Button>().onClick.RemoveAllListeners();
        Globals.PAGE_FINITO.transform.Find("cnvResoconto/btnRiepilogo").GetComponent<Button>().onClick.AddListener(() => Riepilogo());
        Globals.PAGE_FINITO.transform.Find("cnvRiepilogo/btnConfirm").GetComponent<Button>().onClick.AddListener(() => btnConferma());

        Globals.PAGE_ALLENAMENTO.gameObject.SetActive(true);
        StartCoroutine(Percentuale(0));
        Globals.PAGE_ALLENAMENTO.gameObject.SetActive(false);
    }
    IEnumerator Percentuale(float Perc)
    {
        string sPerc;
        while ((int)Perc * 100 < 100)
        {
            Perc += Time.deltaTime;
            Globals.PAGE_FINITO.transform.Find("cnvResoconto/Load").GetComponent<Image>().fillAmount = Perc;
            sPerc = Math.Round(Perc * 100, 0).ToString() + "%";
            Globals.PAGE_FINITO.transform.Find("cnvResoconto").GetComponent<Canvas>().GetComponent<TextMeshProUGUI>().text = sPerc;
            yield return null;
        }
        Globals.PAGE_FINITO.transform.Find("cnvResoconto").GetComponent<Canvas>().GetComponent<TextMeshProUGUI>().text = "FINITO";
        Globals.PAGE_FINITO.transform.Find("cnvResoconto").GetComponent<Canvas>().GetComponent<TextMeshProUGUI>().fontSize = 60;

        Vector2 EndPos = new Vector2(Globals.PAGE_FINITO.transform.Find("cnvResoconto").GetComponent<Canvas>().GetComponent<TextMeshProUGUI>().gameObject.transform.position.x,
            Globals.PAGE_FINITO.transform.Find("cnvResoconto").GetComponent<Canvas>().GetComponent<TextMeshProUGUI>().gameObject.transform.position.y + 350);

        StartCoroutine(Move(
            Globals.PAGE_FINITO.transform.Find("cnvResoconto/Load").GetComponent<Image>().gameObject,
            EndPos,
            2f
            ));

        DataTable dtAllenamento = AppManager.Instance.sql.SelectAllAllenamento(" WHERE " + Globals.COL_ALL_ID + " = " + Globals.ID_ALLENAMENTO, Globals.COL_ALL_ID);
        float Durata = int.Parse(dtAllenamento.Rows[0][Globals.COL_ALL_DURATA].ToString().Substring(0, 2)) * 3060 + int.Parse(dtAllenamento.Rows[0][Globals.COL_ALL_DURATA].ToString().Substring(3, 2)) * 60 + int.Parse(dtAllenamento.Rows[0][Globals.COL_ALL_DURATA].ToString().Substring(6, 2));

        StartCoroutine(DurataCrono(Durata));
    }
    IEnumerator Move(GameObject oggetto, Vector2 endPos, float speed)
    {
        Transform Posizione = oggetto.transform;
        while (Vector2.Distance(Posizione.position, endPos) > 0.1f)
        {
            oggetto.transform.position = Vector3.Lerp(Posizione.position, endPos, speed * Time.deltaTime);
            yield return null;
        }
    }
    IEnumerator DurataCrono(float Durata)
    {
        float Tdurata = 0;

        while (Tdurata <= Durata)
        {
            if (Globals.PAGE_FINITO.transform.Find("cnvResoconto/txtDurata").GetComponent<TextMeshProUGUI>().gameObject.activeSelf)
            {
                Tdurata += 1 * Time.deltaTime * (Durata / 3);
                var TimeConverted = TimeSpan.FromSeconds(Tdurata);
                Globals.PAGE_FINITO.transform.Find("cnvResoconto/txtDurata").GetComponent<TextMeshProUGUI>().text = "DURATA ALLENAMENTO:\r\n" + TimeConverted.ToString(@"hh\:mm\:ss");
            }
            else
            {
                if (Globals.PAGE_FINITO.transform.Find("cnvResoconto/Load").GetComponent<Image>().gameObject.transform.position.y >
                   Globals.PAGE_FINITO.transform.Find("cnvResoconto/txtDurata").GetComponent<TextMeshProUGUI>().gameObject.transform.position.y + 200)
                    Globals.PAGE_FINITO.transform.Find("cnvResoconto/txtDurata").GetComponent<TextMeshProUGUI>().gameObject.SetActive(true);
            }
                yield return null;
        }
        Globals.PAGE_FINITO.transform.Find("cnvResoconto/txtDurata").GetComponent<TextMeshProUGUI>().text = "DURATA ALLENAMENTO:\r\n" + TimeSpan.FromSeconds(Durata).ToString(@"hh\:mm\:ss");
        Thread.Sleep(1000);
        Globals.PAGE_FINITO.transform.Find("cnvResoconto/btnRiepilogo").GetComponent<Button>().gameObject.SetActive(true);
    }
    public void Riepilogo()
    {
        Globals.PAGE_FINITO.transform.Find("cnvRiepilogo").GetComponent<Canvas>().gameObject.SetActive(true);
        Globals.PAGE_FINITO.transform.Find("cnvResoconto").GetComponent<Canvas>().gameObject.SetActive(false);
        Globals.PP_TIME.gameObject.SetActive(false);


        SvuotaPaginaRiepilogo();

        string sCondition = " WHERE ll." + Globals.COL_ALL_ID + " = " + Globals.ID_ALLENAMENTO + " ";
        DataTable dtTable = AppManager.Instance.sql.SelectAllAttivita(sCondition, "tt." + Globals.COL_ATT_ID);

        Globals.BTN_FINITO_ESE.gameObject.SetActive(true);
        Globals.BTN_FINITO_TIM.gameObject.SetActive(true);

        Pos = 0;
        int Num = 3;
        for (int row = 0; row < dtTable.Rows.Count; row++)
        {
            CaricaBottoneRecupero(Num, dtTable.Rows[row][Globals.COL_ATT_RECUPERO].ToString());
            Num += 1;
            CaricaBottoneEsercizio(dtTable.Rows[row][Globals.COL_ESE_NOME].ToString(), Num, dtTable.Rows[row][Globals.COL_ATT_PESO].ToString(), dtTable.Rows[row][Globals.COL_DET_RIPnOt].ToString(), dtTable.Rows[row][Globals.COL_ATT_RIPETIZIONIn].ToString(), dtTable.Rows[row][Globals.COL_ATT_RIPETIZIONIt].ToString());
            Num += 1;
        }
        Globals.BTN_FINITO_ESE.gameObject.SetActive(false);
        Globals.BTN_FINITO_TIM.gameObject.SetActive(false);

        Globals.PP_TIME.transform.Find("cnvPanel/btnConfirm").GetComponent<Button>().onClick.RemoveAllListeners();
        Globals.PP_TIME.transform.Find("cnvPanel/btnCancel").GetComponent<Button>().onClick.RemoveAllListeners();
        Globals.PP_TIME.transform.Find("cnvTime/btnUp_mm").GetComponent<Button>().onClick.RemoveAllListeners();
        Globals.PP_TIME.transform.Find("cnvTime/btnDown_mm").GetComponent<Button>().onClick.RemoveAllListeners();
        Globals.PP_TIME.transform.Find("cnvTime/btnUp_ss").GetComponent<Button>().onClick.RemoveAllListeners();
        Globals.PP_TIME.transform.Find("cnvTime/btnDown_ss").GetComponent<Button>().onClick.RemoveAllListeners();

        Globals.PP_TIME.transform.Find("cnvPanel/btnConfirm").GetComponent<Button>().onClick.AddListener(() => PopupTimeClose("Confirm"));
        Globals.PP_TIME.transform.Find("cnvPanel/btnCancel").GetComponent<Button>().onClick.AddListener(() => PopupTimeClose("Cancel"));
        Globals.PP_TIME.transform.Find("cnvTime/btnUp_mm").GetComponent<Button>().onClick.AddListener(() => ChangeTimeReps("I", "M"));
        Globals.PP_TIME.transform.Find("cnvTime/btnDown_mm").GetComponent<Button>().onClick.AddListener(() => ChangeTimeReps("D", "M"));
        Globals.PP_TIME.transform.Find("cnvTime/btnUp_ss").GetComponent<Button>().onClick.AddListener(() => ChangeTimeReps("I", "S"));
        Globals.PP_TIME.transform.Find("cnvTime/btnDown_ss").GetComponent<Button>().onClick.AddListener(() => ChangeTimeReps("D", "S"));
    }
    private void CaricaBottoneEsercizio(string Nome, int Num, string Peso, string NoT, string Reps, string Time)
    {
        Pos += 1;
        Button button = Globals.BTN_FINITO_ESE;
        //Duplica bottone
        Button Duplicato = Instantiate(button);
        //Rinominalo
        Duplicato.name = "Att" + Num;
        //Assegnagli come padre "Content"
        GameObject Parent = button.transform.parent.gameObject;
        Duplicato.transform.SetParent(Parent.gameObject.transform);
        //Assegnazione indice (posizione)
        Duplicato.transform.SetSiblingIndex(Num + 1);
        //Cambia la stringa del testo figlio
        Duplicato.transform.Find("Pos").GetComponent<TextMeshProUGUI>().text = Pos.ToString();
        Duplicato.transform.Find("Nome").GetComponent<TextMeshProUGUI>().text = Nome;
        Duplicato.transform.Find("Weight").GetComponent<TMP_InputField>().text = Peso;
        Duplicato.transform.Find("Time").GetComponentInChildren<TextMeshProUGUI>().text = Time;
        if (NoT == "N")
        {
            Duplicato.transform.Find("Reps").GetComponent<Canvas>().gameObject.SetActive(true);
            Duplicato.transform.Find("Reps").GetComponent<TMP_InputField>().text = Reps;
            RectTransform rectTransform = Duplicato.transform.Find("Nome").GetComponent<TextMeshProUGUI>().GetComponent<RectTransform>();

            Duplicato.transform.Find("Nome").GetComponent<TextMeshProUGUI>().GetComponent<RectTransform>().sizeDelta = new Vector2(Duplicato.transform.Find("Nome").GetComponent<TextMeshProUGUI>().GetComponent<RectTransform>().rect.width - 100, Duplicato.transform.Find("Nome").GetComponent<TextMeshProUGUI>().GetComponent<RectTransform>().rect.height);
        }
        else
        {
            Duplicato.transform.Find("Reps").GetComponent<Canvas>().gameObject.SetActive(false);
        }
        Duplicato.transform.Find("Reps").GetComponent<Canvas>().GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
        Duplicato.transform.Find("Time").GetComponent<Canvas>().GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
        Duplicato.transform.Find("Weight").GetComponent<Canvas>().GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);

        int ChildNum = 0; ;
        while (1 < 2)
        {
            if (Parent.transform.GetChild(ChildNum).gameObject == Duplicato.gameObject)
                break;
            ChildNum += 1;
        }
        Duplicato.transform.Find("Time").GetComponent<Button>().onClick.RemoveAllListeners();
        Duplicato.transform.Find("Time").GetComponent<Button>().onClick.AddListener(() => PopupTimeShow(Parent, ChildNum));

        Duplicato.transform.Find("btnDelete").GetComponent<Button>().onClick.RemoveAllListeners();
        Duplicato.transform.Find("btnDelete").GetComponent<Button>().onClick.AddListener(() => DeleteAtt(ChildNum));
    }
    private void CaricaBottoneRecupero(int Num, string Time)
    {
        Button button = Globals.BTN_FINITO_TIM;
        //Duplica bottone
        Button Duplicato = Instantiate(button);
        //Rinominalo
        Duplicato.name = "Att" + Num;
        //Assegnagli come padre "Content"
        GameObject Parent = button.transform.parent.gameObject;
        Duplicato.transform.SetParent(Parent.gameObject.transform);
        //Assegnazione indice (posizione)
        Duplicato.transform.SetSiblingIndex(Num + 1);
        //Cambia la stringa del testo figlio
        Duplicato.transform.Find("Tempo").GetComponent<TextMeshProUGUI>().text = Time;


        Button btnIncreaseTime = Duplicato.transform.Find("btnUp").GetComponent<Button>();
        Button btnDecreaseTime = Duplicato.transform.Find("btnDown").GetComponent<Button>();

        btnIncreaseTime.gameObject.AddComponent<ButtonPressTime>();
        btnIncreaseTime.gameObject.GetComponent<ButtonPressTime>().Testo = Duplicato.transform.Find("Tempo").GetComponent<TextMeshProUGUI>();
        btnIncreaseTime.gameObject.GetComponent<ButtonPressTime>().IncreseOrDecrease = "Increase";

        btnDecreaseTime.gameObject.AddComponent<ButtonPressTime>();
        btnDecreaseTime.gameObject.GetComponent<ButtonPressTime>().Testo = Duplicato.transform.Find("Tempo").GetComponent<TextMeshProUGUI>();
        btnDecreaseTime.gameObject.GetComponent<ButtonPressTime>().IncreseOrDecrease = "Decrease";

        btnIncreaseTime.onClick.AddListener(() => ChangeTimeRest("Increase", Duplicato.transform.Find("Tempo").GetComponent<TextMeshProUGUI>()));
        btnDecreaseTime.onClick.AddListener(() => ChangeTimeRest("Decrease", Duplicato.transform.Find("Tempo").GetComponent<TextMeshProUGUI>()));

        int ChildNum = 0; ;
        while (1 < 2)
        {
            if (Parent.transform.GetChild(ChildNum).gameObject == Duplicato.gameObject)
                break;
            ChildNum += 1;
        }

        Duplicato.transform.Find("btnDelete").GetComponent<Button>().onClick.RemoveAllListeners();
        Duplicato.transform.Find("btnDelete").GetComponent<Button>().onClick.AddListener(() => DeleteRec(ChildNum));
    }
    private void PopupTimeShow(GameObject Parent, int ChildNum)
    {
        Globals.PP_TIME.gameObject.SetActive(true);
        string Time = Parent.transform.GetChild(ChildNum).gameObject.transform.Find("Time").GetComponentInChildren<TextMeshProUGUI>().text;
        string ppTime = Time.Substring(0, 2) + " : " + Time.Substring(3, 2);
        Globals.PP_TIME.transform.Find("cnvTime").GetComponent<Canvas>().GetComponent<TextMeshProUGUI>().text = ppTime;

        Globals.INTERO = ChildNum;
    }
    public void PopupTimeClose(string ButtonClicked)
    {
        if (ButtonClicked == "Confirm")
        {
            GameObject Parent = Globals.BTN_FINITO_ESE.transform.parent.gameObject;
            int ChildNum = Globals.INTERO;
            string time = Regex.Replace(Globals.PP_TIME.transform.Find("cnvTime").GetComponent<Canvas>().GetComponent<TextMeshProUGUI>().text, @"\s", "");
            Parent.transform.GetChild(ChildNum).transform.Find("Time").GetComponentInChildren<TextMeshProUGUI>().text = time;
        }
        Globals.PP_TIME.gameObject.SetActive(false);
    }
    private void ChangeTimeRest(string IncreseOrDecrease, TextMeshProUGUI Testo)
    {
        string sValueMin;
        string sValueSec;
        int iValueMin = int.Parse(Testo.text.Substring(0, 2));
        int iValueSec = int.Parse(Testo.text.Substring(3, 2));

        if (IncreseOrDecrease == "Increase")
        {
            iValueSec += 1;
            if (iValueSec > 59)
            {
                iValueSec = 0;
                iValueMin += 1;
            }
            if (iValueMin > 59)
                iValueMin = 0;
        }
        else
        {
            if (iValueSec <= 0)
            {
                iValueSec = 60;
                iValueMin -= 1;
            }
            iValueSec -= 1;
            if (iValueMin < 0)
                iValueMin = 59;
        }
        if (iValueMin < 10)
            sValueMin = "0" + iValueMin;
        else
            sValueMin = iValueMin.ToString();
        if (iValueSec < 10)
            sValueSec = "0" + iValueSec;
        else
            sValueSec = iValueSec.ToString();
        Testo.text = sValueMin + ":" + sValueSec;
    }
    private void ChangeTimeReps(string IoD, string MoS)
    {
        TextMeshProUGUI Testo = Globals.PP_TIME.transform.Find("cnvTime").GetComponent<Canvas>().GetComponent<TextMeshProUGUI>();
        int Index;
        if (MoS == "M")
            Index = 0;
        else
            Index = 5;

        int iValue = int.Parse(Testo.text.Substring(Index, 2));

        if (IoD == "I")
        {
            iValue += 1;
            if (iValue == 60)
                iValue = 0;
        }
        else
        {
            if (iValue == 00)
                iValue = 60;
            iValue -= 1;
        }

        string sValue;
        if (iValue < 10)
            sValue = "0" + iValue;
        else
            sValue = iValue.ToString();

        if (Index == 0)
            sValue = sValue + Testo.text.Substring(2);
        else
            sValue = Testo.text.Substring(0, 5) + sValue;

        Testo.text = sValue;
    }
    private void SvuotaPaginaRiepilogo()
    {
        GameObject Parent = Globals.BTN_FINITO_ESE.transform.parent.gameObject;
        while (Parent.transform.childCount > 2)
        {
            DestroyImmediate(Parent.transform.GetChild(Parent.transform.childCount - 1).gameObject);
        }
    }
    public void Chiudi()
    {
        SvuotaPaginaRiepilogo();
    }
    private void UpdateAll()
    {
        GameObject Parent = Globals.BTN_FINITO_TIM.transform.parent.gameObject;

        string sCondition = " WHERE ll." + Globals.COL_ALL_ID + " = " + Globals.ID_ALLENAMENTO + " ";
        DataTable dtTable = AppManager.Instance.sql.SelectAllAttivita(sCondition, "tt." + Globals.COL_ATT_ID);

        for (int row = 0; row < dtTable.Rows.Count; row++)
        {
            GameObject FiglioRecupero = Parent.transform.GetChild(row * 2 + 2).gameObject;
            GameObject FiglioEsercizio = Parent.transform.GetChild(row * 2 + 2 + 1).gameObject;
            string Recupero = FiglioRecupero.transform.Find("Tempo").GetComponent<TextMeshProUGUI>().text;
            string RepsT = FiglioEsercizio.transform.Find("Time").GetComponentInChildren<TextMeshProUGUI>().text;
            int RepsN = 0;
            if (dtTable.Rows[row][Globals.COL_DET_RIPnOt].ToString() == "N")
                RepsN = int.Parse(FiglioEsercizio.transform.Find("Reps").GetComponent<TMP_InputField>().text);
            double Peso = double.Parse(FiglioEsercizio.transform.Find("Weight").GetComponent<TMP_InputField>().text);

            AppManager.Instance.sql.UpdateAttività(int.Parse(dtTable.Rows[row][Globals.COL_ATT_ID].ToString()), int.Parse(dtTable.Rows[row][Globals.COL_ATT_SERIE].ToString()), RepsN, RepsT, Peso, Recupero, int.Parse(dtTable.Rows[row][Globals.COL_DET_ID].ToString()), int.Parse(dtTable.Rows[row][Globals.COL_ALL_ID].ToString()));
        }
    }
    private void DeleteAtt(int ChildNum)
    {
        UpdateAll();


        string sCondition = " WHERE ll." + Globals.COL_ALL_ID + " = " + Globals.ID_ALLENAMENTO + " ";
        DataTable dtTable = AppManager.Instance.sql.SelectAllAttivita(sCondition, "tt." + Globals.COL_ATT_ID);

        int row = (ChildNum - 1) / 2 - 1;
        int ID = int.Parse(dtTable.Rows[row][Globals.COL_ATT_ID].ToString());

        AppManager.Instance.sql.DeleteAttivita(ID);

        Riepilogo();
    }
    private void DeleteRec(int ChildNum)
    {
        GameObject Parent = Globals.BTN_FINITO_TIM.transform.parent.gameObject;
        Parent.transform.GetChild(ChildNum).gameObject.transform.Find("Tempo").GetComponent<TextMeshProUGUI>().text = "00:00";
    }
    public void btnConferma()
    {
        UpdateAll();
        SvuotaPaginaRiepilogo();
        //allenamento ha in eredità finito, che prende in eredità objects e chiama changetotab, cha ha una corutine, ma se allenamento non
        //è attivo va tutti in errore, quindi si rende attivo, si lancia la funzione e poi menu che lo rende inattivo
        Globals.PAGE_ALLENAMENTO.SetActive(true);
        AppManager.Instance.objects.ChangeToTab(1);
        AppManager.Instance.objects.Scenes("Menu");
    }
}
