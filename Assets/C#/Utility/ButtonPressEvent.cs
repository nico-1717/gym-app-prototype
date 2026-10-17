using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonPressEvent : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // Evento che viene scatenato quando il pulsante viene premuto
    //al posto di unityevent, in ambiente .NET si può utilizzare evnt action che ha le stesse funzionalità
    public UnityEvent onPressed = new UnityEvent();

    // Evento che viene scatenato quando il pulsante viene rilasciato
    public UnityEvent onReleased = new UnityEvent();

    float TimePress = 1.5f;

    public bool buttonPressed;

    private Coroutine checkHoldCoroutine;

    void Start()
    {
        buttonPressed = false;
    }

    // Metodo chiamato quando il pulsante viene premuto
    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
        if (checkHoldCoroutine == null)
        {
            checkHoldCoroutine = StartCoroutine(CheckHoldCoroutine());
        }
    }

    // Metodo chiamato quando il pulsante viene rilasciato
    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;
        if (checkHoldCoroutine != null)
        {
            StopCoroutine(checkHoldCoroutine);
            checkHoldCoroutine = null;
            onReleased.Invoke();
        }
        //per evitare che appena rilasci il button si avvi onClick
        StartCoroutine(ReactivateButton());
    }

    private IEnumerator ReactivateButton()
    {
        yield return new WaitForSeconds(0.01f);
        this.GetComponent<Button>().interactable = true;
    }

    private IEnumerator CheckHoldCoroutine()
    {
        yield return new WaitForSeconds(TimePress);
        if (buttonPressed)
        {
            this.GetComponent<Button>().interactable = false;
            onPressed.Invoke();
        }
    }
}
