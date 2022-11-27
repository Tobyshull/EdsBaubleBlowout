using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    public int pigs = 0;
    [SerializeField]
    private TMP_Text pigsText;

    [SerializeField]
    public int health = 10;
    [SerializeField]
    private TMP_Text healthText;
    [SerializeField]
    private GameObject healingText;

    [SerializeField]
    public int timeOfDay = 420;
    [SerializeField]
    private TMP_Text timeOfDayText;
    [SerializeField]
    RectTransform clock;

    // 1440 minutes in a day
    // 420 = 7am
    // 1 second = 2 minutes -> 720 seconds / day === 12 min / day
    private int minutePerSecond = 2;

    public bool pauseTimeOfDay = false;

    void Start()
    {
        StartCoroutine(TimeOfDayTracker());

        healingText.SetActive(false);
    }

    IEnumerator TimeOfDayTracker()
    {
        while (true)
        {
            if (!pauseTimeOfDay)
            {
                yield return new WaitForSeconds(1.0f / minutePerSecond);
                timeOfDay += 1;

                if(timeOfDay > 1440)
                {
                    timeOfDay = 0;
                    Debug.Log("Midnight hit, handle here!");
                }

                clock.eulerAngles = new Vector3(0, 0, 360 * (timeOfDay / 1440.0f) - 150);
            }
        }
    }

    void Update()
    {
        pigsText.text = pigs.ToString();

        healthText.text = health.ToString();

        int hours = timeOfDay / 60;
        int minutes = timeOfDay % 60;
        timeOfDayText.text = (hours < 10 ? "0" + hours.ToString() : hours.ToString()) + ":" + (minutes < 10 ? "0" + minutes.ToString() : minutes.ToString());
    }
}
