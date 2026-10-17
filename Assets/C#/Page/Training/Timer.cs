using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    float CurrentTime = 0f;
    float StartingTime = 70f;

    //SerializeField serializza una variabile privata, ovvero -> le variabili private non vengono mostrate nell'editor Unity, ma se tu non vuoi renderla pubblica, puoi serializzarla, in questo modo verrà visualizzata nell'editor nonostante sia privata
    [SerializeField] Text ciao;
    void Start()
    {
        CurrentTime = StartingTime;
    }
    public void SetTime(float Time)
    {
        StartingTime = Time;
    }
    void Update()
    {
        if (CurrentTime > 0)
        {
            CurrentTime -= 1 * Time.deltaTime;
        }
        else
        {
            CurrentTime = 0;
        }
        var TimeConverted = TimeSpan.FromSeconds(CurrentTime);
        if (TimeConverted.ToString(@"mm") == "00")
        {
            ciao.text = TimeConverted.ToString(@"ss");
            ciao.fontSize = 300;
        }
        else
        {
            ciao.text = TimeConverted.ToString(@"mm\:ss");
            ciao.fontSize = 150;
        }
    }
}
