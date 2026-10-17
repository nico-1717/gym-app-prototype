using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BarraEsercizi : MonoBehaviour
{
    public void beMain()
    {
        DataTable dtTable;

        string sCondition = " WHERE sc." + Globals.COL_SCH_ID + " = " + Globals.INTERO + " ";
        var tuple = AppManager.Instance.sql.SelectAllDettagli(sCondition, "det." + Globals.COL_DET_POSIZIONE);
        dtTable = tuple.Item1;

        Globals.BTN_PAST.gameObject.SetActive(true);
        Globals.BTN_THEN.gameObject.SetActive(true);

        Svuota(Globals.BTN_PAST);
        Svuota(Globals.BTN_THEN);

        bool Ultimo = true;

        string Recupero;
        int Posizione = 0;
        string Reps;
        for (int row = 0; row < dtTable.Rows.Count; row++)
        {
            for (int SerieN = 0; SerieN < int.Parse(dtTable.Rows[row][Globals.COL_DET_SERIE].ToString()); SerieN++)
            {
                if (SerieN == 0)
                    Recupero = dtTable.Rows[row][Globals.COL_DET_ATTESA].ToString();
                else
                    Recupero = dtTable.Rows[row][Globals.COL_DET_RECUPERO].ToString();
                if (Recupero != "00:00")
                {
                    Posizione += 1;
                    if (Posizione == Globals.POSIZIONE_ATTUALE + 1)
                    {
                        Globals.BTN_NEXT.GetComponent<TextMeshProUGUI>().text = Recupero;
                        Globals.BTN_NEXT.transform.Find("txtReps").GetComponent<TextMeshProUGUI>().text = "";
                        Globals.BTN_NEXT.GetComponent<Image>().color = Color.green;
                        Globals.BTN_NEXT.onClick.RemoveAllListeners();

                        Ultimo = false;
                    }
                    if (Posizione > Globals.POSIZIONE_ATTUALE + 1)
                    {
                        CaricaBottone(Recupero, Globals.BTN_THEN, Posizione, "", Color.green, default);
                    }
                }
                Posizione += 1;
                if (dtTable.Rows[row][Globals.COL_DET_RIPnOt].ToString() == "N")
                {
                    Reps = "(" + dtTable.Rows[row][Globals.COL_DET_RIPETIZIONIn].ToString() + ")";
                }
                else
                {
                    Reps = "(" + dtTable.Rows[row][Globals.COL_DET_RIPETIZIONIt].ToString() + ")";
                }
                if (Posizione < Globals.POSIZIONE_ATTUALE)
                {
                    int IDpast = int.Parse(dtTable.Rows[row][Globals.COL_ESE_ID].ToString());
                    CaricaBottone(dtTable.Rows[row][Globals.COL_ESE_NOME].ToString(), Globals.BTN_PAST, Posizione, Reps, Color.white, IDpast);
                }
                if (Posizione == Globals.POSIZIONE_ATTUALE + 1)
                {
                    Globals.BTN_NEXT.GetComponent<TextMeshProUGUI>().text = dtTable.Rows[row][Globals.COL_ESE_NOME].ToString();
                    Globals.BTN_NEXT.transform.Find("txtReps").GetComponent<TextMeshProUGUI>().text = Reps;
                    Globals.BTN_NEXT.GetComponent<Image>().color = Color.white;
                    int IDnext = int.Parse(dtTable.Rows[row][Globals.COL_ESE_ID].ToString());
                    Globals.BTN_NEXT.onClick.RemoveAllListeners();
                    Globals.BTN_NEXT.onClick.AddListener(() => ShowEsercizio(IDnext));

                    Ultimo = false;
                }
                if (Posizione > Globals.POSIZIONE_ATTUALE + 1)
                {
                    int IDthen = int.Parse(dtTable.Rows[row][Globals.COL_ESE_ID].ToString());
                    CaricaBottone(dtTable.Rows[row][Globals.COL_ESE_NOME].ToString(), Globals.BTN_THEN, Posizione, Reps, Color.white, IDthen);
                }
            }
        }
        Globals.BTN_PAST.gameObject.SetActive(false);
        Globals.BTN_THEN.gameObject.SetActive(false);

        if (Ultimo)
        {
            Globals.BTN_NEXT.GetComponent<TextMeshProUGUI>().text = "FINITO";
            Globals.BTN_NEXT.transform.Find("txtReps").GetComponent<TextMeshProUGUI>().text = "";
            Globals.BTN_NEXT.GetComponent<Image>().color = Color.blue;
            Globals.BTN_NEXT.GetComponent<TextMeshProUGUI>().color = Color.white;
            Globals.BTN_NEXT.onClick.RemoveAllListeners();
        }
        else
            Globals.BTN_NEXT.GetComponent<TextMeshProUGUI>().color = Color.black;
    }
    private void CaricaBottone(string Nome, Button button, int Posizione, string Reps, Color color, int ID)
    {
        GameObject Parent = button.transform.parent.gameObject;
        Button btnInstantiate = Instantiate(button);
        btnInstantiate.name = "Pos" + Posizione;
        btnInstantiate.transform.SetParent(Parent.transform);
        btnInstantiate.transform.SetSiblingIndex(Posizione + 1);
        btnInstantiate.GetComponent<TextMeshProUGUI>().text = Nome;
        btnInstantiate.transform.Find("txtReps").GetComponent<TextMeshProUGUI>().text = Reps;
        btnInstantiate.GetComponent<Image>().color = color;
        btnInstantiate.onClick.RemoveAllListeners();
        if (ID != default)
            btnInstantiate.onClick.AddListener(() => ShowEsercizio(ID));
    }
    private void Svuota(Button button)
    {
        GameObject Parent = button.transform.parent.gameObject;
        while (Parent.transform.childCount > 1)
        {
            DestroyImmediate(Parent.transform.GetChild(Parent.transform.childCount - 1).gameObject);
        }
    }
    public void ShowEsercizio(int IDes)
    {

        string sCondition = " WHERE " + Globals.COL_ESE_ID + " = " + IDes + " ";
        var tuple = AppManager.Instance.sql.SelectAllEsercizi(sCondition, Globals.COL_ESE_ID);
        DataTable dtTable = tuple.Item1;
        List<byte[]> ByteList = tuple.Item2;

        Globals.PP_ESERCIZIO.gameObject.SetActive(true);
        Globals.PP_ESERCIZIO.transform.Find("cnvPanel/txtName").GetComponent<TextMeshProUGUI>().text = dtTable.Rows[0][Globals.COL_ESE_NOME].ToString();
        Globals.PP_ESERCIZIO.transform.Find("cnvPanel/txtAdvice").GetComponent<TextMeshProUGUI>().text = "CONSIGLI:\r\n" + dtTable.Rows[0][Globals.COL_ESE_CONSIGLI].ToString();
        Globals.PP_ESERCIZIO.transform.Find("cnvPanel/txtNote").GetComponent<TextMeshProUGUI>().text = "NOTE:\r\n" + dtTable.Rows[0][Globals.COL_ESE_NOTE].ToString();

        DataTable Musc = AppManager.Instance.sql.SelectAllME(" WHERE me." + Globals.COL_MxE_IDESERCIZIO + " = " + IDes, "mu." + Globals.COL_MUS_NOME);
        string sMuscoli = "";
        for (int row = 0; row < Musc.Rows.Count; row++)
        {
            sMuscoli += Musc.Rows[row][Globals.COL_MUS_NOME].ToString() + " ";
        }
        Globals.PP_ESERCIZIO.transform.Find("cnvPanel/txtMuscles").GetComponent<TextMeshProUGUI>().text = sMuscoli;

        #region Image
        byte[] ByteImmagine = ByteList[0];
        if (ByteImmagine != null)
        {
            UploadImage uploadImage = gameObject.AddComponent<UploadImage>();
            Image immagine = uploadImage.ConvertByteArrayToImage(ByteImmagine);
            //Assegno la suddetta immagine al figlio (immagine) del figlio (ImgCircle = immagine circolare per creare la maschera rotonda) del bottone
            Globals.PP_ESERCIZIO.transform.Find("cnvPanel/Image").GetComponent<Image>().sprite = immagine.sprite;
            //Gli impogno di preservare il suo aspetto in modo da non ottere l'iimagine strecciata
            Globals.PP_ESERCIZIO.transform.Find("cnvPanel/Image").GetComponent<Image>().preserveAspect = true;

            Destroy(uploadImage);
        }
        #endregion
    }
}
