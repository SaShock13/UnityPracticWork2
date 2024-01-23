using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HireWarriorTimer : MonoBehaviour
{
    bool isStarted = false;
    public GameManager gameManager;
    [SerializeField]
    Button hiringWarriorButton;
    int timerTime;
    float currentTimerTime;
    Image timerImage;
    [SerializeField]
    AudioSource clickSound;

    void Start()
    {        
        timerImage = GetComponent<Image>();        
        hiringWarriorButton.interactable = true;
        timerImage.fillAmount = 1;

    }

    void Update()
    {

        if (isStarted)
        {
            timerImage.fillAmount = currentTimerTime / timerTime;
            hiringWarriorButton.interactable = false;
            currentTimerTime -= Time.deltaTime;

            if (currentTimerTime <= 0)
            {
                gameManager.IncreaseWarriorAmount();
                isStarted = false;
                currentTimerTime = timerTime;
                timerImage.fillAmount = 1;
                hiringWarriorButton.interactable = true;
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
