using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PageSwiper : MonoBehaviour, IDragHandler, IEndDragHandler
{
    //posizione del pannello iniziale (assegnato appena viene chiamata la classe)
    //costante private Vector3 Globals.panelLocation;
    //percentuale schermo da scorrere per swipare la pagina
    public float percentThreshold = 0.2f;
    //velocità scorrimento
    public float easing = 0.5f;

    void Start()
    {
        Globals.panelLocation = transform.position;
    }

    public void OnDrag(PointerEventData data)
    {
        float difference = data.pressPosition.x - data.position.x;
        transform.position = Globals.panelLocation - new Vector3(difference, 0, 0);
    }
    public void OnEndDrag(PointerEventData data)
    {
        float percentage = (data.pressPosition.x - data.position.x) / Screen.width;
        if (Mathf.Abs(percentage) >= percentThreshold)
        {
            Vector3 newLocation = Globals.panelLocation;
            //se la percentuale è > 0 allora la panel location diventa quella di quello attuale + la larghezza dello schermo, altrimenti meno
            if (percentage > 0)
            {
                newLocation += new Vector3(-Screen.width, 0, 0);
            }
            else if (percentage < 0)
            {
                newLocation += new Vector3(Screen.width, 0, 0);
            }

            //Controllo che non esca dallo schermo 
            

            if (newLocation.x < Globals.X_PAGE_MENU_EDIT)
                newLocation.x = Globals.X_PAGE_MENU_EDIT;
            else if (newLocation.x > Globals.X_PAGE_MENU_STATS )
                newLocation.x = Globals.X_PAGE_MENU_STATS ;

            StartCoroutine(SmoothMove(transform.position, newLocation, easing));
            Globals.panelLocation = newLocation;
            if (Globals.panelLocation.x == Globals.X_PAGE_MENU_STATS )
                Tabs(0);
            if (Globals.panelLocation.x == Globals.X_PAGE_MENU_TRAINING)
                Tabs(1);
            if (Globals.panelLocation.x == Globals.X_PAGE_MENU_EDIT)
                Tabs(2);

        }
        else
        {
            StartCoroutine(SmoothMove(transform.position, Globals.panelLocation, easing));
        }
    }
    IEnumerator SmoothMove(Vector3 startpos, Vector3 endpos, float seconds)
    {
        float t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            transform.position = Vector3.Lerp(startpos, endpos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
    }
    void Tabs(int iTab)
    {
        Globals.TAB_STATS.GetComponent<Image>().color = Color.white;
        Globals.TAB_TRAINING.GetComponent<Image>().color = Color.white;
        Globals.TAB_EDIT.GetComponent<Image>().color = Color.white;
        switch (iTab)
        {
            case 0:
                Globals.TAB_STATS.GetComponent<Image>().color = Color.red;
                break;
            case 1:
                Globals.TAB_TRAINING.GetComponent<Image>().color = Color.red;
                break;
            case 2:
                Globals.TAB_EDIT.GetComponent<Image>().color = Color.red;
                break;
            default:
                Globals.TAB_TRAINING.GetComponent<Image>().color = Color.red;
                break;
        }
    }
}
