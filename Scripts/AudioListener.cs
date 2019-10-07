using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioListener : MonoBehaviour
{
    //pan needs to be -1 to 1
    // private void Update()
    // {
    //     //we get the camHeight only to get the camWidth
    //     camHeight = 2 * Camera.main.orthographicSize;
    //     //We get the cam width so that we can compare it to the position of the audio source
    //     camWidth = height * Camera.main.aspect;
    //     //Half width because we want the position relative to the center
    //     halfWidth = camWidth / 2;
    //     //compare objX to camera's X
    //     objX = source.transform.pos.x;
    //     camX = Camera.main.transform.pos.x;
    //     //Get object's distance to camera's center to calculate the pan 
    //     objDistToCamCenter = objX - camX;
    //     if(Mathf.Abs(objDistToCamCenter < halfWidth))
    //     {
    //         //Therefore the Object is visible
            
    //         //so calculate the pan value
    //         newPan = objDistToCamCenter / halfWidth;
    //         source.pan = newPan;
    //         //can use this value to calculate the volume too, something roughly like this:
    //         source.volume = newPan / 2;
    //     }
    //     else
    //     {
    //         //Object is not on screen
            
    //         //therefore pan doesn't matter
    //         if(source.volume != 0.0f);
    //         {
    //             source.volume = 0.0f;
    //         }
            
    //     }

    //     Mathf.Sign(-45) == -1
    //     Mathf.Sign(45) == 1
    // }
}
