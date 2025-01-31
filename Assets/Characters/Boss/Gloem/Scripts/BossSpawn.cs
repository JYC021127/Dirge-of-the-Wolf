using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossSpawn : MonoBehaviour
{
    [SerializeField] private GameObject boss;
    private float remainingTime = 5 * 60f; // maximum 5 minutes
    private bool spawned = false;
    public GameObject arrowToBoss;
    private GameObject spawnedArrow;
    private Transform player;
    public int time = 1;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int minute = (int) remainingTime / 60;
        if (minute > 0 && !spawned)
        {
            remainingTime -= Time.deltaTime;

            // spawn boss when there's 2 minutes left
            if (minute <= time)
            {
                Spawn();
            }
        }
        if (spawnedArrow != null)
        {
            spawnedArrow.transform.position = player.position + new Vector3(0, -1, 2);
        }
    }

    private void Spawn()
    {
        if (!spawned)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            GameObject bossObject = Instantiate(boss, this.transform);
            spawnedArrow = Instantiate(arrowToBoss, player);
            //bossTransform.parent = transform;
            bossObject.transform.localPosition = Vector3.zero;
            spawned = true;
        }
    }
}