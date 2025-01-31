using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject health;
    public Image healthLine;
    public GameObject victory;
    private TMP_Text text;
    private GameObject boss;
    private Scene currentScene;
    private UpdateStatus maps;
    private GameObject desert;
    private GameObject snow;
    private float currentHealth;
    private float fullHealth;
    private bool set;
    private float waitTime = 4f;
    private bool bossDead;
    // Start is called before the first frame update
    void Start()
    {
        text = health.GetComponent<TextMeshProUGUI>();
        currentScene = SceneManager.GetActiveScene();
        maps = FindObjectOfType<UpdateStatus>();
        if (maps != null)
        {
            desert = maps.transform.Find("Desert").gameObject;
            snow = maps.transform.Find("Snow").gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        boss = GameObject.FindGameObjectWithTag("Boss");
        
        if (boss != null)
        {
            GameObject enemies = GameObject.Find("Enemies");
            if (enemies != null)
            {
                enemies.SetActive(false);
            }
            currentHealth = boss.GetComponent<BossController>().Health;
            if (!set)
            {
                fullHealth = boss.GetComponent<BossController>().Health;
                set = true;
            }

            if (currentHealth > 0)
            {
                healthLine.fillAmount = currentHealth/fullHealth;
                setText();
            }
            else {
                text.SetText("");
                healthBar.SetActive(false);
                //boss.GetComponent<BossController>().setDead();
                bossDead = true;
            }
        }

        // show victory after death animation
        if (bossDead == true)
        {
            if (waitTime >= 0)
            {
                waitTime -= Time.deltaTime;
            }
            else
            {
                Victory();
            }
        }
    }

    private void setText()
    {
        health.SetActive(true);
        healthBar.SetActive(true);
        float percentage = currentHealth / fullHealth * 100;
        text.SetText(percentage.ToString("F0") + "%");
    }

    public void Victory()
    {
        Cursor.visible = true; 
        Cursor.lockState = CursorLockMode.Confined;
        PlayerAudio BGM = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAudio>();
        if (currentScene.name == "ICE")
        {
            BGM.PlayVictoryBGM();
        }
        else {
            BGM.NormalVictoryBGM();
        }
        victory.SetActive(true);

        // updating game progress
        if (maps != null)
        {
            // current is grass, make desert playable
            if (currentScene.name == "mainscene")
            {
                if (!desert.activeSelf)
                {
                    desert.SetActive(true);
                }
            }
            // current is desert, make ice playable
            else if (currentScene.name == "DesertScene")
            {
                if (!snow.activeSelf)
                {
                    snow.SetActive(true);
                }
            }
        }
    }
}
