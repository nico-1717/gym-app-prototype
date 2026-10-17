using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Data;

public class Objects : MonoBehaviour
{
    #region MENU
    public void Scenes(string sScena)
    {
        SwitchCanvas("Menu");
        Globals.SCENE_MENU.SetActive(false);
        Globals.SCENE_SCHEDE.SetActive(false);
        Globals.SCENE_ESERCIZI.SetActive(false);
        Globals.IMG_BYTE_INSERT = null;
        switch (sScena)
        {
            case "Menu":
                Globals.SCENE_MENU.SetActive(true);
                Globals.BTN_HOME.gameObject.SetActive(false);
                Globals.BTN_BACK.gameObject.SetActive(false);
                break;
            case "Schede":
                Globals.SCENE_SCHEDE.SetActive(true);
                Globals.BTN_HOME.gameObject.SetActive(true);
                Globals.BTN_BACK.gameObject.SetActive(true);
                break;
            case "Esercizi":
                Globals.SCENE_ESERCIZI.SetActive(true);
                Globals.BTN_HOME.gameObject.SetActive(true);
                Globals.BTN_BACK.gameObject.SetActive(true);
                break;
            default:
                Globals.SCENE_MENU.SetActive(true);
                Globals.BTN_HOME.gameObject.SetActive(false);
                Globals.BTN_BACK.gameObject.SetActive(true);
                break;
        }
    }
    public void Pages(string sPage)
    {
        PageSwiper pageSwiper = GetComponent<PageSwiper>();
        Vector3 EndPosition;
        Vector3 StartPosition = Globals.PAGE_MENU.gameObject.transform.position;
        float x = -Globals.PAGE_MENU_TRAINING.GetComponent<RectTransform>().anchoredPosition.x + 300;
        switch (sPage)
        {
            case "Stats":
                x = -Globals.PAGE_MENU_STATS.GetComponent<RectTransform>().anchoredPosition.x + 300;
                break;
            case "Training":
                x = -Globals.PAGE_MENU_TRAINING.GetComponent<RectTransform>().anchoredPosition.x + 300;
                break;
            case "Edit":
                x = -Globals.PAGE_MENU_EDIT.GetComponent<RectTransform>().anchoredPosition.x + 300;
                break;
        }
        EndPosition = Globals.PAGE_MENU.gameObject.transform.position;
        EndPosition.x = x;
        StartCoroutine(SmoothMove(StartPosition, EndPosition, 0.5f));
        Globals.panelLocation = EndPosition;
    }
    IEnumerator SmoothMove(Vector3 startpos, Vector3 endpos, float seconds)
    {
        float t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            Globals.PAGE_MENU.gameObject.transform.position = Vector3.Lerp(startpos, endpos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
    }

    public void Tabs(int iTab)
    {
        Globals.TAB_STATS.GetComponent<Image>().color = Color.white;
        Globals.TAB_TRAINING.GetComponent<Image>().color = Color.white;
        Globals.TAB_EDIT.GetComponent<Image>().color = Color.white;
        switch (iTab)
        {
            case 0:
                Globals.TAB_STATS.GetComponent<Image>().color = Color.red;
                break;
            case 1:
                Globals.TAB_TRAINING.GetComponent<Image>().color = Color.red;
                break;
            case 2:
                Globals.TAB_EDIT.GetComponent<Image>().color = Color.red;
                break;
            default:
                Globals.TAB_TRAINING.GetComponent<Image>().color = Color.red;
                break;
        }
    }

    public void ChangeToTab(int Tab)
    {
        switch (Tab)
        {
            case 0:
                Pages("Stats");
                Tabs(0);
                break;
            case 1:
                Pages("Training");
                Tabs(1);
                break;
            case 2:
                Pages("Edit");
                Tabs(2);
                break;
            default:
                Pages("Training");
                Tabs(1);
                break;
        }

    }

    public void TabsSwitchState(string sState)
    {
        Scenes("Schede");
        Globals.PAGE_TAB_VIEW.SetActive(false);
        switch (sState)
        {
            case "View&C":
                Globals.PAGE_TAB_VIEW.SetActive(true);
                AppManager.Instance.schede.Carica();
                break;
            case "View":
                Globals.PAGE_TAB_VIEW.SetActive(true);
                break;
            default:
                break;
        }

    }

    public void ExercisesSwitchState(string sState)
    {
        Insert insert = gameObject.AddComponent<Insert>();
        Scenes("Esercizi");
        Globals.PAGE_EXER_VIEW.SetActive(false);
        Globals.PAGE_EXER_VIEW_DET.SetActive(false);
        Globals.PAGE_EXER_ADD.SetActive(false);
        Globals.PAGE_EXER_ADD_DET.SetActive(false);
        switch (sState)
        {
            case "View":
                Globals.PAGE_EXER_VIEW.SetActive(true);
                break;
            case "View&C":
                Globals.PAGE_EXER_VIEW.SetActive(true);
                AppManager.Instance.esercizi.CaricaEsercizi();
                break;
            case "ViewDetails":
                Globals.PAGE_EXER_VIEW_DET.SetActive(true);
                break;
            case "Add":
                Button_Area("Answer");
                insert.Init_InsertEsercizio();
                break;
            case "Modify":
                Button_Area("Answer");
                break;
            case "AddDetails":
                Globals.PAGE_EXER_ADD_DET.SetActive(true);
                Button_Area("Answer");
                break;
            default:
                break;
        }

    }

    public void Button_Area(string sArea)
    {
        AppManager.Instance.objects.ShowPopup(Globals.PP_BUTTON);
        Globals.TAB_ANSWER.SetActive(false);
        switch (sArea)
        {
            case "Answer":
                Globals.TAB_ANSWER.SetActive(true);
                Globals.PP_BUTTON.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
                Globals.PP_BUTTON.GetComponent<RectTransform>().offsetMax = new Vector2(0, (150 - 1024));
                break;
            case "None":
                Globals.TAB_ANSWER.SetActive(false);
                AppManager.Instance.objects.ClosePopup();
                break;
            default:
                break;
        }

    }

    public string CheckActiveScena()
    {
        string sScena = "Empty";
        if (Globals.SCENE_MENU.activeSelf == true)
        {
            sScena = "Menu";
        }
        if (Globals.SCENE_ESERCIZI.activeSelf == true)
        {
            sScena = "Esercizi";
        }
        if (Globals.SCENE_SCHEDE.activeSelf == true)
        {
            sScena = "Scheda";
        }
        return sScena;
    }
    public void ClosePopup(GameObject ogPopup = null)
    {
        //Se gli passi un popop disattiva solo quello (controllando che sia l'unico attivo, così può disattivare anche il canvas), altrimenti tutti
        if (ogPopup != null)
        {
            ogPopup.SetActive(false);

            bool bTrovato = false;
            for (int i = 0; i < Globals.CANVAS_POPUP.transform.childCount; i++)
            {
                if (Globals.CANVAS_POPUP.transform.GetChild(i).gameObject.activeSelf)
                {
                    bTrovato = true;
                    break;
                }
            }
            if (!bTrovato)
                Globals.CANVAS_POPUP.gameObject.SetActive(false);
        }
        else
        {
            for (int i = 0; i < Globals.CANVAS_POPUP.transform.childCount; i++)
                Globals.CANVAS_POPUP.transform.GetChild(i).gameObject.SetActive(false);
            Globals.CANVAS_POPUP.gameObject.SetActive(false);
        }
    }
    public void ShowPopup(GameObject ogPopup)
    {
        Globals.CANVAS_POPUP.gameObject.SetActive(true);
        ogPopup.SetActive(true);
    }
    public void BtnHome()
    {
        Scenes("Menu");
        ChangeToTab(1);
        ClosePopup();

        AppManager.Instance.esercizi.SvuotaPaginaEsercizi();
        AppManager.Instance.schede.SvuotaPaginaSchede();
    }
    #endregion
    #region TRAINING
    public void PagesTraining(string sPage)
    {
        SwitchCanvas("Start");
        SelectScheda selectScheda = gameObject.AddComponent<SelectScheda>();
        Globals.PAGE_ALLENAMENTO.SetActive(false);
        Globals.PAGE_ESERCIZI.SetActive(false);
        Globals.PAGE_SCHEDE.SetActive(false);
        Globals.PAGE_FINITO.SetActive(false);
        switch (sPage)
        {
            case "Schede":
                Globals.PAGE_SCHEDE.SetActive(true);
                break;
            case "Esercizi":
                Globals.PAGE_ESERCIZI.SetActive(true);
                break;
            case "Allenamento":
                Globals.PAGE_ALLENAMENTO.SetActive(true);
                break;
            case "Finito":
                Globals.PAGE_FINITO.SetActive(true);
                break;
            case "Carica":
                selectScheda.CaricaScheda();
                break;
        }
    }
    public void btnEsciAllenamento()
    {
        DataTable dataTable = AppManager.Instance.sql.SelectAllAttivita(" WHERE ll." + Globals.COL_ALL_ID + " = " + Globals.ID_ALLENAMENTO, "ll." + Globals.COL_ALL_ID);
        for (int row = 0; row < dataTable.Rows.Count; row++)
        {
            AppManager.Instance.sql.DeleteAttivita(int.Parse(dataTable.Rows[row][Globals.COL_ATT_ID].ToString()));
        }
        AppManager.Instance.sql.DeleteAllenamento(Globals.ID_ALLENAMENTO);

        ChangeToTab(1);
        Scenes("Menu");
    }
    #endregion
    #region Web
    public void SelectWeb()
    {
        SwitchCanvas("Web");
    }
    #endregion
    private void SwitchCanvas(string sCanvas)
    {
        Globals.CANVAS_MENU.gameObject.SetActive(false);
        Globals.CANVAS_START.gameObject.SetActive(false);
        Globals.CANVAS_WEB.gameObject.SetActive(false);
        switch (sCanvas)
        {
            case "Menu":
                Globals.CANVAS_MENU.gameObject.SetActive(true);

                Globals.PAGE_MENU_STATS.SetActive(true);
                Globals.PAGE_MENU_TRAINING.SetActive(true);
                Globals.PAGE_MENU_EDIT.SetActive(true);

                GameObject Pagina;
                Pagina = Globals.PAGE_MENU_STATS;
                Pagina.GetComponent<RectTransform>().anchoredPosition = new Vector2(-Screen.width, Pagina.GetComponent<RectTransform>().anchoredPosition.y);
                Pagina = Globals.PAGE_MENU_EDIT;
                Pagina.GetComponent<RectTransform>().anchoredPosition = new Vector2(Screen.width, Pagina.GetComponent<RectTransform>().anchoredPosition.y);
                //Position Page Menu
                Globals.X_PAGE_MENU_STATS = -Globals.PAGE_MENU_STATS.GetComponent<RectTransform>().anchoredPosition.x + 300;
                Globals.X_PAGE_MENU_TRAINING = -Globals.PAGE_MENU_TRAINING.GetComponent<RectTransform>().anchoredPosition.x + 300;
                Globals.X_PAGE_MENU_EDIT = -Globals.PAGE_MENU_EDIT.GetComponent<RectTransform>().anchoredPosition.x + 300;
                break;
            case "Start":
                Globals.CANVAS_START.gameObject.SetActive(true);
                break;
            case "Web":
                Globals.CANVAS_WEB.gameObject.SetActive(true);
                Globals.BTN_BACK.gameObject.SetActive(true);
                break;
        }

    }
}
