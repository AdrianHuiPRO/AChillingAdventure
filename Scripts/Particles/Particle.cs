using UnityEngine;

public class Particle : MonoBehaviour
{
    private float Timer;
    public float _SetTime = 3f;

    private void Update()
    {
        Timer += Time.deltaTime;

        if(Timer >= _SetTime)
        {
            this.gameObject.SetActive(false);
            Timer = 0f;
        }
    }
}
