using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FilterSearchMusc : MonoBehaviour
{
    public Button btnFiltered;
    private static Button btnFilter;
    private static Button btnReset;
    private static TMP_InputField inputField;
    public void Init()
    {
        btnFilter = gameObject.transform.Find("svFilter/Viewport/contentFilter/btnFilter").GetComponent<Button>();
        btnReset = gameObject.transform.Find("InputField/btnReset").GetComponent<Button>();
        inputField = gameObject.transform.Find("InputField").GetComponent<TMP_InputField>();

        btnFiltered.transform.Find("Text").GetComponent<TextMeshProUGUI>().overflowMode = TextOverflowModes.Truncate;

        Button btnMuscles = inputField.transform.parent.transform.Find("btnMuscles").GetComponent<Button>();
        btnMuscles.onClick.RemoveAllListeners();
        btnMuscles.onClick.AddListener(MusclesShow);

        btnReset.onClick.RemoveAllListeners();
        btnReset.onClick.AddListener(ResetInputFieldTextSchede);

        inputField.onValueChanged.AddListener(Filter);
    }
    void OnEnable()
    {
        bMuscolo = new bool[6];
        for (int row = 0; row < bMuscolo.Length; row++)
        {
            bMuscolo[row] = false;
        }
        btnFilter.gameObject.SetActive(false);

        SvuotaFilter();
    }
    public void Filter(string sSearch)
    {
        GameObject Parent = btnFiltered.transform.parent.gameObject;

        int ChildNum = Parent.transform.childCount;
        while (ChildNum > 1)
        {
            Parent.transform.GetChild(ChildNum - 1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(btnFiltered.gameObject.GetComponent<RectTransform>().rect.width, btnFiltered.gameObject.GetComponent<RectTransform>().rect.height);
            if (Parent.transform.GetChild(ChildNum - 1).Find("Text").GetComponent<TextMeshProUGUI>().text.ToLower().IndexOf(sSearch.ToLower()) == -1)
                Parent.transform.GetChild(ChildNum - 1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(btnFiltered.gameObject.GetComponent<RectTransform>().rect.width, 0);
            ChildNum -= 1;
        }

        FilterMuscles();
    }
    public void ResetInputFieldTextSchede()
    {
        TMP_InputField InputField = this.transform.Find("InputField").gameObject.GetComponent<TMP_InputField>();

        InputField.text = "";
        InputField.Select();
    }
    static bool[] bMuscolo;
    static string[] sMuscolo;
    private void FilterMuscles()
    {
        GameObject ParentFilter = btnFilter.transform.parent.gameObject;
        if (ParentFilter.transform.childCount > 1)
        {
            string[] Muscles = new string[ParentFilter.transform.childCount - 1];
            int row = 0;
            while (row < ParentFilter.transform.childCount - 1)
            {
                Muscles[row] = ParentFilter.transform.GetChild(row + 1).Find("txtName").GetComponent<TextMeshProUGUI>().text;
                row++;
            }

            GameObject ParentEs = btnFiltered.transform.parent.gameObject;
            int ChildNum = ParentEs.transform.childCount;
            while (ChildNum > 1)
            {
                foreach (string mus in Muscles)
                {
                    if (ParentEs.transform.GetChild(ChildNum - 1).gameObject.transform.Find("txtMuscles").GetComponent<TextMeshProUGUI>().text.ToLower().IndexOf(mus.ToLower()) == -1)
                    {
                        ParentEs.transform.GetChild(ChildNum - 1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(btnFiltered.gameObject.GetComponent<RectTransform>().rect.width, 0);
                        break;
                    }
                }
                ChildNum -= 1;
            }
        }
    }
    public void MusclesShow()
    {
        AppManager.Instance.objects.Button_Area("Answer");
        Muscles muscles = gameObject.AddComponent<Muscles>();
        muscles.Muscoli_Show(bMuscolo, "FilterSearch");
    }
    public void MusclesClose(bool[] NewMuscolo)
    {
        AppManager.Instance.objects.Button_Area("None");

        bMuscolo = NewMuscolo;
        for (int row = 0; row < NewMuscolo.Length; row++)
        {
            bMuscolo[row] = NewMuscolo[row];
        }
        UpdateMuscles();
    }
    private void UpdateMuscles()
    {
        SvuotaFilter();

        btnFilter.gameObject.SetActive(true);
        int nFilter = 1;
        for (int row = 0; row < bMuscolo.Length; row++)
        {
            if (bMuscolo[row] == true)
            {
                CaricaBtn(nFilter, row);
                nFilter += 1;
            }
        }
        btnFilter.gameObject.SetActive(false);
        Filter(inputField.text);
    }
    private void CaricaBtn(int nFilter, int nMuscolo)
    {
        GameObject Parent = btnFilter.transform.parent.gameObject;
        Button btnInstantiate = Instantiate(btnFilter);
        btnInstantiate.name = "Filter" + nFilter;
        btnInstantiate.transform.SetParent(Parent.transform);
        btnInstantiate.transform.SetSiblingIndex(nFilter);
        btnInstantiate.transform.Find("txtName").GetComponent<TextMeshProUGUI>().text = NameMuscles(nMuscolo);
        btnInstantiate.transform.Find("btnDel").GetComponent<Button>().onClick.RemoveAllListeners();
        btnInstantiate.transform.Find("btnDel").GetComponent<Button>().onClick.AddListener(() => DelFilter(nMuscolo));
    }
    private string NameMuscles(int row)
    {
        sMuscolo = new string[6];

        sMuscolo[0] = "Spalle";
        sMuscolo[1] = "Petto";
        sMuscolo[2] = "Schiena";
        sMuscolo[3] = "Braccia";
        sMuscolo[4] = "ABS";
        sMuscolo[5] = "Gambe";

        return sMuscolo[row];
    }
    private void DelFilter(int nMuscolo)
    {
        bMuscolo[nMuscolo] = !bMuscolo[nMuscolo];

        UpdateMuscles();
    }
    private void SvuotaFilter()
    {
        GameObject Parent = btnFilter.transform.parent.gameObject;
        while (Parent.transform.childCount > 1)
        {
            DestroyImmediate(Parent.transform.GetChild(Parent.transform.childCount - 1).gameObject);
        }
    }
}
