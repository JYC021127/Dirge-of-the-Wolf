using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnowDown : MonoBehaviour
{

    private bool hasTriggered = false;



    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.tag == "Boss") 
        {
            other.gameObject.GetComponent<BossController>().KnockDown();
            Destroy(gameObject, 1f);
            hasTriggered = true; 
        }
    }
}
