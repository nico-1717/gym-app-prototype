using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class IntIncOrDecr : MonoBehaviour
{
    private TMP_InputField inputField;
    private Button btnUp;
    private Button btnDown;
    public int Min = 0;
    public int Max = 100;
    void OnEnable()
    {
        string searchStringUp = "Up";
        string searchStringDown = "Down";
        Transform parentTransform = gameObject.transform;

        foreach (Transform child in parentTransform)
        {
            if (child.name.Contains(searchStringUp))
            {
                btnUp = child.GetComponent<Button>();
            }
            if (child.name.Contains(searchStringDown))
            {
                btnDown = child.GetComponent<Button>();
            }
        }

        inputField = gameObject.GetComponentInChildren<TMP_InputField>();

        btnDown.onClick.RemoveAllListeners();
        btnUp.onClick.RemoveAllListeners();
        btnDown.onClick.AddListener(() => Execute("D"));
        btnUp.onClick.AddListener(() => Execute("I"));
    }
    public void Execute(string IoD)
    {
        if (IoD == "I")
        {
            if (int.Parse(inputField.text) < Max)
                inputField.text = (int.Parse(inputField.text) + 1).ToString();
        }
        else
        {
            if (int.Parse(inputField.text) > Min)
                inputField.text = (int.Parse(inputField.text) - 1).ToString();
        }
    }
}
