using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AllenamentoBottoni : MonoBehaviour
{
    static private TextMeshProUGUI txtTimer;
    static private Image iProgressBar;
    private void Start()
    {
        txtTimer = Globals.PAGE_ALLENAMENTO.transform.Find("Recupero/CnvTimer/txtTimer").GetComponent<TextMeshProUGUI>();
        iProgressBar = Globals.PAGE_ALLENAMENTO.transform.Find("Recupero/CnvTimer/RadialProgressBar/Fill").GetComponent<Image>();
    }
    public void btnPause()
    {
        Globals.BOOL = !Globals.BOOL;
    }
    public void btnIncreaseRest(int Amount)
    {
        Globals.CURRENT_TIME += Amount;
        PrintTime(TimeSpan.FromSeconds(Globals.CURRENT_TIME));
    }
    public void btnDecreaseRest(int Amount)
    {
        Globals.CURRENT_TIME -= Amount;
        if (Globals.CURRENT_TIME > 0)
        {
            PrintTime(TimeSpan.FromSeconds(Globals.CURRENT_TIME));
        }
        else
        {
            Globals.CURRENT_TIME = 00;
        }
    }
    public void PrintTime(TimeSpan TimeConverted)
    {
        if (TimeConverted.ToString(@"mm") == "00")
        {
            if (Globals.ALLENAMENTO_MODE == "Recupero")
            {
                txtTimer.text = TimeConverted.ToString(@"ss");
                txtTimer.fontSize = 300;
            }
            else
            {
                Globals.CNV_ESE_TIMER.gameObject.transform.Find("txtTimer").GetComponent<TextMeshProUGUI>().text = TimeConverted.ToString(@"ss");
                Globals.CNV_ESE_TIMER.gameObject.transform.Find("txtTimer").GetComponent<TextMeshProUGUI>().fontSize = 300;
            }
        }
        else
        {
            if (Globals.ALLENAMENTO_MODE == "Recupero")
            {
                txtTimer.text = TimeConverted.ToString(@"mm\:ss");
                txtTimer.fontSize = 150;
            }
            else
            {
                Globals.CNV_ESE_TIMER.gameObject.transform.Find("txtTimer").GetComponent<TextMeshProUGUI>().text = TimeConverted.ToString(@"mm\:ss");
                Globals.CNV_ESE_TIMER.gameObject.transform.Find("txtTimer").GetComponent<TextMeshProUGUI>().fontSize = 150;
            }
        }
        UpdateProgressBar(TimeConverted);
    }
    public void UpdateProgressBar(TimeSpan TimeConverted)
    {
        if (Globals.ALLENAMENTO_MODE == "Recupero")
        {
            iProgressBar.fillAmount = (60 - ((float)TimeConverted.TotalMilliseconds) / 1000) / 60;
        }
        else
        {
            Globals.CNV_ESE_TIMER.gameObject.transform.Find("RadialProgressBar").GetComponent<Image>().gameObject.transform.Find("Fill").GetComponent<Image>().fillAmount = (Globals.STARTING_TIME - Globals.CURRENT_TIME) / Globals.STARTING_TIME;
        }
    }
}
