using UnityEngine;

public class Lever : MonoBehaviour
{
    Animator Animator;

    bool isToggled = true;
    bool canInteract;
    public GameObject doorObject;
    public GameObject tutPopup;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (canInteract && Input.GetKeyDown(KeyCode.E))
        {
            isToggled = !isToggled;
            doorObject.SetActive(isToggled);

            if (tutPopup != null)
            {
                tutPopup.SetActive(isToggled);
            }

            Animator.SetBool("triggered", isToggled);
        }
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