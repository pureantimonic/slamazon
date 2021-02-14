using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using TMPro;

public class GUIScript : MonoBehaviour
{
    enum State
    {
        Playing,
        Pause,
        Setting,
        End
    }
    State gameState = State.Playing;
    public GameObject pauseMenuPanel;
    public GameObject settingPanel;
    public GameObject endPanel;

    public GameObject timeDisplay;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI PackageText;


    public int minLeft = 1;
    public int secondLeft = 10;
    public bool timeGone = false;

    public int droneHealth;
    public int gameScore;
    public int deliveredPackage;

    public Slider healthBar;

    public AudioMixer audioMixer;

    public DroneController droneContriller;

    // Start is called before the first frame update
    void Start()
    {
        minLeft = 1;
        secondLeft = 10;
        if(timeDisplay != null)
        {
            timeDisplay.GetComponent<Text>().text = "0"+ minLeft + ":" + secondLeft;
        } 
    }

    // Update is called once per frame
    
    void Update()
    {
        if((timeDisplay != null) && !timeGone && secondLeft > 0)
        {
            StartCoroutine(TimerTake());
        }
    }
    

    public void OnGamePause(InputValue val)
    {
        switch (gameState)
        {
            case State.Playing:
                Pause();
                break;
            case State.Pause:
                Resume();
                break;
            case State.Setting:
                ReturnFromSetting();
                break;
            case State.End:
                LoadMainMenu();
                break;

        }
        
    }

    public void GameEnd()
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        gameState = State.End;
        pauseMenuPanel.SetActive(false);
        settingPanel.SetActive(false);
        endPanel.SetActive(true);
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        gameState = State.Playing;
    }

    public void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
        gameState = State.Pause;
    }

    public void GoSettingMenu()
    {
        pauseMenuPanel.SetActive(false);
        settingPanel.SetActive(true);
        gameState = State.Setting;
    }

    public void ReturnFromSetting()
    {
        settingPanel.SetActive(false);
        pauseMenuPanel.SetActive(true);
        gameState = State.Pause;
    }

    public void LoadMainMenu()
    {
        gameState = State.End;
        SceneManager.LoadScene(0);
    }

    public void PlayScene()
    {
        Time.timeScale = 1f;
        gameState = State.Playing;
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        gameState = State.End;
        Application.Quit();
    }
    
    IEnumerator TimerTake()
    {
        timeGone = true;
        yield return new WaitForSeconds(1);
        secondLeft -= 1;
        if (secondLeft < 10)
        {
            timeDisplay.GetComponent<Text>().text = "0" + minLeft + ":0" + secondLeft;
        }
        else
        {
            timeDisplay.GetComponent<Text>().text = "0" + minLeft + ":" + secondLeft;
        }
        if(secondLeft == 0)
        {
            if(minLeft == 0)
            {
                GameEnd();
            }
            else
            {
                minLeft -= 1;
                secondLeft = 60;
            }
        }
        timeGone = false;
    }

    public void SetHealth(int health)
    {
        healthBar.value = health;
        droneHealth = health;
    }

    public void SetMaxHealth(int health)
    {
        healthBar.maxValue = health;
        healthBar.value = health;
        droneHealth = health;
    }

    public void UpdateGUI(int score, int delivered)
    {
        gameScore = score;
        deliveredPackage = delivered;
        ScoreText.text = gameScore.ToString("F0");
        PackageText.text = deliveredPackage.ToString();
    }
    public void SetVolume(float volume)
    {
        //Debug.Log(volume);
        audioMixer.SetFloat("Volume", volume);
    }

    public void SetMouseSensitivity(float sensitivity)
    {
        droneContriller.SetAngularSpeed(sensitivity/40 + 0.01F);
    }
}
