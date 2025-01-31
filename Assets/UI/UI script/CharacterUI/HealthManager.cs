using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private GameObject health;
    public Slider playerHealthSlider;
    private TMP_Text text;

    private float currentHealth;
    private float previousHealth;
    private float showHealthTime = 0.8f;
    // Start is called before the first frame update
    void Start()
    {
        text = health.GetComponent<TextMeshProUGUI>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        currentHealth = player.GetComponent<Animals>().Health;
        previousHealth = currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        currentHealth = player.GetComponent<Animals>().Health;
        playerHealthSlider.value = currentHealth;
        float difference = currentHealth - previousHealth;

        SetText(difference);

        if (currentHealth <= 0) 
        {
            health.SetActive(false);
        }
        
        // show text for 1 second
        if (health.activeSelf & showHealthTime > 0) {
            showHealthTime -= Time.deltaTime;
        }
        else
        {
            health.SetActive(false);
            showHealthTime = 0.8f;
        }
    }

    // set health text according to wether it's positive or negative
    private void SetText(float difference)
    {
        // increased health
        if (difference > 0) 
        {
            showHealthTime = 0.8f;
            health.SetActive(true);
            text.color = new Color32(108, 197, 98, 255);
            text.SetText(" +" + difference);
            previousHealth = currentHealth;
        }

        // decreased health
        if (difference < 0)
        {
            showHealthTime = 0.5f;
            health.SetActive(true);
            text.color = new Color32(200, 61, 45, 255);
            text.SetText(" " + difference.ToString());
            previousHealth = currentHealth;
        }
    }


}
