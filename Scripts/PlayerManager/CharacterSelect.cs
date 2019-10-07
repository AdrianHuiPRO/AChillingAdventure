using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelect : MonoBehaviour
{
    private static CharacterSelect instance;
    private GamePlayerManager _GPM;
    private bool _Player1isIcebert = true;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            instance = null;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void FixedUpdate()
    {
        if(_GPM == null)
        {
            _GPM = FindObjectOfType<GamePlayerManager>();
        }

        if(_GPM != null)
        {
            if(_Player1isIcebert == true)
            {
                _GPM.SetControllertoIcebert = 0;
                _GPM.SetControllertoSpicegirl = 1;
            }
            else if(_Player1isIcebert == false)
            {
                _GPM.SetControllertoIcebert = 1;
                _GPM.SetControllertoSpicegirl = 0;
            } 
        }
    }

    public void Player1isIcebert()
    {
        _Player1isIcebert = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Debug.Log("Load Scene, P1 = Icebert");
    }

    public void Player2isIcebert()
    {
        _Player1isIcebert = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Debug.Log("Load Scene, P1 = SpiceGirl");
    }
}
