using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPower : MonoBehaviour
{
    public Slider powerSlider;
    public float sprintValue = 300f;
    public float dodgeValue = 500f;
    public float recoverValue = 10f;
    private float acceleration;
    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {  
        // if player is sprinting or dodged, then it will use power
        if (player.IsSprinting() || player.Dodged())
        {
            if (player.IsSprinting())
            {
                powerSlider.value -= sprintValue * Time.deltaTime;
            }
            if (player.Dodged())
            {
                powerSlider.value -= dodgeValue;
                
            }
        }
    

        else {
            if (powerSlider.value < powerSlider.maxValue)
            {
                acceleration = (25000f - powerSlider.value)/recoverValue;
                powerSlider.value += acceleration * Time.deltaTime;
            }
            else {
                powerSlider.value = Mathf.RoundToInt(powerSlider.value);
            }
        }
    }
}
