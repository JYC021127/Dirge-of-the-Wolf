using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code reference: https://www.cnblogs.com/Akishimo/p/5057542.html
public class Animals : MonoBehaviour
{
    public float Health;
    [SerializeField]
    private float MaxHealth;
    public float Damage = 200f;
    public GameObject particle;
    public Transform BloodPosition;

    public float maxSpeed = 10;

    public float maxForce = 100;

    public float mass = 1;

    protected float sqrMaxSpeed;

    public Vector3 velocity;

    private Steering[] steerings;

    private Vector3 steeringForce;

    protected Vector3 acceleration;

    public bool isPlanar;

    public float damping = 0.9f;

    private float timer;

    private float interval = 0.2f;
    private float prevEnemyDamage = 0;
    private float prevBossDamage = 0;
    private float prevPlayerDamage = 0;
    private float prevSpiritDamage = 0;
    public bool PlayerInvincible;
    public bool PlayerAttackBuff;
    public bool PlayerDefenceBuff;
    private float invincibleTimer = 10f;
    private float attackTimer = 10f;
    private float DefenceTimer = 20f;
    private bool invStartTiming;
    private float invRemainingTime;
    private bool attStartTiming;
    private float attRemainingTime;
    private bool defStartTiming;
    private float defRemainingTime;

    // Start is called before the first frame update
    protected void Start()
    {
        // Initialize variables and references to components.
        steeringForce = Vector3.zero;
        steerings = GetComponents<Steering>();
        timer = 0;
        sqrMaxSpeed = maxSpeed * maxSpeed;
        MaxHealth = Health;
    }

    // Update is called once per frame
    public void Update()
    {
        timer += Time.deltaTime;
        steeringForce = Vector3.zero;
        if (timer > interval)
        {
            foreach (Steering s in steerings)
            {
                if (s.enabled)
                {
                    steeringForce += s.Force() * s.weight;
                }

                steeringForce = Vector3.ClampMagnitude(steeringForce, maxForce);
                acceleration = steeringForce / mass;
                timer = 0;
            }
        }
        GameObject boss = GameObject.FindGameObjectWithTag("Boss");
        if (boss != null && prevBossDamage == 0)
        {
            prevBossDamage = boss.GetComponent<Animals>().GetDamage();
        }
        InvincibleCounter();
        AttackCounter();
        DefenceCounter();
    }

    // Cause damage, and delete enemy when health reaches 0
    public virtual void TakeDamage(float amount)
    {
        Health -= amount;
        GameObject getHit = Instantiate(particle, BloodPosition.position, Quaternion.identity, this.transform);

        Destroy(getHit, 2f);
    }

    public float GetMaxHealth()
    {
        return MaxHealth;
    }

    public void SetDamage(float damage)
    {
        Damage = damage;
    }

    public float GetDamage()
    {
        return Damage;
    }

    public void IncreaseDamage(float times)
    {
        SetDamage(Damage * times);
    }

    public void SetPlayerInvinsible(bool invincible)
    {
        PlayerInvincible = invincible;
    }

    public void SetPlayerAttackBuff(bool buff)
    {
        PlayerAttackBuff = buff;
    }

    public void SetPlayerDefenceBuff(bool buff)
    {
        PlayerDefenceBuff = buff;
    }

    // Counter for invincible buff
    private void InvincibleCounter()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject boss = GameObject.FindGameObjectWithTag("Boss");
        if (invStartTiming == false && PlayerInvincible)
        {
            if (enemies.Length > 0 && prevEnemyDamage == 0)
            {
                prevEnemyDamage = enemies[0].GetComponent<Animals>().GetDamage();
            }
            invRemainingTime = invincibleTimer;
            invStartTiming = true;
            if (enemies.Length > 0)
            {
                foreach (GameObject enemy in enemies)
                {
                    enemy.GetComponent<Animals>().SetDamage(0);
                }
            }
            if (boss != null)
            {
                boss.GetComponent<Animals>().SetDamage(0);
            }
        }
        else if (invStartTiming == true)
        {
            if (invRemainingTime > 0)
            {
                invRemainingTime -= Time.deltaTime;
            }
            else
            {
                // Set damage back to original values
                if (enemies.Length > 0)
                {
                    foreach (GameObject enemy in enemies)
                    {
                        enemy.GetComponent<Animals>().SetDamage(prevEnemyDamage);
                    }
                }
                if (boss != null)
                {
                    boss.GetComponent<Animals>().SetDamage(prevBossDamage);
                }
                invStartTiming = false;
                invRemainingTime = invincibleTimer;
                PlayerInvincible = false;
            }
        }
    }

    // Counter for attack buff
    private void AttackCounter()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (attStartTiming == false && PlayerAttackBuff)
        {
            if (prevPlayerDamage == 0)
            {
                prevPlayerDamage = player.GetComponent<Animals>().GetDamage();
            }

            attRemainingTime = attackTimer;
            attStartTiming = true;
            player.GetComponent<Animals>().IncreaseDamage(2f);
        }
        else if (attStartTiming == true)
        {
            if (attRemainingTime > 0)
            {
                attRemainingTime -= Time.deltaTime;
            }
            else
            {
                player.GetComponent<Animals>().SetDamage(prevPlayerDamage);

                attStartTiming = false;
                attRemainingTime = attackTimer;
                PlayerAttackBuff = false;
            }
        }
    }

    // Counter for defence buff
    private void DefenceCounter()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject boss = GameObject.FindGameObjectWithTag("Boss");
        if (defStartTiming == false && PlayerDefenceBuff)
        {
            if (enemies.Length > 0 && prevEnemyDamage == 0)
            {
                prevEnemyDamage = enemies[0].GetComponent<Animals>().GetDamage();
            }
            defRemainingTime = DefenceTimer;
            defStartTiming = true;
            float reducedDamage = prevEnemyDamage / 2f;
            // Set to reduced damage for all enemies
            if (enemies.Length > 0)
            {
                foreach (GameObject enemy in enemies)
                {
                    enemy.GetComponent<Animals>().SetDamage(reducedDamage);
                }
            }
            if (boss != null)
            {
                boss.GetComponent<Animals>().SetDamage(reducedDamage);
            }
        }
        else if (defStartTiming == true)
        {
            if (defRemainingTime > 0)
            {
                defRemainingTime -= Time.deltaTime;
            }
            else
            {
                // Set damage back to original values
                if (enemies.Length > 0)
                {
                    foreach (GameObject enemy in enemies)
                    {
                        enemy.GetComponent<Animals>().SetDamage(prevEnemyDamage);
                    }
                }
                if (boss != null)
                {
                    boss.GetComponent<Animals>().SetDamage(prevBossDamage);
                }
                defStartTiming = false;
                defRemainingTime = DefenceTimer;
                PlayerDefenceBuff = false;
            }
        }
    }
}
