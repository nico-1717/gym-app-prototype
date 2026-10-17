using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Muscles : MonoBehaviour
{
    // 0 = Spalle
    // 1 = Petto
    // 2 = Schiena
    // 3 = Braccia
    // 4 = ABS
    // 5 = Gambe
    bool[] bPastMuscolo;
    static bool[] bNewMuscolo;
    string sPage;

    public void Muscoli_Show(bool[] Muscolo, string Page)
    {
        sPage = Page;
        bPastMuscolo = new bool[Muscolo.Length];
        bNewMuscolo = new bool[Muscolo.Length];
        for (int row = 0; row < Muscolo.Length; row++)
        {
            bPastMuscolo[row] = Muscolo[row];
            bNewMuscolo[row] = Muscolo[row];
        }
        Muscoli_Carica();
        AppManager.Instance.objects.ShowPopup(Globals.PP_MUSCOLI);

        Globals.BTN_SPALLE.onClick.RemoveAllListeners();
        Globals.BTN_PETTO.onClick.RemoveAllListeners();
        Globals.BTN_SCHIENA.onClick.RemoveAllListeners();
        Globals.BTN_BRACCIA.onClick.RemoveAllListeners();
        Globals.BTN_ABS.onClick.RemoveAllListeners();
        Globals.BTN_GAMBE.onClick.RemoveAllListeners();

        Globals.BTN_SPALLE.onClick.AddListener(() => Muscoli_Switch(0));
        Globals.BTN_PETTO.onClick.AddListener(() => Muscoli_Switch(1));
        Globals.BTN_SCHIENA.onClick.AddListener(() => Muscoli_Switch(2));
        Globals.BTN_BRACCIA.onClick.AddListener(() => Muscoli_Switch(3));
        Globals.BTN_ABS.onClick.AddListener(() => Muscoli_Switch(4));
        Globals.BTN_GAMBE.onClick.AddListener(() => Muscoli_Switch(5));
    }
    private void Muscoli_Carica()
    {
        ColorBlock theColor;
        Globals.BTN_CONFIRM.onClick.RemoveAllListeners();
        Globals.BTN_CONFIRM.onClick.AddListener(() => Muscoli_Chiudi(true));
        Globals.BTN_CANCEL.onClick.RemoveAllListeners();
        Globals.BTN_CANCEL.onClick.AddListener(() => Muscoli_Chiudi(false));
        Button button;
        for (int row = 0; row < bPastMuscolo.Length; row++)
        {
            switch (row)
            {
                case 0:
                    button = Globals.BTN_SPALLE;
                    break;
                case 1:
                    button = Globals.BTN_PETTO;
                    break;
                case 2:
                    button = Globals.BTN_SCHIENA;
                    break;
                case 3:
                    button = Globals.BTN_BRACCIA;
                    break;
                case 4:
                    button = Globals.BTN_ABS;
                    break;
                case 5:
                    button = Globals.BTN_GAMBE;
                    break;
                default:
                    button = Globals.BTN_SPALLE;
                    break;
            }
            theColor = button.colors;
            if (bNewMuscolo[row] == true)
            {
                theColor.highlightedColor = Color.red;
                theColor.normalColor = Color.yellow;
                theColor.pressedColor = Color.yellow;
                theColor.selectedColor = Color.yellow;
            }
            else
            {
                theColor.highlightedColor = Color.red;
                theColor.normalColor = Color.white;
                theColor.pressedColor = Color.white;
                theColor.selectedColor = Color.white;
            }
            button.colors = theColor;
        }
    }
    private void Muscoli_Switch(int iMuscolo)
    {
        bNewMuscolo[iMuscolo] = !bNewMuscolo[iMuscolo];
        Muscoli_Carica();
    }
    private void Muscoli_Chiudi(bool bConferma)
    {
        AppManager.Instance.objects.ClosePopup();

        switch (sPage)
        {
            case "InsertEsercizio":
                Insert insert = gameObject.AddComponent<Insert>();
                if (bConferma == true)
                    insert.Page_InsertEsercizio(bNewMuscolo);
                else
                    insert.Page_InsertEsercizio(bPastMuscolo);
                break;
            case "FilterSearch":
                FilterSearchMusc filterSearch = gameObject.GetComponent<FilterSearchMusc>();
                if (bConferma == true)
                    filterSearch.MusclesClose(bNewMuscolo);
                else
                    filterSearch.MusclesClose(bPastMuscolo);
                break;
        }
    }
}
