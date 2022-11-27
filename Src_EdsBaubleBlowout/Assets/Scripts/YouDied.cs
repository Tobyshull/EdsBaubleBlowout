using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class YouDied : MonoBehaviour
{
    [SerializeField]
    TMP_Text deathMessage;

    private void Start()
    {
        PlayerPrefs.SetInt("First Open", 0);

        deathMessage.text = PlayerPrefs.GetString("Death Message");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
