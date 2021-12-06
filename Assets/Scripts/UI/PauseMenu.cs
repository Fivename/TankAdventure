using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using LitJson;
public class PauseMenu : MonoBehaviour
{

    private static PauseMenu _instance;
    public static PauseMenu Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.Find("PauseMenu").GetComponent<PauseMenu>();
            return _instance;
        }
    }

    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public Player player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        GameIsPaused = false;
    }
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0.0f;
        GameIsPaused = true;
    }

    public void MainMenu()
    {
        GameIsPaused = false;
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Menu");
    }
    public void SaveGame()
    {
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
