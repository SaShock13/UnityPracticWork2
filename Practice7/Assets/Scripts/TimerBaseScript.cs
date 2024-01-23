using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerBaseScript : MonoBehaviour
{
    
    int timerTime;
    float currentTimerTime;
    Image timerImage;
    public bool tick=false;
    void Start()
    {
        timerTime = 20;
        timerImage = GetComponent<Image>();
        currentTimerTime = timerTime;
    }

    void Update()
    {
        
        tick = false;
        currentTimerTime -= Time.deltaTime; ;
        timerImage.fillAmount = currentTimerTime / timerTime;
        if (currentTimerTime <= 0) 
        { 
            tick = true;
            currentTimerTime = timerTime;             
        }
    }
    public void SetTimerTime(int time)
    {
        currentTimerTime = time;
        timerTime = time;
    }
}
