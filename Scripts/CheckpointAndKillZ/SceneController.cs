using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public KeyCode _MainMenu;
    public KeyCode _Level1;
    public KeyCode _Level2;
    public KeyCode _Level3;
    public KeyCode _Level4;
    public KeyCode _Level5;
    public KeyCode _Level6;
    public KeyCode _Level7;
    public KeyCode _Level8;
    public KeyCode _Level9;
    public KeyCode _Level10;
    public KeyCode _LevelWin;
    public KeyCode _ResetScene;

    public string _MainMenuScene;
    public string _Level1Scene;
    public string _Level2Scene;
    public string _Level3Scene;
    public string _Level4Scene;
    public string _Level5Scene;
    public string _Level6Scene;
    public string _Level7Scene;
    public string _Level8Scene;
    public string _Level9Scene;
    public string _Level10Scene;
    public string _YouwinScene;

    private void Update() 
    {
        if(Input.GetKeyDown(_ResetScene))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if(Input.GetKeyDown(_MainMenu))
        {
            SceneManager.LoadScene(_MainMenuScene);
        }

        if(Input.GetKeyDown(_Level1))
        {
            SceneManager.LoadScene(_Level1Scene);
        }

        if(Input.GetKeyDown(_Level2))
        {
            SceneManager.LoadScene(_Level2Scene);
        }

        if(Input.GetKeyDown(_Level3))
        {
            SceneManager.LoadScene(_Level3Scene);
        }
        if(Input.GetKeyDown(_Level4))
        {
            SceneManager.LoadScene(_Level4Scene);
        }
        if(Input.GetKeyDown(_Level5))
        {
            SceneManager.LoadScene(_Level5Scene);
        }
        if(Input.GetKeyDown(_Level6))
        {
            SceneManager.LoadScene(_Level6Scene);
        }
        if(Input.GetKeyDown(_Level7))
        {
            SceneManager.LoadScene(_Level7Scene);
        }
        if(Input.GetKeyDown(_Level8))
        {
            SceneManager.LoadScene(_Level8Scene);
        }
        if(Input.GetKeyDown(_Level9))
        {
            SceneManager.LoadScene(_Level9Scene);
        }
        if(Input.GetKeyDown(_Level10))
        {
            SceneManager.LoadScene(_Level10Scene);
        }
        if(Input.GetKeyDown(_LevelWin))
        {
            SceneManager.LoadScene(_YouwinScene);
        }
    }
}
