using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class VictoryManager : MonoBehaviour

{
    public GameObject victory;
    private GameObject boss;

    void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Boss");

        
    }
    // Update is called once per frame
    void Update()
    {
        // if boss dies, then player win
        if (boss != null)
        {
            if (boss.GetComponent<Animals>().Health <= 0)
            {
                Victory();
            }
        }
    }

    // this won't run anymore, moved to bosshealth script
    public void Victory()
    {
        victory.SetActive(true);
    }
}