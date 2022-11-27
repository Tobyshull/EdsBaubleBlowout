using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    public Transform levelSpawnPoint;
    public GameObject[] enemies;
    public float enemyReleaseRate;

    private void Start()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].SetActive(false);
        }
    }


    private void Update()
    {
        bool allNull = true;
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                allNull = false;
                break;
            }
        }

        if (allNull)
        {
            PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
            SceneManager.LoadScene("House");
        }
    }

    public void StartLevel()
    {
        StartCoroutine(ReleaseTheBeasts());
    }

    IEnumerator ReleaseTheBeasts()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            yield return new WaitForSeconds(enemyReleaseRate);
            enemies[i].SetActive(true);
        }
    }
}
