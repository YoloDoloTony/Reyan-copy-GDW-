using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextPopup : MonoBehaviour
{
    public GameObject popupBox;
    public Animator animator;
    public TMP_Text popupText;

    public void Popup(string text)
    {
        popupText.text = text;
        animator.SetTrigger("pop");
    }

    public void ClosePopup()
    {
        animator.SetTrigger("close");
    }
}