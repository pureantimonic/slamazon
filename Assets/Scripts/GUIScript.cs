using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GUIScript : MonoBehaviour
{
    public static bool gamePaused = false;
    public GameObject pauseMenuUI;

    public GameObject timeDisplay;
    public int secondLeft = 30;
    public bool timeGone = false;

    public Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        if(timeDisplay != null)
        {
            timeDisplay.GetComponent<Text>().text = "00:" + secondLeft;
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
        if (gamePaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
    }

    void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gamePaused = true;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void PlayScene()
    {
        Time.timeScale = 1f;
        gamePaused = false;
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
    
    IEnumerator TimerTake()
    {
        timeGone = true;
        yield return new WaitForSeconds(1);
        secondLeft -= 1;
        if (secondLeft < 10)
        {
            timeDisplay.GetComponent<Text>().text = "00:0" + secondLeft;
        }
        else
        {
            timeDisplay.GetComponent<Text>().text = "00:" + secondLeft;
        }
        timeGone = false;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }
}
