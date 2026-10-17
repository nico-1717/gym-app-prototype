using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FilterSearch : MonoBehaviour
{
    public Button btnFiltered;
    private static TMP_InputField inputField;
    private static Button btnReset;
    public void Init()
    {
        inputField = this.transform.Find("InputField").gameObject.GetComponent<TMP_InputField>();
        btnReset = this.transform.Find("InputField/btnReset").GetComponent<Button>();

        inputField.onValueChanged.AddListener(Filter);

        btnReset.onClick.RemoveAllListeners();
        btnReset.onClick.AddListener(ResetInputFieldTextSchede);

        btnFiltered.transform.Find("Text").GetComponent<TextMeshProUGUI>().overflowMode = TextOverflowModes.Truncate;
    }
    void OnEnable()
    {
        ResetInputFieldTextSchede();
    }
    public void Filter(string sSearch)
    {
        GameObject Parent = btnFiltered.transform.parent.gameObject;

        int ChildNum = Parent.transform.childCount;
        while (ChildNum > 1)
        {
            Parent.transform.GetChild(ChildNum - 1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(btnFiltered.gameObject.GetComponent<RectTransform>().rect.width, btnFiltered.gameObject.GetComponent<RectTransform>().rect.height);
            if (Parent.transform.GetChild(ChildNum - 1).gameObject.transform.Find("Text").GetComponent<TextMeshProUGUI>().text.ToLower().IndexOf(sSearch.ToLower()) == -1)
                Parent.transform.GetChild(ChildNum - 1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(btnFiltered.gameObject.GetComponent<RectTransform>().rect.width, 0);
            ChildNum -= 1;
        }
    }
    public void ResetInputFieldTextSchede()
    {
        inputField.text = "";
        inputField.Select();
    }
}
