using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class defeatManager : MonoBehaviour

{
    public GameObject defeat;
    public GameObject victory;
    private float remainingTime = 5 * 60f;
    private bool bossDied;
    private float deathAnimationTime = 2.4f;
    private GameObject boss;

    void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Boss");
    }
    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        // if player dies, then show defeat page
        if (player.GetComponent<Animals>().Health <= 0)
        {
            // hide all wolf spirits
            GameObject[] wolfSpirits = GameObject.FindGameObjectsWithTag("WolfSpirit");
            foreach (GameObject spirit in wolfSpirits)
            {
                spirit.SetActive(false);
            }
            bossDied = true;
            if (deathAnimationTime > 0)
            {
                deathAnimationTime -= Time.deltaTime;
            }
            else {
                Destroy(GameObject.FindGameObjectWithTag("Boss"));
                Defeated();
            }
        }
        if (!bossDied)
        {
            if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
                if (boss != null) {
                    if (boss.GetComponent<BossController>().Health <= 0)
                    {
                        bossDied = true;
                    }
                }
            }
            else
            {
                if (boss != null)
                {
                    Destroy(boss);
                    GameObject[] missiles = GameObject.FindGameObjectsWithTag("Missile");
                    foreach (GameObject missile in missiles)
                    {
                        missile.SetActive(false);
                    }
                    Defeated();
                }
            }
        }
    }

    public void Defeated()
    {
        Destroy(victory);
        Cursor.visible = true; 
        Cursor.lockState = CursorLockMode.Confined;
        defeat.SetActive(true);
        PlayerAudio BGM = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAudio>();
        BGM.NormalDefeatBGM();
    }
}