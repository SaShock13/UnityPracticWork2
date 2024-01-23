using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Todo 
/// Задание
//Усовершенствуйте игру, начатую на модуле, добавьте следующий функционал:
//
//наем воина и крестьянгина завязать на днях
// 
//Подберите параметры таким образом, чтобы можно было сыграть как минимум 10 циклов нападений на деревню.
//Подрезать аудио файлы!.
//Сделайте кнопку отключения звука.
//При нажатии на кнопки реализуйте короткий звук клика.
//При сборе урожая, нападении врагов, спавне крестьян и воинов, поедании зерна должны воспроизводиться соответствующие звуки, что-то вроде звуков в классической Age of Empires II.
//Бесплатные звуки и музыку можно найти здесь:
//freesound.org
//reddit.com
//reddit.com
//reddit.com
//reddit.com
//habr.com
//habr.com
//Сделайте симпатичный интерфейс: с плашками, своими шрифтами и так далее.
/// </summary>


public class GameManager : MonoBehaviour
{

    #region ПОЛЯ
    System.Random rnd = new System.Random();
    int warriorDeaths=0;
    int nextRaidWarriorsAmount = 0;
    int daysCounter = 1;
    /// <summary>
    /// Максимальное значение таймера рейда
    /// </summary>
    int raidTimerTime;
    /// <summary>
    /// Текущее значение таймера Рейда
    /// </summary>
    float currentRaidTimerTime;
    /// <summary>
    /// Счетчик дней до рейда
    /// </summary>
    int raidCounterInDays;
    int allRaidsCount = 0;
    bool isWin = false;

    public bool isPaused = false;

    [SerializeField] 
    int oneDayInSec =7 ;
    [SerializeField] 
    int HarvestInDays = 2;
    [SerializeField]
    int peasantCount =1;
    [SerializeField]
    int wheatCount = 10;
    [SerializeField]
    int warriorCount = 10;
    [SerializeField]
    int peasantMakingWheat = 5;
    [SerializeField]
    int warriorEatWheat = 3;
    [SerializeField]
    int warriorPrice = 5;
    int winTarget = 1500;
    /// <summary>
    /// Время между рейдами в днях
    /// </summary>
    [SerializeField] 
    int raidTimerTimeInDays = 5;
    [SerializeField]
    int peasantHiringTimeInDays;
    [SerializeField]
    int warriorHiringTimeInDays;


    [SerializeField] 
    TMP_Text peasantCounterText;
    [SerializeField]
    TMP_Text warriorCounterText;
    [SerializeField]
    TMP_Text wheatCounterText;
    [SerializeField]
    TMP_Text daysCounterText; 
    [SerializeField]
    TMP_Text timeToRaidText;
    [SerializeField]
    TMP_Text raidInfoText;
    [SerializeField]
    TMP_Text winInfoText;
    [SerializeField]
    TMP_Text nextRaidWarriorsAmountText;
    [SerializeField]
    TMP_Text gameOverText;
    [SerializeField]
    TMP_Text gameOverInfoText;

    [SerializeField]
    TimerBaseScript daysTimer;
    [SerializeField]
    TimerBaseScript harvestTimer;
    [SerializeField]
    PauseSCript pause;
    [SerializeField]
    HirePeasantTimer peasantTimer;
    [SerializeField]
    HireWarriorTimer warriorTimer;

    [SerializeField]
    Image raidTimerImage;

    [SerializeField]
    Button hirePeasantButton;
    [SerializeField]
    Button hireWarriorButton;

    [SerializeField]
    Canvas gameCanvas;
    [SerializeField]
    Canvas gameOverCanvas;
    [SerializeField]
    Canvas pauseCanvas;
    [SerializeField]
    Canvas winCanvas;
    [SerializeField]
    Canvas raidInfoCanvas;

    
    public AudioSource backSound;
    [SerializeField]
    AudioSource chewingSound;
    [SerializeField]
    AudioSource harvestSound;
    [SerializeField]
    AudioSource raidSound;
    [SerializeField]
    AudioSource winSound;
    [SerializeField]
    AudioSource battleLooseSound;
    [SerializeField]
    AudioSource starvingLooseSound;
    [SerializeField]
    AudioSource warriorSound;
    [SerializeField]
    AudioSource peasantSound;
    #endregion
    
    void Start()
    {
        Debug.Log("GameManager");
        backSound.Play();
        raidCounterInDays = raidTimerTimeInDays;        
        raidTimerTime = oneDayInSec * raidTimerTimeInDays;
        currentRaidTimerTime = raidTimerTime;
        harvestTimer.SetTimerTime (oneDayInSec*HarvestInDays);
        peasantTimer.SetTimerTime(peasantHiringTimeInDays*oneDayInSec);
        warriorTimer.SetTimerTime(warriorHiringTimeInDays * oneDayInSec);
        daysTimer.SetTimerTime( oneDayInSec);
        UpdateRessourses();
    }
    private void Update()
    {
        if (isPaused)
        {
            MuteAll();
        }
        if (daysTimer.tick) 
        {
            chewingSound.Play();
            wheatCount -= warriorCount * warriorEatWheat;
            if (wheatCount < 0)
            {
                gameOverInfoText.text = $"Вы не справились\nДеревня просуществовала {daysCounter} дней";
                starvingLooseSound.Play();
                GameOver("Ваша деревня \nВЫМЕРЛА\nот голода");
            }            
            raidCounterInDays--;
            daysCounter++;
        }

        if (wheatCount < warriorPrice)
        {
            hireWarriorButton.interactable = false;
        }
        
        if (harvestTimer.tick)
        {
            harvestSound.Play();
            wheatCount += peasantCount * peasantMakingWheat;

            if (wheatCount > warriorPrice & hireWarriorButton.interactable == false)
            {
                hireWarriorButton.interactable = true; ;
            }
            if (wheatCount>=winTarget)
            {
                Time.timeScale = 0;
                WinGame();
            }
        }
        currentRaidTimerTime -= Time.deltaTime;
        raidTimerImage.fillAmount = currentRaidTimerTime / raidTimerTime;
        if (currentRaidTimerTime <= 0)
        {
            if (!isWin)
            {
                if (nextRaidWarriorsAmount != 0)
                {
                    Raid();
                }
                allRaidsCount++;

                if (allRaidsCount % 3 == 0) // каждые 3 нападения увеличивает количество врагов на 2
                {
                    nextRaidWarriorsAmount += 2;
                }
                raidCounterInDays = raidTimerTimeInDays;
                currentRaidTimerTime = raidTimerTime;
                daysTimer.SetTimerTime(oneDayInSec); 
            }
        }
        UpdateRessourses();
    }

    public void FinishRaid()
    {
        currentRaidTimerTime = raidTimerTime;
        raidInfoCanvas.enabled = false;        
        Time.timeScale = 1;
        daysCounter++;
        daysTimer.SetTimerTime(oneDayInSec);
        raidSound.Stop();
        
    }
    public void IncreasePeasantsAmount()
    {
        peasantCount = Convert.ToInt32(peasantCounterText.text);
        peasantCount++;
        peasantSound.Play();
        peasantCounterText.text = peasantCount.ToString();
    }
    public void IncreaseWarriorAmount()
    {                
            warriorSound.Play();
            warriorCount++;
            wheatCount -= warriorPrice;
            UpdateRessourses();    
    }
    void Raid()
    {        
        Debug.Log("Начало рейда");        
        raidSound.Play();
        
        if (warriorCount < nextRaidWarriorsAmount)
        {
            Time.timeScale = 0;
            gameOverInfoText.text = $"На деревню напали {nextRaidWarriorsAmount} вражеских воинов," +
                                    $"\nНа защите деревни стояло {warriorCount} воинов" +
                                    $"\nВы не справились\nДеревня просуществовала {daysCounter} дней" +
                                    $"\nДеревня пережила {allRaidsCount} нападений" +
                                    $"\nЗа всё время погибло {warriorDeaths} воинов";
            warriorDeaths += warriorCount;
            warriorCount = 0;
            battleLooseSound.Play();
            GameOver("Ваша деревня \nЗАХВАЧЕНА");
        }
        else
        {            
            Time.timeScale = 0;
            warriorCounterText.text = warriorCount.ToString();
            timeToRaidText.text = "0";
            raidInfoCanvas.enabled = true;            
            raidInfoText.text = $"На вас напало {nextRaidWarriorsAmount} единиц врага" +
                                $"\nНа защите вашей деревни стоит {warriorCount} единиц воинов" +
                                $"\nОсталось ваших воинов :{warriorCount - nextRaidWarriorsAmount}";
            warriorCount -= nextRaidWarriorsAmount;
            warriorDeaths += nextRaidWarriorsAmount;
        }   
        Debug.Log("Конец рейда");
    }
    void GameOver(string text)
    {
        Time.timeScale = 0;
        gameCanvas.enabled = false;
        raidInfoCanvas.enabled = false;
        gameOverCanvas.enabled = true;
        gameOverText.text = text;
    }
    void WinGame()
    {
        winSound.Play();
        isWin = true;
        winInfoText.text =  $"Вы достигли цели в 2000 единиц пшеницы за {daysCounter} дней." +                            
                            $"\nЗа время существования деревни вы" +
                            $"\nПережили {allRaidsCount-3} нападений" +
                            $"\nуничтожили {warriorDeaths} врагов";
        winCanvas.enabled = true;
    }
    void UpdateRessourses()
    {
        peasantCounterText.text = peasantCount.ToString();
        warriorCounterText.text = warriorCount.ToString();
        wheatCounterText.text = wheatCount.ToString();
        daysCounterText.text = daysCounter.ToString();
        timeToRaidText.text = raidCounterInDays.ToString();
        nextRaidWarriorsAmountText.text = nextRaidWarriorsAmount.ToString();
    }
    void MuteAll()
    {
        chewingSound.Stop();
        harvestSound.Stop();
        //backSound.Stop();
    }


}
