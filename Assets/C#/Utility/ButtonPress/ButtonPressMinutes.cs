using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using TMPro;

public class ButtonPressMinutes : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float MinutesPressed1 = 0.5f;
    public float MinutesPressed2 = 2f;
    public bool buttonPressed;
    public string IncreseOrDecrease;

    private TextMeshProUGUI Testo;

    float Minutes = 0f;
    float MinutesSpeed = 0f;
    float Speed = 0.3f;
    float FastSpeed = 0.1f;

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;
    }
    void OnEnable()
    {
        Testo = gameObject.transform.parent.Find("txtTime").GetComponent<TextMeshProUGUI>();
    }
    void Start()
    {
        buttonPressed = false;
    }
    void Update()
    {
        int iValueMin;

        string sValueMin;

        bool bChange;

        if (buttonPressed == true)
        {
            Minutes += 1 * Time.deltaTime;
            MinutesSpeed += 1 * Time.deltaTime;
        }
        else
        {
            Minutes = 0f;
            MinutesSpeed = 0f;
        }

        bChange = false;

        if (Minutes > MinutesPressed2)
        {
            if (MinutesSpeed > FastSpeed)
            {
                bChange = true;
            }
        }
        else
        {
            if (Minutes > MinutesPressed1)
            {
                if (MinutesSpeed > Speed)
                {
                    bChange = true;
                }
            }
        }


        if (bChange == true)
        {
            MinutesSpeed = 0f;
            iValueMin = int.Parse(Testo.text.Substring(0, 2));

            if (IncreseOrDecrease == "Increase")
            {
                iValueMin += 1;
                if (iValueMin > 59)
                {
                    iValueMin = 0;
                }
            }
            else
            {
                if (iValueMin <= 0)
                {
                    iValueMin = 60;
                }
                iValueMin -= 1;
            }
            if (iValueMin < 10)
                sValueMin = "0" + iValueMin;
            else
                sValueMin = iValueMin.ToString();
            Testo.text = sValueMin + " : " + Testo.text.Substring(5, 2);
        }
    }
}
