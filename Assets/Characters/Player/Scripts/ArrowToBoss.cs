using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowToBoss : MonoBehaviour
{
    private Transform BossTransform;

    void Start()
    {
        // Find and store the transform of the boss object in the scene.
        BossTransform = GameObject.FindGameObjectWithTag("Boss").transform;
    }

    void Update()
    {
        // Make the arrow point toward the boss's position.
        this.transform.LookAt(BossTransform);

        // Check the distance between the arrow and the boss, destroy the arrow if it's too close.
        if (Vector3.Distance(this.transform.position, BossTransform.position) <= 10f)
        {
            Destroy(gameObject);
        }
    }
}
