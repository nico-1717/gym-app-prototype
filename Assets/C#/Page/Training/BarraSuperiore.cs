using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BarraSuperiore : MonoBehaviour
{
    float Cronometro;
    private void Start()
    {
        Cronometro = 0;
    }
    void Update()
    {
        if (Globals.PAGE_ALLENAMENTO.gameObject.activeSelf)
        {
            Globals.PAGE_ALLENAMENTO.gameObject.transform.Find("BarraSuperiore/Tempo/Ora").GetComponent<TextMeshProUGUI>().text = DateTime.Now.ToString("HH:mm");
            Cronometro += 1 * Time.deltaTime;
            var TimeConverted = TimeSpan.FromSeconds(Cronometro);
            Globals.PAGE_ALLENAMENTO.gameObject.transform.Find("BarraSuperiore/Tempo/Cronometro").GetComponent<TextMeshProUGUI>().text = TimeConverted.ToString(@"hh\:mm\:ss");
        }
    }
}
