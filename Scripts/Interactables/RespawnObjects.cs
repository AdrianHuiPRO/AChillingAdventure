using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnObjects : MonoBehaviour
{
    public void ReEnableGameObject(GameObject item)
    {
        //Start Coroutine here
        StartCoroutine(ReEnableCoroutine(item));
    }

    IEnumerator ReEnableCoroutine(GameObject item)
    {
        yield return new WaitForSecondsRealtime(1.5f);
        if(item.GetComponent<Rigidbody>() != null)
        {
            item.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        item.SetActive(true);
    }
}