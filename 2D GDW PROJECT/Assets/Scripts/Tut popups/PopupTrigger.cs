using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupTrigger : MonoBehaviour
{
    public string popUp;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TextPopup pop = GameObject.Find("PopUpManager").GetComponent<TextPopup>();
            pop.Popup(popUp);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TextPopup pop = GameObject.Find("PopUpManager").GetComponent<TextPopup>();
            pop.ClosePopup();
        }
    }
}