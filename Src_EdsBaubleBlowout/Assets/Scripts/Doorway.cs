using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Doorway : MonoBehaviour
{
    public GameObject playerObject;

    public string newSceneName;

    public void FinishedFade()
    {
        SceneManager.LoadScene(newSceneName);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == playerObject)
        {
            FadeController fadeController = playerObject.GetComponent<FadeController>();
            fadeController.fading = false;
            fadeController.fadeFinishEvent.AddListener(() => FinishedFade());

            playerObject.GetComponent<PlayerMovement>().enabled = false;
        }
    }
}
