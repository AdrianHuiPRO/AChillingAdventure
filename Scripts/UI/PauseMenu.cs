using InControl;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public PlayerController[] _Players;
    public GameObject _PauseMenu;
    public GameObject _OptionsMenu;
    public GameObject _RestartMenu;
    public GameObject _MainMenuMenu;
    public int _MainMenuScene = 0;

    public InputDevice Devices1;
    public InputDevice Devices2;

    private bool GameIsPaused = false;

    [SerializeField]
    private GameObject _OptionsButton;
    [SerializeField]
    private GameObject _RestartButton;
    [SerializeField]
    private GameObject _ResumeButton;
    [SerializeField]
    private GameObject _MainMenuButton;

    public EventSystem _LeavingMenu;
    
    
    
    void Update()
    {   
        _Players = GameObject.FindObjectsOfType<PlayerController>();
        Devices1 = _Players[0].Device;
        Devices2 = _Players[1].Device;

        if(Devices1 != null && Devices1.CommandWasPressed)
        {
            if(GameIsPaused == true) // FIXME
            {
                Resume();
            }
            else
            {
                Pause();
                foreach (var item in _Players)
                {
                    item.enabled = false;
                    item.GetComponent<ScriptCheck>().enabled = false;
                }
            }  
        }
        else if(Devices2 != null && Devices2.CommandWasPressed)
        {
            if(GameIsPaused == true)
            {
                Resume();
            }
            else
            {
                Pause();
                foreach (var item in _Players)
                {
                    item.enabled = false;
                    item.GetComponent<ScriptCheck>().enabled = false;
                }
            }  
        }

        if(GameIsPaused == true)
        {
            if(Devices1.Action2.WasPressed || Devices2.Action2.WasPressed)
            {
                if(_PauseMenu.activeInHierarchy == true)
                {
                    Resume();
                    _LeavingMenu.SetSelectedGameObject(_ResumeButton);
                }
                else if(_RestartMenu.activeInHierarchy == true)
                {
                    LeavingMenu();
                    _LeavingMenu.SetSelectedGameObject(_RestartButton);
                }
                else if(_OptionsMenu.activeInHierarchy == true)
                {
                    LeavingMenu();
                    _LeavingMenu.SetSelectedGameObject(_OptionsButton);
                }
                else if(_MainMenuMenu.activeInHierarchy == true)
                {
                    LeavingMenu();
                    _LeavingMenu.SetSelectedGameObject(_MainMenuButton);
                }
            }
        }
    
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameIsPaused == true)
            {
                Resume();
            }
            else
            {
                Pause();
                foreach (var item in _Players)
                {
                    item.enabled = false;
                    item.GetComponent<ScriptCheck>().enabled = false;
                }
            }
        }
    }


    public void Pause()
    {
        _PauseMenu.SetActive(true);
        _OptionsMenu.SetActive(false);
        _RestartMenu.SetActive(false);
        _MainMenuMenu.SetActive(false);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Resume()
    {
        _PauseMenu.SetActive(false);
        _OptionsMenu.SetActive(false);
        _RestartMenu.SetActive(false);
        _MainMenuMenu.SetActive(false);
        foreach (var item in _Players)
        {
            item.enabled = true;
            item.GetComponent<ScriptCheck>().enabled = true;
        }
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void LeavingMenu()
    {
        _PauseMenu.SetActive(true);
        _OptionsMenu.SetActive(false);
        _RestartMenu.SetActive(false);
        _MainMenuMenu.SetActive(false);
        Time.timeScale = 0f;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(_MainMenuScene);
    }
}
