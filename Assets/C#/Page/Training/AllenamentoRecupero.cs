using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class AllenamentoRecupero : MonoBehaviour
{
    public void Carica(int Secondi)
    {
        Globals.STARTING_TIME = Secondi;
        Globals.CURRENT_TIME = Secondi;
        Globals.BOOL = false;
    }
    private void Start()
    {
        if (Globals.COUNT_TIMER > 1)
            Destroy(this);
    }
    void Update()
    {
        if (Globals.ALLENAMENTO_MODE != "Null")
        {
            if (Globals.BOOL == false)
            {
                if (Globals.CURRENT_TIME > 0)
                {
                    Globals.CURRENT_TIME -= 1 * Time.deltaTime;
                    var TimeConverted = TimeSpan.FromSeconds(Globals.CURRENT_TIME);

                    AllenamentoBottoni bottoni = gameObject.AddComponent<AllenamentoBottoni>();
                    bottoni.PrintTime(TimeConverted);
                }
                else
                {
                    Finito();
                }
            }
        }
    }
    public void Finito()
    {
        AppManager.Instance.allenamento.Next();
    }
}