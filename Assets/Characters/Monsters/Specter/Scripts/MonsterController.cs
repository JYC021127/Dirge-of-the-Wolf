using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MonsterController : Animals
{
    private GameObject player;
    private Transform playerTransform;
    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    public float attackRange = 5f;
    public Animator MonsterAnimator;
    public WeaponCollider weaponCollider;
    private float Speed;
    private bool isActing = false;
    private bool isDead = false;
    public GameObject blueElementPrefab;
    public GameObject redElementPrefab;
    public GameObject greenElementPrefab;

    public Image HealthBar;

    void Start()
    {
        // Initialize references and settings at the start of the game.
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        Speed = navMeshAgent.speed;
        navMeshAgent.speed = 0;
        player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform;
    }

    void Update()
    {
        if (Health <= 0 && isDead == false)
        {
            // Handle the case where the monster is defeated.
            isDead = true;
            navMeshAgent.speed = 0;
            MonsterAnimator.SetTrigger("Dead");
            SpawnSprit();
            Destroy(gameObject, 2f);
            return;
        }

        if (isDead == false)
        {
            // Check the distance to the player and decide whether to attack or chase.
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            if (distanceToPlayer <= attackRange)
            {
                // If the player is within the attack range, trigger an attack.
                MonsterAnimator.SetFloat("Move", 0f);
                navMeshAgent.speed = 0;
                MonsterAnimator.SetTrigger("Attack");
            }
            else
            {
                if (isActing == false)
                {
                    // If not attacking, start chasing the player.
                    Chase();
                }
            }
        }
    }

    public override void TakeDamage(float amount)
    {
        // Handle taking damage and update health bar.
        float fill = amount / GetMaxHealth();
        Health -= amount;
        HealthBar.fillAmount -= fill;
        if (Health <= 0)
        {
            HealthBar.transform.parent.gameObject.SetActive(false);
        }
        
        GameObject getHit = Instantiate(particle, BloodPosition.position, Quaternion.identity, this.transform);

        Destroy(getHit, 2f);
        
    }

    private void SpawnSprit()
    {
        // Randomly spawn spirit elements upon defeat.
        float roll = Random.Range(0f, 1f); 

        Vector3 position = new Vector3(transform.position.x, transform.position.y+1, transform.position.z);
        if (roll <= 0.90f) 
        {
            GameObject blue = Instantiate(blueElementPrefab, position, Quaternion.identity);
            //player.GetComponent<PlayerController>().BlueSpirit += 1;
        }
        else if (roll <= 0.95f)
        {
            GameObject red = Instantiate(redElementPrefab, position, Quaternion.identity);
            //player.GetComponent<PlayerController>().RedSpirit += 1;
        }
        else 
        {
            GameObject green = Instantiate(greenElementPrefab, position, Quaternion.identity);
            //player.GetComponent<PlayerController>().GreenSpirit += 1;
        }
    }

    private void Chase()
    {
        // Start chasing the player by setting the destination.
        navMeshAgent.speed = Speed;
        MonsterAnimator.SetFloat("Move", 1f);
        navMeshAgent.SetDestination(playerTransform.position);
    }

    private void StartAttack()
    {
        // Start the attack animation and disable movement.
        MonsterAnimator.SetBool("CanMove", false);
        navMeshAgent.speed = 0;
        isActing = true;
        MonsterAnimator.ResetTrigger("Attack");
    }

    private void StartDMG()
    {
        // Start the damage phase.
        weaponCollider.gameObject.SetActive(true);
    }

    private void StartDMG1()
    {
        // Start the damage phase.
        weaponCollider.gameObject.SetActive(true);
    }

    private void EndDMG()
    {
        // End the damage phase.
        weaponCollider.gameObject.SetActive(false);
    }

    private void EndDMG1()
    {
        // End the damage phase.
        weaponCollider.gameObject.SetActive(false);
    }

    private void EndAttack()
    {
        // End the attack animation and allow movement.
        isActing = false;
        navMeshAgent.speed = Speed;
        MonsterAnimator.SetBool("CanMove", true);
        weaponCollider.OnDisable();
    }
}