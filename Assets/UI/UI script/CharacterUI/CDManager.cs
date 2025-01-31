using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CDManager : MonoBehaviour
{
    public GameObject DashCD;
    public GameObject RedCD;
    public GameObject GreenCD;
    public GameObject BlueCD;
    private PlayerController player;
    private float dodgeCooldown = 4f; // same as the one in playercontroller, double check!
    private float redCooldown = 20f;
    private float greenCooldown = 20f;
    private float dodgeIncrement;
    private float redIncrement;
    private float greenIncrement;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        dodgeIncrement = 1 / dodgeCooldown;
        redIncrement = 1 / redCooldown;
        greenIncrement = 1 / greenCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject boss = GameObject.FindGameObjectWithTag("Boss");
        // Dash
        if (player.Dodged())
        {
            DashCD.GetComponent<Image>().fillAmount = 0;
        }
        if (DashCD.GetComponent<Image>().fillAmount < 1)
        {
            DashCD.GetComponent<Image>().fillAmount += dodgeIncrement * Time.deltaTime;
        }



        // Red Skill
        if (player.RedSpirit < 1 || boss == null)
        {
            RedCD.SetActive(false);
        }
        else {
            RedCD.SetActive(true);
            if (RedCD.GetComponent<Image>().fillAmount == 1)
            {
                // casted spell
                if (player.redCasted)
                {
                    RedCD.GetComponent<Image>().fillAmount = 0;
                }
            }
            // counting down
            if (RedCD.GetComponent<Image>().fillAmount < 1)
            {
                RedCD.GetComponent<Image>().fillAmount += redIncrement * Time.deltaTime;
            }
        }
        



        // Green Skill
        if (player.GreenSpirit < 1 || boss == null)
        {
            GreenCD.SetActive(false);
        }
        else {
            GreenCD.SetActive(true);
            if (GreenCD.GetComponent<Image>().fillAmount == 1)
            {
                // casted spell
                if (player.greenCasted)
                {
                    GreenCD.GetComponent<Image>().fillAmount = 0;
                }
            }
            // counting down
            if (GreenCD.GetComponent<Image>().fillAmount < 1)
            {
                GreenCD.GetComponent<Image>().fillAmount += greenIncrement * Time.deltaTime;
            }
        }
        



        // Blue Skill
        if (player.BlueSpirit < 1 || boss == null)
        {
            BlueCD.SetActive(false);
        }
        else {
            BlueCD.SetActive(true);
        }
    }
}
