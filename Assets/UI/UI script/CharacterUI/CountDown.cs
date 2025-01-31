using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountDown : MonoBehaviour
{
    [SerializeField] private TMP_Text timeLeft;
    [SerializeField] private TMP_Text bossInform;
    public float remainingTime = 5 * 60f; // maximum 5 minutes
    private int minute;
    private int second;

    // Start is called before the first frame update
    void Start()
    {
        minute = (int)remainingTime / 60;
        second = (int)remainingTime % 60;
        SetText();
    }

    // Update is called once per frame
    void Update()
    {
        if (remainingTime > 0)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            GameObject boss = GameObject.FindGameObjectWithTag("Boss");
            // do countdown only when boss and player are both alive
            if (player.GetComponent<Animals>().Health > 0)
            {

                if (boss == null || boss != null && boss.GetComponent<BossController>().Health > 0)
                {
                    // inform player when boss arrives
                    if (minute == 2)
                    {
                        if (second == 00)
                        {
                            bossInform.SetText("Boss has arrived!");
                        }
                    }
                    if (minute == 1 && second == 55)
                    {
                        bossInform.SetText("");
                    }

                    if (minute == 0 && second <= 10)
                    {
                        bossInform.SetText(second + " seconds remaining");
                    }
                    remainingTime -= Time.deltaTime;
                }  
            }
            SetText();
        }

        // game ends
        else
        {
            timeLeft.SetText("");
        }
        
    }

    // boss comes out 3 minutes in
    // colour of text will change to red
    private void SetText()
    {
        minute = (int)remainingTime / 60;
        second = (int)remainingTime % 60;
        if (minute == 2) {
            timeLeft.GetComponent<TextMeshProUGUI>().color = new Color32(195, 162, 71, 255);
        }

        if (minute <= 1) {
            timeLeft.GetComponent<TextMeshProUGUI>().color = new Color32(195, 76, 71, 255);
        }

        timeLeft.SetText(minute.ToString().PadLeft(2, '0') + "'" + second.ToString().PadLeft(2, '0') + "''");
    }
}
