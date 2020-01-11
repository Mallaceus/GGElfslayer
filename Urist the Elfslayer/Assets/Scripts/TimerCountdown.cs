using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerCountdown : MonoBehaviour
{

    public float timeLeft = 60.0f;


    void Update()
    {
        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0.0f)
        {
            TimerStop();
        }
    }

    void TimerStop()
    {
        SceneManager.LoadSceneAsync("l3");
    }
}
