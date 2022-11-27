using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    private void Start()
    {
        if(PlayerPrefs.GetInt("First Open") == 0)
        {
            PlayerPrefs.SetInt("First Open", 1);
            PlayerPrefs.SetInt("Health", 10);
            PlayerPrefs.SetInt("Time of day", 420);
            PlayerPrefs.SetInt("Pigs", 0);
            PlayerPrefs.SetInt("Energy", 100);
        }
    }

    public void OpenMenu()
    {
        SceneManager.LoadScene("House");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
