using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using TMPro;

public class ButtonPressTime : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float SecondsPressed1 = 0.5f;
    public float SecondsPressed2 = 2f;
    public float SecondsPressed3 = 4f;
    public TextMeshProUGUI Testo;
    public bool buttonPressed;
    public string IncreseOrDecrease;

    float Seconds = 0f;
    float SecondsSpeed = 0f;
    float Speed = 0.3f;
    float FastSpeed = 0.1f;
    float FastFastSpeed = 0.03f;

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;
    }

    void Start()
    {
        buttonPressed = false;
    }
    void Update()
    {
        int iValueMin;
        int iValueSec;

        string sValueMin;
        string sValueSec;

        bool bChange;

        if (buttonPressed == true)
        {
            Seconds += 1 * Time.deltaTime;
            SecondsSpeed += 1 * Time.deltaTime;
        }
        else
        {
            Seconds = 0f;
            SecondsSpeed = 0f;
        }

        bChange = false;
        if (Seconds > SecondsPressed3)
        {
            if (SecondsSpeed > FastFastSpeed)
            {
                bChange = true;
            }
        }
        else
        {
            if (Seconds > SecondsPressed2)
            {
                if (SecondsSpeed > FastSpeed)
                {
                    bChange = true;
                }
            }
            else
            {
                if (Seconds > SecondsPressed1)
                {
                    if (SecondsSpeed > Speed)
                    {
                        bChange = true;
                    }
                }
            }
        }

        if (bChange == true)
        {
            SecondsSpeed = 0f;
            iValueMin = int.Parse(Testo.text.Substring(0, 2));
            iValueSec = int.Parse(Testo.text.Substring(3, 2));

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
    }
}
