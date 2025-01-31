using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class UpdateStatus : MonoBehaviour
{
    //public GameObject[] objectsToPreserve;
    private static UpdateStatus instance;
    void Start()
    {
        // making sure that preserved objects dont duplicate when switching multiple scenes
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        // foreach (GameObject map in objectsToPreserve)
        // {
        //     DontDestroyOnLoad(map);
        // }
    }
}
