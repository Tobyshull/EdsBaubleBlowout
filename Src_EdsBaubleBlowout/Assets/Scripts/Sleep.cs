using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sleep : MonoBehaviour
{
    [SerializeField]
    private FadeController fadeController;

    public void InitialFadeComplete()
    {
        PlayerPrefs.SetInt("Energy", 100);
        PlayerPrefs.SetInt("Time of day", 420);
        fadeController.fading = false;
        fadeController.fadeFinishEvent.AddListener(() => SecondFadeFinished());
    }

    public void SecondFadeFinished()
    {
        SceneManager.LoadScene("House");
    }
}
