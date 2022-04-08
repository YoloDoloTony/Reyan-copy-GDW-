using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    bool isToggled = true;
    bool canInteract;
    public GameObject doorObject;
    Animator Animator;

    SpriteRenderer SpriteRenderer;

    private void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canInteract && Input.GetKeyDown(KeyCode.E))
        {
            isToggled = !isToggled;
            doorObject.SetActive(isToggled);
            Animator.SetBool("flipped", isToggled);
        }

        /*if (isToggled)
        {
            SpriteRenderer.color = Color.green;
        }
        else
        {
            SpriteRenderer.color = Color.red;
            //will change when we have anims
        }
        if (canInteract)
        {
            SpriteRenderer.color = Color.yellow;
        }*/
    }

    private void OnTriggerEnter2D(Collider2D leverBox)
    {
        if (leverBox.CompareTag("Player"))
        {
            canInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D leverBox)
    {
        if (leverBox.CompareTag("Player"))
        {
            canInteract = false;
        }
    }
}