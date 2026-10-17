using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Threading.Tasks;
using TMPro;

public class MessageBox : MonoBehaviour
{
    static private TextMeshProUGUI txtMessage;
    static private Button btnLeft;
    static private Button btnCenter;
    static private Button btnRight;
    static private Canvas cnvPopup;
    static private Canvas cnvText;
    static private Canvas cnvButton;
    public Toggle tglCheck;

    public TaskCompletionSource<int> ButtonClicked { get; set; } = null;

    private static MessageBox messageBox;

    private MessageBox() { }

    public static MessageBox Instance()
    {
        if (!messageBox)
        {
            messageBox = FindObjectOfType(typeof(MessageBox)) as MessageBox;
            if (!messageBox)
                Debug.LogError("There needs to be one active messageBox script on a GameObject in your scene.");
        }

        return messageBox;
    }
    public IEnumerator ShowMsg(int PopupNumber, string sMessage, int iSeconds)
    {
        SetPopup(PopupNumber);

        GameObject goMsg = cnvPopup.gameObject;

        AppManager.Instance.objects.ShowPopup(cnvPopup.gameObject);

        txtMessage.text = sMessage;

        yield return new WaitForSeconds(iSeconds);

        ClosePanel(goMsg);
    }
    public async Task<int> ShowMsgBox(int PopupNumber, string squestion, string sBtnLeft = null, string sBtnCenter = null, string sBtnRight = null, bool bCheck = false)
    {
        SetPopup(PopupNumber);

        AppManager.Instance.objects.ShowPopup(cnvPopup.gameObject);

        txtMessage.text = squestion;

        tglCheck.gameObject.SetActive(bCheck);

        if (sBtnLeft != null)
        {
            btnLeft.transform.Find("txtBtn").GetComponent<TextMeshProUGUI>().text = sBtnLeft;
            btnLeft.gameObject.SetActive(true);
            btnLeft.onClick.RemoveAllListeners();
            btnLeft.onClick.AddListener(() => ClosePanel());
            btnLeft.onClick.AddListener(() => { ButtonClicked?.TrySetResult(1); });
        }
        else
            btnLeft.gameObject.SetActive(false);

        if (sBtnCenter != null)
        {
            btnCenter.transform.Find("txtBtn").GetComponent<TextMeshProUGUI>().text = sBtnCenter;
            btnCenter.gameObject.SetActive(true);
            btnCenter.onClick.RemoveAllListeners();
            btnCenter.onClick.AddListener(() => ClosePanel());
            btnCenter.onClick.AddListener(() => { ButtonClicked?.TrySetResult(2); });
        }
        else
            btnCenter.gameObject.SetActive(false);

        if (sBtnRight != null)
        {
            btnRight.transform.Find("txtBtn").GetComponent<TextMeshProUGUI>().text = sBtnRight;
            btnRight.gameObject.SetActive(true);
            btnRight.onClick.RemoveAllListeners();
            btnRight.onClick.AddListener(() => ClosePanel());
            btnRight.onClick.AddListener(() => { ButtonClicked?.TrySetResult(3); });
        }
        else
            btnRight.gameObject.SetActive(false);

        RidimensionaMsgBox(PopupNumber);

        return await ButtonClickedTask();
    }
    void SetPopup(int PopupNumber)
    {
        switch (PopupNumber)
        {
            case 1:
                cnvPopup = Globals.CANVAS_POPUP.transform.Find("ppMessageBox1").GetComponent<Canvas>();
                cnvText = cnvPopup.transform.Find("cnvPanel/cnvText").GetComponent<Canvas>();
                cnvButton = cnvPopup.transform.Find("cnvPanel/cnvButton").GetComponent<Canvas>();
                txtMessage = cnvText.transform.Find("txtQuestion").GetComponent<TextMeshProUGUI>();
                btnLeft = cnvButton.transform.Find("btnLeft").GetComponent<Button>();
                btnCenter = cnvButton.transform.Find("btnCenter").GetComponent<Button>();
                btnRight = cnvButton.transform.Find("btnRight").GetComponent<Button>();
                tglCheck = cnvButton.transform.Find("Toggle").GetComponent<Toggle>();
                tglCheck.isOn = false;
                break;
            case 2:
                cnvPopup = Globals.CANVAS_POPUP.transform.Find("ppMessage2").GetComponent<Canvas>();
                txtMessage = cnvPopup.transform.Find("txtMessage").GetComponent<TextMeshProUGUI>();
                break;
        }
    }
    void ClosePanel(GameObject goPopup = null)
    {
        AppManager.Instance.objects.ClosePopup(goPopup);
    }

    private Task<int> ButtonClickedTask()
    {
        ButtonClicked = new TaskCompletionSource<int>();
        return ButtonClicked.Task;
    }
    private void RidimensionaMsgBox(int PopupNumber)
    {
        switch (PopupNumber)
        {
            case 1:
                float preferredHeight;
                float HeightMax = 0;
                float cnvButtonHeight = 0;
                float cnvTextHeight = 0;
                TextMeshProUGUI txt;
                GameObject goContenitore;

                txt = txtMessage;
                goContenitore = cnvText.gameObject;
                //Imposto i margini del testo
                txt.GetComponent<TextMeshProUGUI>().margin = new Vector4(10, 10, 10, 10);
                // Calcola l'altezza del testo in base alla lunghezza del messaggio
                preferredHeight = txt.preferredHeight;
                cnvTextHeight = preferredHeight;
                // Imposta l'altezza del MessageBox in base all'altezza del testo
                goContenitore.GetComponent<RectTransform>().sizeDelta = new Vector2(txt.GetComponent<RectTransform>().sizeDelta.x, cnvTextHeight);

                goContenitore = btnLeft.gameObject;
                if (goContenitore.activeSelf)
                {
                    txt = goContenitore.transform.Find("txtBtn").GetComponent<TextMeshProUGUI>();
                    goContenitore = cnvText.gameObject;
                    txt.GetComponent<TextMeshProUGUI>().margin = new Vector4(10, 10, 10, 10);
                    preferredHeight = txt.preferredHeight;
                    if (preferredHeight > HeightMax)
                        HeightMax = preferredHeight;
                }
                goContenitore = btnRight.gameObject;
                if (goContenitore.activeSelf)
                {
                    txt = goContenitore.transform.Find("txtBtn").GetComponent<TextMeshProUGUI>();
                    goContenitore = cnvText.gameObject;
                    txt.GetComponent<TextMeshProUGUI>().margin = new Vector4(10, 10, 10, 10);
                    preferredHeight = txt.preferredHeight;
                    if (preferredHeight > HeightMax)
                        HeightMax = preferredHeight;
                }
                goContenitore = btnCenter.gameObject;
                if (goContenitore.activeSelf)
                {
                    txt = goContenitore.transform.Find("txtBtn").GetComponent<TextMeshProUGUI>();
                    goContenitore = cnvText.gameObject;
                    txt.GetComponent<TextMeshProUGUI>().margin = new Vector4(10, 10, 10, 10);
                    preferredHeight = txt.preferredHeight;
                    if (preferredHeight > HeightMax)
                        HeightMax = preferredHeight;
                }

                if (btnLeft.gameObject.activeSelf && btnRight.gameObject.activeSelf)
                {
                    btnLeft.GetComponent<RectTransform>().sizeDelta = new Vector2(btnLeft.transform.GetComponent<RectTransform>().sizeDelta.x, HeightMax);
                    btnRight.GetComponent<RectTransform>().sizeDelta = new Vector2(btnRight.transform.GetComponent<RectTransform>().sizeDelta.x, HeightMax);
                    cnvButtonHeight += HeightMax + 25;
                }
                if (btnCenter.gameObject.activeSelf)
                {
                    btnCenter.GetComponent<RectTransform>().sizeDelta = new Vector2(btnCenter.transform.Find("txtBtn").GetComponent<RectTransform>().sizeDelta.x, HeightMax);
                    cnvButtonHeight += HeightMax + 25;
                }
                if (tglCheck.gameObject.activeSelf)
                {
                    cnvButtonHeight += tglCheck.GetComponent<RectTransform>().rect.height + 25;
                }

                cnvButton.GetComponent<RectTransform>().sizeDelta = new Vector2(cnvButton.GetComponent<RectTransform>().sizeDelta.x, cnvButtonHeight);

                GameObject cnvPanel = cnvPopup.transform.Find("cnvPanel").gameObject;
                cnvPanel.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, cnvTextHeight + cnvButtonHeight);

                float yPosition = cnvButton.GetComponent<RectTransform>().position.y + cnvButton.GetComponent<RectTransform>().rect.height;
                float xPositionLeft = cnvButton.GetComponent<RectTransform>().position.x - cnvButton.GetComponent<RectTransform>().rect.width / 2;
                float xPositionRight = cnvButton.GetComponent<RectTransform>().position.x + cnvButton.GetComponent<RectTransform>().rect.width / 2;
                btnLeft.GetComponent<RectTransform>().position = new Vector2(xPositionLeft + 25, yPosition - 25);
                btnRight.GetComponent<RectTransform>().position = new Vector2(xPositionRight - 25, yPosition - 25);
                btnCenter.GetComponent<RectTransform>().position = new Vector2(cnvButton.GetComponent<RectTransform>().position.x, yPosition - 25);
                tglCheck.GetComponent<RectTransform>().position = new Vector2(cnvButton.GetComponent<RectTransform>().position.x, yPosition - 25);

                if (btnLeft.gameObject.activeSelf && btnRight.gameObject.activeSelf)
                    btnCenter.GetComponent<RectTransform>().position = new Vector2(btnCenter.GetComponent<RectTransform>().position.x, -25 - HeightMax + btnCenter.GetComponent<RectTransform>().position.y);

                if (btnLeft.gameObject.activeSelf || btnCenter.gameObject.activeSelf)
                    tglCheck.GetComponent<RectTransform>().position = new Vector2(tglCheck.GetComponent<RectTransform>().position.x, -25 - HeightMax + tglCheck.GetComponent<RectTransform>().position.y);

                break;
            default:
                break;
        }
    }
}