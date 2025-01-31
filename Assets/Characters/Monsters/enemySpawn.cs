using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawn : MonoBehaviour
{
    [SerializeField] private GameObject enemy; // the enemy
    [SerializeField] public int numOfEnemies = 3;  // number of enemies to spawn at this location
    [SerializeField] private float spawnRadius = 20; 
    [SerializeField] private float stepTime = 5f; // time between spawn after all gets killed    
    private bool startTiming;
    private float remainingTime;
    private bool bossArrived;

    // Start is called before the first frame update
    void Start()
    {
        spawn();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject boss = GameObject.FindGameObjectWithTag("Boss");
        if (boss != null)
        {
            bossArrived = true;
            numOfEnemies = 0;
            //Destroy(this.gameObject);
        }

        int groupCount = transform.childCount;

        // check if there are still enemys in that location
        if (groupCount <= 0) {
            if (startTiming == false)
            {
                remainingTime = stepTime;
                startTiming = true;
            }
            else {
                if (remainingTime > 0)
                {
                    remainingTime -= Time.deltaTime;
                }
                else
                {
                    startTiming = false;
                    remainingTime = stepTime;

                    // make sure not to spawn right in the player' face
                    // enemy includes player, and boss
                    // ADD MORE ENEMY TAGS HERE IF MORE ARE CREATED!!!!!!!!!
                    GameObject[] player = GameObject.FindGameObjectsWithTag("Player");

                    GameObject[] enemies = new GameObject[player.Length];
                    player.CopyTo(enemies, 0);


                    int check = 0;
                    foreach (GameObject enemy in enemies)
                    {
                        // make sure not to count the object itself as an enemy
                        if (enemy.name != transform.name && 
                            getDistance(enemy) < spawnRadius * 2)
                        {
                            check++;
                        }
                    }
                    // respawn if no enemy is within radius
                    if (check == 0) 
                    {
                        spawn();
                    }
                    // will do countdown again if enemy is found within radius
                }
            }
        }
    }

    // code reference from workshop 4 solution
    // spawn a group of enemys at specified location
    private void spawn()
    {
        int num = numOfEnemies;
        for (var i = 0; i < num; i++)
        {
            // creating a new child enemy
            var enemyTransform = Instantiate(this.enemy).transform;
            enemyTransform.parent = transform;
            
            // set a random position within a radius
            var randomPosition = (Vector3) Random.insideUnitCircle * spawnRadius;
            enemyTransform.position = randomPosition;
            enemyTransform.localPosition = new Vector3(randomPosition.x, 0.0f, randomPosition.y);
        }
    }

    // returns the distance between this object and enemy object
    private float getDistance(GameObject enemy)
    {
        return Vector3.Distance(transform.position, enemy.transform.position);
    }
    
}
