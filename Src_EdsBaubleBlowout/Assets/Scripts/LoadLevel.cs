using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    public Level[] levels;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Level level = levels[PlayerPrefs.GetInt("Level")];
            collision.transform.position = level.levelSpawnPoint.position;
            level.StartLevel();
        }
    }
}
