using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseSCript : MonoBehaviour
{
    [SerializeField]
    GameManager gameManager;
    
    Button playPauseButton;
    [SerializeField]
    Canvas pauseCanvas;

    [SerializeField]
    Sprite playSprite;
    [SerializeField]
    Sprite pauseSprite;
    [SerializeField]
    TMP_Text pauseText;
    [SerializeField]
    AudioSource pauseSound;
    private void Start()
    {
        Debug.Log("PauseScript");
        playPauseButton = GetComponent<Button>();
    }
    
    public void PlayPauseGame()
    {
        pauseSound.pitch = 0.2f;
        //pauseSound.reverbZoneMix = 1;
        
        playPauseButton.image.sprite = playSprite;
        Time.timeScale = 0;        
        gameManager.isPaused = true;
        pauseCanvas.enabled = true;
        pauseSound.Play();
        
    }
    public void ResumeGame()
    {
        pauseSound.pitch = 1;
        pauseSound.Stop();
        playPauseButton.image.sprite = pauseSprite;
        gameManager.isPaused = false;
        Time.timeScale = 1;
        gameManager.backSound.Play();
        pauseText.text = "Пауза";
        pauseCanvas.enabled = false;

    }
   

}
