using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupWeb : MonoBehaviour
{
    public static bool ANSWERED;
    public static string CLICKED;

    [SerializeField] Canvas cnvPopup;
    [SerializeField] Button btnLeft;
    [SerializeField] Button btnCenter;
    [SerializeField] Button btnRight;
    [SerializeField] TextMeshProUGUI txtMessaggio;
    //public string Mostra(string sMex, string sbtnSx, string sbtnDx, string sbtnCn)
    //{
    //    txtMessaggio.text = sMex;
    //    if (sbtnSx == null)
    //        btnLeft.gameObject.SetActive(false);
    //    else
    //    {
    //        btnLeft.gameObject.SetActive(true);
    //        btnLeft.GetComponent<TextMeshProUGUI>().text = sbtnSx;
    //    }
    //    if (sbtnCn == null)
    //        btnCenter.gameObject.SetActive(false);
    //    else
    //    {
    //        btnCenter.gameObject.SetActive(true);
    //        btnCenter.GetComponent<TextMeshProUGUI>().text = sbtnCn;
    //    }
    //    if (sbtnDx == null)
    //        btnRight.gameObject.SetActive(false);
    //    else
    //    {
    //        btnRight.gameObject.SetActive(true);
    //        btnRight.GetComponent<TextMeshProUGUI>().text = sbtnDx;
    //    }
    //}
}
