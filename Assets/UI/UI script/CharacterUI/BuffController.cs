using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffController : MonoBehaviour
{
    //private GameObject player;
    public GameObject InvincibleBuffSymbol;
    public GameObject AttackBuffSymbol;
    public GameObject DefenceBuffSymbol;

    // Update is called once per frame
    void Update()
    {
        Animals buffState = GetComponent<Animals>();
        // invincible buff
        if (buffState.PlayerInvincible)
        {
            InvincibleBuffSymbol.SetActive(true);
        }
        else {
            InvincibleBuffSymbol.SetActive(false);
        }

        // attack buff
        if (buffState.PlayerAttackBuff)
        {
            AttackBuffSymbol.SetActive(true);
        }
        else {
            AttackBuffSymbol.SetActive(false);
        }

        // defence buff
        if (buffState.PlayerDefenceBuff)
        {
            DefenceBuffSymbol.SetActive(true);
        }
        else {
            DefenceBuffSymbol.SetActive(false);
        }
    }
}
