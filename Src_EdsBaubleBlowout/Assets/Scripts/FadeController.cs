using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    public UnityEvent fadeFinishEvent;

    public bool fading = false;

    [SerializeField]
    private Image panel;
    [SerializeField]
    private float fadeSpeed;

    private bool fadedTasksRun = false;

    void Update()
    {
        if (fading && panel.color.a > 0)
        {
            fadedTasksRun = true;
            panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, panel.color.a - (fadeSpeed * Time.deltaTime));
        }
        if (fading && panel.color.a <= 0 && fadedTasksRun)
        {
            fadedTasksRun = false;
            fadeFinishEvent.Invoke();
            fadeFinishEvent.RemoveAllListeners();
        }

        if(!fading && panel.color.a < 1)
        {
            fadedTasksRun = true;
            panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, panel.color.a + (fadeSpeed * Time.deltaTime));
        }
        if (!fading && panel.color.a >= 1 && fadedTasksRun)
        {
            fadedTasksRun = false;
            fadeFinishEvent.Invoke();
            fadeFinishEvent.RemoveAllListeners();
        }
    }
}
