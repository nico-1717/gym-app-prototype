using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DettagliEsercizi : MonoBehaviour
{
    int iID;
    int iPos;
    int iIDes;
    int iIDsc;
    public void CaricaPaginaDettagli(int ID, string Nome, int Serie, string Recupero, string Attesa, string RipetizioniT, int RipetizioniN, string NoT, int Posizione, int IDes, int IDsc)
    {

        iID = ID;
        iPos = Posizione;
        iIDes = IDes;
        iIDsc = IDsc;
        GameObject.Find(Globals.PAGE_EXER_ADD_DET.gameObject.name).transform.Find("Name").GetComponent<TextMeshProUGUI>().text = Nome;
        GameObject.Find(Globals.PAGE_EXER_ADD_DET.gameObject.name).transform.Find("CanvasSeries/InputFieldSeries").GetComponent<TMP_InputField>().text = Serie.ToString();
        GameObject.Find(Globals.PAGE_EXER_ADD_DET.gameObject.name).transform.Find("CanvasRepsN/InputFieldReps").GetComponent<TMP_InputField>().text = RipetizioniN.ToString();
        GameObject.Find(Globals.PAGE_EXER_ADD_DET.gameObject.name).transform.Find("CanvasRepsT/txtTime").GetComponent<TextMeshProUGUI>().text = RipetizioniT.Substring(0, 2) + " : " + RipetizioniT.Substring(3, 2);
        GameObject.Find(Globals.PAGE_EXER_ADD_DET.gameObject.name).transform.Find("CanvasRest/CanvasWait/txtTime").GetComponent<TextMeshProUGUI>().text = Attesa.Substring(0, 2) + " : " + Attesa.Substring(3, 2);
        Canvas cnvRest = GameObject.Find(Globals.PAGE_EXER_ADD_DET.gameObject.name).transform.Find("CanvasRest/CanvasRest").GetComponent<Canvas>();
        cnvRest.transform.Find("txtTime").GetComponent<TextMeshProUGUI>().text = Recupero.Substring(0, 2) + " : " + Recupero.Substring(3, 2);
        if (Serie == 1)
            cnvRest.gameObject.SetActive(false);
        else
            cnvRest.gameObject.SetActive(true);

        if (NoT == "N")
        {
            GameObject.Find(Globals.PAGE_EXER_ADD_DET.gameObject.name).transform.Find("RadioButton").GetComponent<ToggleGroup>().transform.Find("OptionReps").GetComponent<Toggle>().isOn = true;
        }
        else
        {
            GameObject.Find(Globals.PAGE_EXER_ADD_DET.gameObject.name).transform.Find("RadioButton").GetComponent<ToggleGroup>().transform.Find("OptionTime").GetComponent<Toggle>().isOn = true;
        }

        RadioButton_Change(NoT);

        Globals.BTN_CONFIRM.onClick.RemoveAllListeners();
        Globals.BTN_CONFIRM.onClick.AddListener(() => ConfirmEdit());
        Globals.BTN_CANCEL.onClick.RemoveAllListeners();
        Globals.BTN_CANCEL.onClick.AddListener(() => AppManager.Instance.navManager.NavigateBack());
    }
    private void ConfirmEdit()
    {

        int iSerie = int.Parse(GameObject.Find(Globals.PAGE_EXER_ADD_DET.gameObject.name).transform.Find("CanvasSeries/InputFieldSeries").GetComponent<TMP_InputField>().text);
        int iRipetizioniN = int.Parse(GameObject.Find(Globals.PAGE_EXER_ADD_DET.gameObject.name).transform.Find("CanvasRepsN/InputFieldReps").GetComponent<TMP_InputField>().text);
        string sRipetizioniT = Regex.Replace(GameObject.Find(Globals.PAGE_EXER_ADD_DET.gameObject.name).transform.Find("CanvasRepsT/txtTime").GetComponent<TextMeshProUGUI>().text, @"\s", "");
        string sAttesaT = Regex.Replace(GameObject.Find(Globals.PAGE_EXER_ADD_DET.gameObject.name).transform.Find("CanvasRest/CanvasWait/txtTime").GetComponent<TextMeshProUGUI>().text, @"\s", "");
        string sRecuperoT = Regex.Replace(GameObject.Find(Globals.PAGE_EXER_ADD_DET.gameObject.name).transform.Find("CanvasRest/CanvasRest/txtTime").GetComponent<TextMeshProUGUI>().text, @"\s", "");
        string NoT;
        if (GameObject.Find(Globals.PAGE_EXER_ADD_DET.gameObject.name).transform.Find("RadioButton").GetComponent<ToggleGroup>().transform.Find("OptionReps").GetComponent<Toggle>().isOn)
        {
            NoT = "N";
        }
        else
        {
            NoT = "T";
        }
        if (AppManager.Instance.sql.UpdateDettagli(iID, iSerie, NoT, iRipetizioniN, sRipetizioniT, iPos, sRecuperoT, sAttesaT, iIDes, iIDsc) == true)
        {
            AppManager.Instance.navManager.NavigateBack();
        }
        else
        {
            Debug.Log("Errore");
            AppManager.Instance.navManager.NavigateTo(() => AppManager.Instance.objects.Pages("Edit"));
        }
    }
    public void RadioButton_Change(string NoT)
    {
        if (NoT == "N")
        {
            GameObject.Find(Globals.PAGE_EXER_ADD_DET.gameObject.name).transform.Find("CanvasRepsN").GetComponent<Canvas>().gameObject.SetActive(true);
            GameObject.Find(Globals.PAGE_EXER_ADD_DET.gameObject.name).transform.Find("CanvasRepsT").GetComponent<Canvas>().gameObject.SetActive(false);
        }
        else
        {
            GameObject.Find(Globals.PAGE_EXER_ADD_DET.gameObject.name).transform.Find("CanvasRepsN").GetComponent<Canvas>().gameObject.SetActive(false);
            GameObject.Find(Globals.PAGE_EXER_ADD_DET.gameObject.name).transform.Find("CanvasRepsT").GetComponent<Canvas>().gameObject.SetActive(true);
        }
    }
    public void CheckNumberOfSeries()
    {
        Canvas cnvRest = GameObject.Find(Globals.PAGE_EXER_ADD_DET.gameObject.name).transform.Find("CanvasRest/CanvasRest").GetComponent<Canvas>();
        if (GameObject.Find(Globals.PAGE_EXER_ADD_DET.gameObject.name).transform.Find("CanvasSeries/InputFieldSeries").GetComponent<TMP_InputField>().text == "1")
            cnvRest.gameObject.SetActive(false);
        else
            cnvRest.gameObject.SetActive(true);
    }
    public void ChangeTime(string AToREoRI_MoS_DoI)
    {
        int iValue;
        string sValue;

        TextMeshProUGUI Testo;
        switch (AToREoRI_MoS_DoI.Substring(0, 2))
        {
            case "AT":
                Testo = Globals.PAGE_EXER_ADD_DET.transform.Find("CanvasRest/CanvasWait/txtTime").GetComponent<TextMeshProUGUI>();
                break;
            case "RE":
                Testo = Globals.PAGE_EXER_ADD_DET.transform.Find("CanvasRest/CanvasRest/txtTime").GetComponent<TextMeshProUGUI>();
                break;
            default:
                Testo = Globals.PAGE_EXER_ADD_DET.transform.Find("CanvasRepsT/txtTime").GetComponent<TextMeshProUGUI>();
                break;
        }

        int Index;
        if (AToREoRI_MoS_DoI.Substring(3, 1) == "M")
            Index = 0;
        else
            Index = 5;

        iValue = int.Parse(Testo.text.Substring(Index, 2));

        if (AToREoRI_MoS_DoI.Substring(5, 1) == "I")
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
    public void CheckValueIF(string sTesto, int iMax)
    {
        int num;

        if (!int.TryParse(sTesto, out num))
        {
            this.GetComponent<TMP_InputField>().text = "0";
        }
        else
        {
            if (num > 100)
            {
                this.GetComponent<TMP_InputField>().text = iMax.ToString();
            }
            if (num < 0)
            {
                this.GetComponent<TMP_InputField>().text = "0";
            }
        }
    }
}
