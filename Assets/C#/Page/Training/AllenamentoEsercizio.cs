using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AllenamentoEsercizio : MonoBehaviour
{
    public void Carica(int IDdet)
    {
        DataTable dtTable;

        string sCondition = " WHERE det." + Globals.COL_DET_ID + " = " + IDdet + " ";
        var tuple = AppManager.Instance.sql.SelectAllDettagli(sCondition, "det." + Globals.COL_DET_POSIZIONE);
        dtTable = tuple.Item1;
        List<byte[]> arrayList = tuple.Item2;

        Globals.PAGE_ALLENAMENTO.gameObject.transform.Find("Esercizio").GetComponent<Canvas>().gameObject.SetActive(true);
        Globals.PAGE_ALLENAMENTO.gameObject.transform.Find("Recupero").GetComponent<Canvas>().gameObject.SetActive(false);

        if (dtTable.Rows[0][Globals.COL_DET_RIPnOt].ToString() == "N")
            RepsN(dtTable.Rows[0][Globals.COL_ESE_NOME].ToString(), dtTable.Rows[0][Globals.COL_DET_RIPETIZIONIn].ToString(), arrayList);
        else
            RepsT(dtTable.Rows[0][Globals.COL_ESE_NOME].ToString(), dtTable.Rows[0][Globals.COL_DET_RIPETIZIONIt].ToString(), arrayList);
        DataTable dtWeight = AppManager.Instance.sql.SelectAllAttivita(" WHERE de." + Globals.COL_DET_ID + " = " + IDdet, "ll." + Globals.COL_ALL_DATA + " DESC, " + "tt." + Globals.COL_ATT_SERIE + " DESC");
        if (dtWeight.Rows.Count > 0)
            Weight(double.Parse(dtWeight.Rows[0][Globals.COL_ATT_PESO].ToString()));
        else
            Weight(0);
    }
    private void RepsN(string Nome, string Reps, List<byte[]> arrayList)
    {
        Globals.CNV_ESE_REPS.gameObject.SetActive(true);
        Globals.CNV_BTN_REPS.gameObject.SetActive(true);

        Globals.CNV_ESE_TIMER.gameObject.SetActive(false);
        Globals.CNV_BTN_TIMER.gameObject.SetActive(false);

        Globals.CNV_ESE_REPS.gameObject.transform.Find("goName").GetComponent<Transform>().GetComponent<CurvedText>().text = Nome;
        Globals.CNV_ESE_REPS.gameObject.transform.Find("txtReps").GetComponent<TextMeshProUGUI>().text = Reps;
        #region Immagine
        byte[] ByteImmagine = arrayList[0];
        if (ByteImmagine != null)
        {
            UploadImage uploadImage = gameObject.AddComponent<UploadImage>();
            if (!gameObject.GetComponent<Image>())
                gameObject.AddComponent<Image>();
            Image immagine = gameObject.GetComponent<Image>();
            immagine = uploadImage.ConvertByteArrayToImage(ByteImmagine);
            Image PostoImmagine = Globals.CNV_ESE_REPS.gameObject.transform.Find("RadialProgressBar").GetComponent<Image>().gameObject.transform.Find("Fill").GetComponent<Image>().gameObject.transform.Find("Mask").GetComponent<Image>().gameObject.transform.Find("Image").GetComponent<Image>();
            PostoImmagine.sprite = immagine.sprite;
            PostoImmagine.preserveAspect = true;
            PostoImmagine.SetNativeSize();

            RectTransform rt = PostoImmagine.GetComponent<RectTransform>();
            double width = rt.sizeDelta.x * rt.localScale.x;
            double height = rt.sizeDelta.y * rt.localScale.y;

            Image Mask = Globals.CNV_ESE_REPS.gameObject.transform.Find("RadialProgressBar").GetComponent<Image>().gameObject.transform.Find("Fill").GetComponent<Image>().gameObject.transform.Find("Mask").GetComponent<Image>();
            RectTransform rtMask = Mask.GetComponent<RectTransform>();
            float widthMask = rtMask.sizeDelta.x * rtMask.localScale.x;
            float heightMask = rtMask.sizeDelta.y * rtMask.localScale.y;

            if (width > height)
            {
                PostoImmagine.rectTransform.sizeDelta = new Vector3(4000, heightMask, 1);
            }
            else
            {
                PostoImmagine.rectTransform.sizeDelta = new Vector3(widthMask, 4000, 1);
            }
            PostoImmagine.rectTransform.anchoredPosition = new Vector3(0, 0, 1);
            Destroy(immagine);
        }
        #endregion
    }
    private void RepsT(string Nome, string Time, List<byte[]> arrayList)
    {
        Globals.CNV_ESE_TIMER.gameObject.SetActive(true);
        Globals.CNV_BTN_TIMER.gameObject.SetActive(true);

        Globals.CNV_ESE_REPS.gameObject.SetActive(false);
        Globals.CNV_BTN_REPS.gameObject.SetActive(false);

        Globals.CNV_ESE_TIMER.gameObject.transform.Find("goName").GetComponent<Transform>().GetComponent<CurvedText>().text = Nome;
        if (!gameObject.GetComponent<AllenamentoRecupero>())
            gameObject.AddComponent<AllenamentoRecupero>();
        AllenamentoRecupero recupero = gameObject.GetComponent<AllenamentoRecupero>();
        Globals.ALLENAMENTO_MODE = "Esercizio";
        int Secondi = int.Parse(Time.Substring(0, 2)) * 60 + int.Parse(Time.Substring(3, 2));
        recupero.Carica(Secondi);
        #region Immagine
        byte[] ByteImmagine = arrayList[0];
        if (ByteImmagine != null)
        {
            UploadImage uploadImage = gameObject.AddComponent<UploadImage>();
            if (!gameObject.GetComponent<Image>())
                gameObject.AddComponent<Image>();
            Image immagine = gameObject.GetComponent<Image>();
            immagine = uploadImage.ConvertByteArrayToImage(ByteImmagine);
            Image PostoImmagine = Globals.CNV_ESE_TIMER.gameObject.transform.Find("RadialProgressBar").GetComponent<Image>().gameObject.transform.Find("Fill").GetComponent<Image>().gameObject.transform.Find("Mask").GetComponent<Image>().gameObject.transform.Find("Image").GetComponent<Image>();
            PostoImmagine.sprite = immagine.sprite;
            PostoImmagine.preserveAspect = true;
            PostoImmagine.SetNativeSize();

            RectTransform rt = PostoImmagine.GetComponent<RectTransform>();
            double width = rt.sizeDelta.x * rt.localScale.x;
            double height = rt.sizeDelta.y * rt.localScale.y;

            Image Mask = Globals.CNV_ESE_TIMER.gameObject.transform.Find("RadialProgressBar").GetComponent<Image>().gameObject.transform.Find("Fill").GetComponent<Image>().gameObject.transform.Find("Mask").GetComponent<Image>();
            RectTransform rtMask = Mask.GetComponent<RectTransform>();
            float widthMask = rtMask.sizeDelta.x * rtMask.localScale.x;
            float heightMask = rtMask.sizeDelta.y * rtMask.localScale.y;

            if (width > height)
            {
                PostoImmagine.rectTransform.sizeDelta = new Vector3(4000, heightMask, 1);
            }
            else
            {
                PostoImmagine.rectTransform.sizeDelta = new Vector3(widthMask, 4000, 1);
            }
            PostoImmagine.rectTransform.anchoredPosition = new Vector3(0, 0, 1);
            Destroy(immagine);
        }
        #endregion
    }
    private void Weight(double Weight)
    {
        Globals.CNV_WEIGHT.gameObject.transform.Find("InputFieldWeight").GetComponent<TMP_InputField>().text = Weight.ToString();
    }
    public void Finito(bool Fatto)
    {
        if (Fatto == false)
            Globals.ES_FATTO = "NO";

        AppManager.Instance.allenamento.Next();
    }
}
