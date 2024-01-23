using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class HirePeasantTimer : MonoBehaviour
{
    bool isStarted = false;
    public GameManager gameManager;
    [SerializeField]
    Button hiringPeasantButton;
    int timerTime ;
    float currentTimerTime;
    Image timerImage;
    [SerializeField]
    AudioSource clickSound;
    
    void Start()
    {
        timerImage = GetComponent<Image>();        
        hiringPeasantButton.interactable = true;
        timerImage.fillAmount = 1;
    }


    void Update()
    {
        if (isStarted)
        {
            timerImage.fillAmount = currentTimerTime / timerTime;
            hiringPeasantButton.interactable = false;
            currentTimerTime -= Time.deltaTime; ;

            if (currentTimerTime <= 0)
            {
                gameManager.IncreasePeasantsAmount();
                isStarted = false;
                currentTimerTime = timerTime;
                hiringPeasantButton.interactable = true;
                timerImage.fillAmount = 1;
            }
        }
    }
    public void SetTimerTime(int time)
    {
        timerTime = time;
    }
    public void StartTimer()
    {
        clickSound.Play();
        currentTimerTime = timerTime;
        isStarted = true;
    }
}
