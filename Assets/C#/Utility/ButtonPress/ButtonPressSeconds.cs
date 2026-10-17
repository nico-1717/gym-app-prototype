using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using TMPro;

public class ButtonPressSeconds : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float SecondsPressed1 = 0.5f;
    public float SecondsPressed2 = 2f;
    public bool buttonPressed;
    public string IncreseOrDecrease;

    private TextMeshProUGUI Testo;

    private float Seconds = 0f;
    private float SecondsSpeed = 0f;
    private float Speed = 0.3f;
    private float FastSpeed = 0.1f;

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
        int iValueSec;

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
        

        if (bChange == true)
        {
            SecondsSpeed = 0f;
            iValueSec = int.Parse(Testo.text.Substring(5, 2));

            if (IncreseOrDecrease == "Increase")
            {
                iValueSec += 1;
                if (iValueSec > 59)
                {
                    iValueSec = 0;
                }
            }
            else
            {
                if (iValueSec <= 0)
                {
                    iValueSec = 60;
                }
                iValueSec -= 1;
            }
            if (iValueSec < 10)
                sValueSec = "0" + iValueSec;
            else
                sValueSec = iValueSec.ToString();
            Testo.text = Testo.text.Substring(0, 2) + " : " + sValueSec;
        }
    }
}
