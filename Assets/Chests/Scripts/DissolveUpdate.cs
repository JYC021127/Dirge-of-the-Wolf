using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveUpdate : MonoBehaviour
{
    private new Renderer renderer; // Reference to the Renderer component.
    private float countDown = 3f; // Time in seconds for the dissolve effect.

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>(); // Get the Renderer component attached to this GameObject.
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent.name == "Claimed") // Check if the parent's name is "Claimed".
        {
            // Disable the BoxCollider of the parent object.
            GetComponent<MeshCollider>().enabled = false;

            // Calculate the dissolve speed based on the current "_DissolveThreshold" value.
            float speed = (2 - renderer.material.GetFloat("_DissolveThreshold")) / countDown * Time.deltaTime;

            if (renderer.materials.Length > 1) // Check if there are multiple materials on the Renderer.
            {
                // Divide the speed by 2 if there are multiple materials.
                speed = (2 - renderer.material.GetFloat("_DissolveThreshold")) / countDown * Time.deltaTime;

                // Update "_DissolveThreshold" and "_EdgeSize" for both materials.
                renderer.materials[0].SetFloat("_DissolveThreshold", Mathf.MoveTowards(renderer.material.GetFloat("_DissolveThreshold"), 1, speed));
                renderer.materials[1].SetFloat("_DissolveThreshold", Mathf.MoveTowards(renderer.material.GetFloat("_DissolveThreshold"), 1, speed));
            }

            // Update "_DissolveThreshold" and "_EdgeSize" for the single material.
            renderer.material.SetFloat("_DissolveThreshold", Mathf.MoveTowards(renderer.material.GetFloat("_DissolveThreshold"), 1, speed));

            // Check if the dissolve threshold is greater than or equal to 0.8.
            if (renderer.material.GetFloat("_DissolveThreshold") >= 0.8)
            {
                // Destroy the parent GameObject.
                Destroy(transform.parent.gameObject);
            }
        }
    }
}
