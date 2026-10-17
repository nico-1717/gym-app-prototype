using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SelectScheda : MonoBehaviour
{
    #region Scheda
    public void CaricaScheda()
    {
        AppManager.Instance.objects.PagesTraining("Schede");
        SvuotaPaginaSchede();

        EserciziAllenamento eserciziAllenamento = gameObject.AddComponent<EserciziAllenamento>();
        DataTable dtTable = AppManager.Instance.sql.SelectAllSchede("", Globals.COL_SCH_NOME);

        Button[] Duplicato = new Button[dtTable.Rows.Count];
        for (int row = 0; row < dtTable.Rows.Count; row++)
        {
            CaricaBottoneSchede(dtTable, Duplicato, row);

            int IDscheda = int.Parse(dtTable.Rows[row][Globals.COL_SCH_ID].ToString());
            Duplicato[row].onClick.RemoveAllListeners();
            Duplicato[row].onClick.AddListener(() => eserciziAllenamento.InitEs(IDscheda));
        }
        Globals.BTN_ADD_SCHEDA_ALNMT.GetComponent<Button>().gameObject.SetActive(false);

    }
    private void CaricaBottoneSchede(DataTable dtTable, Button[] Duplicato, int row)
    {
        Duplicato[row] = Instantiate(Globals.BTN_ADD_SCHEDA_ALNMT);
        Duplicato[row].name = "SchedaN" + row;
        Duplicato[row].transform.SetParent((GameObject.Find("Content")).transform);
        Duplicato[row].transform.SetSiblingIndex(row + 1);
        GameObject.Find(Duplicato[row].gameObject.name).GetComponent<TextMeshProUGUI>().text = dtTable.Rows[row][Globals.COL_SCH_NOME].ToString();
    }
    private void SvuotaPaginaSchede()
    {
        Globals.BTN_ADD_SCHEDA_ALNMT.GetComponent<Button>().gameObject.SetActive(true);

        GameObject Parent = Globals.BTN_ADD_SCHEDA_ALNMT.transform.parent.gameObject;

        while (Parent.transform.childCount > 1)
        {
            DestroyImmediate(Parent.transform.GetChild(Parent.transform.childCount - 1).gameObject);
        }
    }
    #endregion
}
