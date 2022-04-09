using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public GameObject closedDoor;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            closedDoor.SetActive(false);
            StartCoroutine(LoadScene());
        }
    }

    IEnumerator LoadScene()
    {
        FindObjectOfType<AudioManager>().Play("Door");
        yield return new WaitForSeconds(1.4f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}