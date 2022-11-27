using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void OpenMenu()
    {
        SceneManager.LoadScene("House");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
