using UnityEngine;
using UnityEngine.SceneManagement;

public class WinChangeScene : MonoBehaviour
{
    public void ReachEndLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
